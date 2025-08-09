namespace EventTicketingSystem.CSharp.Domain.Services;

public class ExportService
{
    public async Task<byte[]> ExportToCsv<T>(List<T> data, CsvConfiguration? config = null)
    {
        if (data == null || data.Count == 0)
            throw new ArgumentException("Data cannot be null or empty", nameof(data));

        config ??= new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true,
            MissingFieldFound = null
        };

        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        using var csvWriter = new CsvWriter(streamWriter, config);

        await csvWriter.WriteRecordsAsync(data);
        await streamWriter.FlushAsync();

        return memoryStream.ToArray();
    }

    public Task<byte[]> ExportToExcel<T>(List<T> data, string sheetName = "Export", Action<IXLWorksheet>? styleAction = null)
    {
        if (data is null || data.Count == 0)
        {
            throw new ArgumentException("Data cannot be null or empty", nameof(data));
        }

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        var properties = typeof(T).GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = properties[i].Name;
        }

        for (int row = 0; row < data.Count; row++)
        {
            for (int col = 0; col < properties.Length; col++)
            {
                var value = properties[col].GetValue(data[row])?.ToString() ?? string.Empty;
                worksheet.Cell(row + 2, col + 1).Value = value;
            }
        }

        styleAction?.Invoke(worksheet);

        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return Task.FromResult(memoryStream.ToArray());
    }

    public Task<byte[]> ExportToPdf<T>(List<T> data, string title = "Export")
    {
        using var memoryStream = new MemoryStream();
        using var document = SKDocument.CreatePdf(memoryStream, new SKDocumentPdfMetadata
        {
            Title = title,
            Author = "Event Ticketing System",
            Subject = "Data Export",
            Creator = "Export Service"
        });

        using var canvas = document.BeginPage(595, 842);

        var titlePaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 20,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
        };

        var headerPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 14,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
        };

        var cellPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 12,
            IsAntialias = true
        };

        canvas.DrawText(title, 595 / 2 - titlePaint.MeasureText(title) / 2, 50, titlePaint);

        var properties = typeof(T).GetProperties();
        float yPosition = 80;
        const float rowHeight = 20;
        const float cellPadding = 10;

        float[] columnWidths = new float[properties.Length];
        for (int i = 0; i < properties.Length; i++)
        {
            columnWidths[i] = headerPaint.MeasureText(properties[i].Name) + cellPadding * 2;
        }

        float xPosition = 50;
        for (int i = 0; i < properties.Length; i++)
        {
            canvas.DrawRect(xPosition, yPosition, columnWidths[i], rowHeight,
                new SKPaint { Color = new SKColor(240, 240, 240) });

            canvas.DrawText(properties[i].Name,
                xPosition + cellPadding,
                yPosition + rowHeight - cellPadding,
                headerPaint);

            xPosition += columnWidths[i];
        }

        yPosition += rowHeight;

        foreach (var item in data)
        {
            xPosition = 50;
            for (int i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(item)?.ToString() ?? string.Empty;

                canvas.DrawText(value,
                    xPosition + cellPadding,
                    yPosition + rowHeight - cellPadding,
                    cellPaint);

                canvas.DrawRect(xPosition, yPosition, columnWidths[i], rowHeight,
                    new SKPaint { Color = SKColors.LightGray, Style = SKPaintStyle.Stroke });

                xPosition += columnWidths[i];
            }
            yPosition += rowHeight;
        }

        document.EndPage();
        document.Close();

        return Task.FromResult(memoryStream.ToArray());
    }
}