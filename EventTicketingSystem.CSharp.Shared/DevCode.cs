using System.Globalization;

namespace EventTicketingSystem.CSharp.Shared;

public static partial class DevCode
{
    private static long MaxFileSize = 5 * 1024 * 1024;
    private static byte[] key = "C4162ECDB4594969BB1040E846869706".ToByte();
    private static byte[] iv = "AC507B7DAC7B458C".ToByte();

    #region Extensions

    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T ToObject<T>(this string jsonStr)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<T>(jsonStr,
                new JsonSerializerSettings { DateParseHandling = DateParseHandling.DateTimeOffset });
            return result!;
        }
        catch
        {
            return (T)Convert.ChangeType(jsonStr, typeof(T));
        }
    }

    public static string GetEnumDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }

    public static string GenerateUlid()
    {
        return Ulid.NewUlid().ToString();
    }

    public static byte[] ToByte(this string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static bool IsSameDigit(this string input)
    {
        return input.Distinct().Count() == 1;
    }

    public static bool IsSequential(this string input)
    {
        for (var i = 1; i < input.Length; i++)
        {
            if (input[i] - input[i - 1] != 1)
            {
                return false;
            }
        }

        return true;
    }

    public static DateTime ToDateTime(this DateTime? dt)
    {
        return dt == null ? default(DateTime) : Convert.ToDateTime(dt);
    }

    public static DateTime? ToDateTime(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (DateTime.TryParse(input, out DateTime result))
            return result;

        return null;
    }

    public static DateTime? ToDateTime(this string input, string format, CultureInfo culture = null)
    {
        if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(format))
            return null;

        culture ??= CultureInfo.InvariantCulture;

        if (DateTime.TryParseExact(input, format, culture, DateTimeStyles.None, out DateTime result))
            return result;

        return null;
    }


    #endregion

    #region Validation

    public static bool IsNullOrEmpty(this object? str)
    {
        var result = true;
        try
        {
            result = str == null ||
                     string.IsNullOrEmpty(str.ToString()?.Trim()) ||
                     string.IsNullOrWhiteSpace(str.ToString()?.Trim());
        }
        catch
        {
            // ignored
        }

        return result;
    }

    public static bool IsValidEmail(this string email)
    {
        bool result = true;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email)
            {
                result = false;
            }
        }
        catch
        {
            result = false;
        }

        return result;
    }


    #endregion

    #region Thousand Separators

    public static string ToThousandSeparator<T>(this T value) where T : struct, IFormattable
    {
        return value.ToString("N", null); // 1,000,000
    }

    #endregion

    #region Date Format MM Version

    public static string ToDateTimeFormat(this DateTime date)
    {
        return date.ToString("dd-MM-yyyy"); // 08-07-2025
    }

    public static string ToDateTimeFormat(this DateTime? date)
    {
        return date.HasValue ? date.Value.ToDateTimeFormat() : string.Empty;
    }

    public static string ToDateTimeFormatLong(this DateTime date)
    {
        return date.ToString("dd-MMMM-yyyy"); // 08-July-2025
    }

    #endregion

    #region Custom Log Errors

    public static void LogExceptionError(this ILogger logger,
        Exception ex,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string methodName = "")
    {
        var fileName = Path.GetFileName(filePath);
        var message = $"File Name - {fileName} | Method Name - {methodName} | Error - {ex.ToJson()}";
        logger.LogCustomError(message);
    }

    public static void LogCustomError(this ILogger logger,
        object? str,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string methodName = "")
    {
        var fileName = Path.GetFileName(filePath);
        var message =
            $"File Name - {fileName} | Method Name - {methodName} | Result - {(str is string ? str : str!.ToJson())}";
        logger.LogError(message);
    }

    public static void LogCustomInformation(this ILogger logger,
        object str,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string methodName = "")
    {
        var fileName = Path.GetFileName(filePath);
        var message = $"File Name - {fileName} | Method Name - {methodName} | Result - {(str is string ? str : str.ToJson())}";
        logger.LogInformation(message);
    }

    #endregion

    #region DbContext Extension

    public static async Task<int> SaveAndDetachAsync(this DbContext db)
    {
        var res = await db.SaveChangesAsync();
        foreach (var entry in db.ChangeTracker.Entries().ToArray())
        {
            entry.State = EntityState.Detached;
        }

        return res;
    }

    #endregion

    #region File Upload

    public static async Task<List<FileUploadData>> UploadFilesAsync(this EnumDirectory directory, IEnumerable<IFormFile> files)
    {
        if (files == null || !files.Any())
        {
            throw new Exception("Error: No files were uploaded.");
        }

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), directory.ToString());
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var uploadedFiles = new List<FileUploadData>();

        foreach (var file in files)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception($"Error: One or more files are empty.");
            }

            if (file.Length > MaxFileSize)
            {
                throw new Exception($"Error: '{file.FileName}' exceeds {MaxFileSize / (1024 * 1024)}MB limit.");
            }

            var fileName = GenerateUlid() + ".jpg";
            var filePath = Path.Combine(uploadPath, fileName);
            var savePath = Path.Combine(directory.ToString(), fileName);
            try
            {
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream);

                uploadedFiles.Add(new FileUploadData
                {
                    FilePath = savePath,
                    FileName = fileName,
                });
            }
            catch (Exception ex)
            {
                foreach (var uploadedFile in uploadedFiles)
                {
                    try
                    {
                        File.Delete(uploadedFile.FilePath);
                    }
                    catch
                    {
                        // throw;
                    }
                }
                throw new Exception($"Error uploading files: {ex.Message}");
            }
        }

        return uploadedFiles;
    }

    #endregion

    #region Base64 Extenstion

    public static async Task<string> GetBase64FromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new Exception($"File not found at path: {filePath}");
        }

        byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
        return Convert.ToBase64String(fileBytes);
    }

    #endregion

    #region Hash Password

    public static string HashPassword(this string password, string username)
    {
        password = password.Trim();
        username = username.Trim();
        string saltedCode = EncodedSalt(username);
        string hashString;
        using (var sha256 = SHA256.Create())
        {
            var hash = sha256.ComputeHash(Encoding.Default.GetBytes(password + saltedCode));
            hashString = ToHex(hash, false);
        }

        return hashString;
    }

    private static string EncodedSalt(string decodeString)
    {
        decodeString = decodeString.ToLower()
            .Replace("a", "@")
            .Replace("i", "!")
            .Replace("l", "1")
            .Replace("e", "3")
            .Replace("o", "0")
            .Replace("s", "$")
            .Replace("n", "&");
        return decodeString;
    }

    public static string ToHex(byte[] bytes, bool upperCase)
    {
        StringBuilder result = new StringBuilder(bytes.Length * 2);
        for (int i = 0; i < bytes.Length; i++)
            result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
        return result.ToString();
    }

    #endregion

    #region String to Hex to String

    public static string StringToHex(this string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        return BitConverter.ToString(bytes).Replace("-", "");
    }

    public static string HexToString(this string hex)
    {
        if (string.IsNullOrEmpty(hex)) return string.Empty;

        var bytes = new byte[hex.Length / 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return Encoding.UTF8.GetString(bytes);
    }

    public static byte[] ConvertFromStringToHex(this string inputHex)
    {
        inputHex = inputHex.Replace(" ", "");

        if (inputHex.Length % 2 != 0)
            throw new ArgumentException("Hex string must have even length");

        var resultantArray = new byte[inputHex.Length / 2];
        for (var i = 0; i < resultantArray.Length; i++)
        {
            resultantArray[i] = Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
        }
        return resultantArray;
    }

    public static string ConvertByteArrayToHexString(this byte[] byteArray)
    {
        if (byteArray == null || byteArray.Length == 0)
            return string.Empty;
        return BitConverter.ToString(byteArray).Replace("-", "");
    }

    #endregion

    #region Encryption and Decryption

    public static string? ToEncrypt(this string? str)
    {
        if (str is null) return null;
        var encrypted = Strings.Encrypt(str, key, iv);
        return encrypted;
    }

    public static string ToDecrypt(this string str)
    {
        string decrypted = "";
        try
        {
            if (!str.IsNullOrEmpty())
            {
                decrypted = Strings.Decrypt(str, key, iv);
            }
        }
        catch
        {
            decrypted = str;
        }

        return decrypted;
    }

    #endregion

    #region Get Request Url

    public static string GetRequestUrl(HttpContext context)
    {
        if (context == null || context.Request == null)
        {
            return string.Empty;
        }

        var request = context.Request;
        var url = $"{request.Scheme}://{request.Host}";
        return url;
    }

    #endregion
}