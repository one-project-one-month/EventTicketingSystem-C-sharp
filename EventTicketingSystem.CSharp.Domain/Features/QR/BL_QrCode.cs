namespace EventTicketingSystem.CSharp.Domain.Features.QR;

public class BL_QrCode
{
    private readonly DA_QrCode _da_QrCode;
    private string QR_DIR_NAME = EnumDirectory.QrImage.ToString();
    private const int WIDTH = 300;
    private const int HEIGHT = 300;
    private const string QR_IMAGE_FILE_EXTENSION = ".jpeg";

    public BL_QrCode(DA_QrCode da_QrCode)
    {
        _da_QrCode = da_QrCode;
    }

    public async Task<Result<QrGenerateResponseModel>> Generate(QrGenerateRequestModel requestModel)
    {
        var model = new QrGenerateResponseModel();

        var response = await _da_QrCode.GenerateQr(requestModel);

        if (response.IsError)
        {
            return Result<QrGenerateResponseModel>.SystemError("Ticket information not found for the provided ticket code.");
        }

        string fileName = requestModel.TicketCode + QR_IMAGE_FILE_EXTENSION;
        string outputFileName = Path.Combine(QR_DIR_NAME, fileName);

        SaveQrImage(response.Data.QrString, outputFileName);

        response.Data.FilePath = outputFileName;

        response.Message = "QR code generated successfully.";
        return response;
    }

    public async Task<Result<QrCheckResponseModel>> Check(string qrString)
    {
        var response = await _da_QrCode.CheckQr(qrString);
        if (response.IsError)
        {
            return Result<QrCheckResponseModel>.SystemError(response.Message);
        }
        response.Message = "QR code is valid.";
        return response;

    }

    private void SaveQrImage(string qrString, string outputFileName)
    {
        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Height = WIDTH,
                Width = HEIGHT,
                Margin = 1
            }
        };

        var pixelData = writer.Write(qrString);
        var image = ConvertToImageSharp(pixelData);

        var directory = Path.GetDirectoryName(outputFileName);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory!);
        }

        image.Save(outputFileName, new JpegEncoder
        {
            Quality = 90
        });
    }

    private Image<Rgba32> ConvertToImageSharp(PixelData pixelData)
    {
        var rgbaPixels = new Rgba32[pixelData.Width * pixelData.Height];

        for (int i = 0; i < rgbaPixels.Length; i++)
        {
            int offset = i * 4;
            byte b = pixelData.Pixels[offset + 0];
            byte g = pixelData.Pixels[offset + 1];
            byte r = pixelData.Pixels[offset + 2];
            byte a = pixelData.Pixels[offset + 3];

            rgbaPixels[i] = new Rgba32(r, g, b, a);
        }

        return Image.LoadPixelData<Rgba32>(
            new ReadOnlySpan<Rgba32>(rgbaPixels),
            pixelData.Width,
            pixelData.Height
        );
    }
}