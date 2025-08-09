namespace EventTicketingSystem.CSharp.Domain.Services;

public class CommonService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapper;
    private readonly ILogger<CommonService> _logger;

    public CommonService(AppDbContext db, DapperService dapper, ILogger<CommonService> logger)
    {
        _db = db;
        _dapper = dapper;
        _logger = logger;
    }

    #region Create Sequence for Event

    public async Task CreateSequence(string uniqueName, string eventCode)
    {
        var eventSequence = new TblSequence
        {
            Uniquename = uniqueName,
            Sequenceno = "0000000",
            Sequencedate = DateTime.Now,
            Sequencetype = EnumSequenceType.EventTicket.ToString(),
            Eventcode = eventCode,
            Deleteflag = false
        };

        var transactionSequence = new TblSequence
        {
            Uniquename = uniqueName,
            Sequenceno = "0000000",
            Sequencedate = DateTime.Now,
            Sequencetype = EnumSequenceType.Transaction.ToString(),
            Eventcode = eventCode,
            Deleteflag = false
        };

        await _db.TblSequences.AddRangeAsync(eventSequence, transactionSequence);
        await _db.SaveAndDetachAsync();
    }

    #endregion

    #region Generate Table Sequence Code

    public async Task<string> GenerateSequenceCode(EnumTableUniqueName uniqueName)
    {
        var sequence = await _db.TblSequences
                        .AsNoTracking()
                        .Where(
                            x => x.Uniquename == uniqueName.GetEnumDescription() &&
                            x.Sequencetype == EnumSequenceType.Table.ToString() &&
                            x.Deleteflag == false)
                        .FirstOrDefaultAsync();

        if (sequence is null)
        {
            throw new Exception("Sequence not found for the given event code.");
        }

        var sequnceCode = $"{sequence.Uniquename}";

        var param = new { id = sequence.Sequenceid };

        string? seqNo = await _dapper.QueryStoredProcedureFirstOrDefault<string>(Queries.sp_sequencecode, param);

        string sequenceNo = seqNo ?? throw new Exception("Sequence not found.");

        sequnceCode += sequenceNo;

        return sequnceCode;
    }

    public async Task<string> GenerateTransactionSequenceCode(EnumTableUniqueName uniqueName)
    {
        var sequence = await _db.TblSequences
                        .AsNoTracking()
                        .Where(
                            x => x.Uniquename == uniqueName.GetEnumDescription() &&
                            x.Sequencetype == EnumSequenceType.Table.ToString() &&
                            x.Deleteflag == false)
                        .FirstOrDefaultAsync();

        if (sequence is null)
        {
            throw new Exception("Sequence not found for the given event code.");
        }

        var date = $"{sequence.Sequencedate:yyyyMMdd}";
        var sequenceCodePrefix = $"{sequence.Uniquename}{date}";

        var param = new { id = sequence.Sequenceid };

        string? seqNo = await _dapper.QueryStoredProcedureFirstOrDefault<string>(Queries.sp_sequencecode, param);

        string sequenceNo = seqNo ?? throw new Exception("Sequence not found.");

        sequenceCodePrefix += sequenceNo;

        return sequenceCodePrefix;
    }

    #endregion

    #region Generate Ticket Sequence Code

    public async Task<List<string>> GenerateTicketSequenceCode(string uniqueName, string eventCode, int quantity)
    {
        var sequence = await _db.TblSequences
                        .AsNoTracking()
                        .Where(     
                            x => x.Uniquename == uniqueName &&
                            x.Sequencetype == EnumSequenceType.EventTicket.ToString() &&
                            x.Eventcode == eventCode &&
                            x.Deleteflag == false)
                        .FirstOrDefaultAsync();

        if (sequence is null)
        {
            throw new Exception("Sequence not found for the given event code.");
        }

        var param = new { id = sequence.Sequenceid, qty = quantity };

        var seqNos = await _dapper.QueryStoredProcedureAsync<string>(Queries.sp_sequencecode_bulk, param);

        if (seqNos is null || !seqNos.Any())
        {
            throw new InvalidOperationException("No sequence numbers returned.");
        }

        return seqNos.Select(sn => $"{sequence.Uniquename}{sn}").ToList();
    }

    #endregion

    #region Generate Transaction Sequence Code

    public async Task<List<string>> GenerateTransactionSequenceCode(string uniqueName, string eventCode, int quantity)
    {
        var sequence = await _db.TblSequences
                        .AsNoTracking()
                        .Where(
                            x => x.Uniquename == uniqueName &&
                            x.Sequencetype == EnumSequenceType.Transaction.ToString() &&
                            x.Eventcode == eventCode &&
                            x.Deleteflag == false)
                        .FirstOrDefaultAsync();

        if (sequence is null)
        {
            throw new Exception("Sequence not found for the given event code.");
        }

        var date = $"{sequence.Sequencedate:yyyyMMdd}";
        var sequenceCodePrefix = $"{sequence.Uniquename}{date}";

        var param = new { id = sequence.Sequenceid, qty = quantity };

        var seqNos = await _dapper.QueryStoredProcedureAsync<string>(Queries.sp_sequencecode_bulk, param);
        if (seqNos is null || !seqNos.Any())
        {
            throw new Exception("Sequence not found.");
        }

        return seqNos.Select(sn => $"{sequenceCodePrefix}{sn}").ToList();
    }

    #endregion

    #region Delete Expired Ticket QR Images

    public async Task<bool> DeleteExpiredQrImages(List<IFormFile> files)
    {
        var deletedFiles = new List<string>();

        var qrImagePath = Path.Combine(Directory.GetCurrentDirectory(), EnumDirectory.QrImage.ToString());

        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file.FileName);

            var transactionTicket = await _db.TblTransactiontickets
                .FirstOrDefaultAsync(x => x.Qrimage == fileName);

            if (transactionTicket is null)
            {
                return true;
            }

            var ticket = await _db.TblTickets
                .FirstOrDefaultAsync(x => x.Ticketcode == transactionTicket.Ticketcode);

            if (ticket != null && !ticket.Isused)
            {
                var fullPath = Path.Combine(qrImagePath, fileName);

                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                        deletedFiles.Add(fileName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogExceptionError(ex);
                        return false;
                    }
                }
            }
        }

        return false;
    }

    #endregion

    #region Delete Images Not Exist

    public async Task<bool> DeleteExtraFiles()
    {
        try
        {
            var venueImagesInDb = await _db.TblVenues
                .Select(x => x.Venueimage)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToListAsync();

            DeleteExtraFilesForDirectory(EnumDirectory.VenueImage, venueImagesInDb);

            var qrImagesInDb = await _db.TblTransactiontickets
                .Select(x => x.Qrimage)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToListAsync();

            DeleteExtraFilesForDirectory(EnumDirectory.QrImage, qrImagesInDb);

            var adminProfilesInDb = await _db.TblAdmins
                .Select(x => x.Profileimage)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToListAsync();

            DeleteExtraFilesForDirectory(EnumDirectory.ProfileImage, adminProfilesInDb!);

            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting image ==> {ex.ToString()}.");
        }
    }

    private List<string> DeleteExtraFilesForDirectory(EnumDirectory directory, List<string> validFileNames)
    {
        var deletedFiles = new List<string>();
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), directory.ToString());

        var allFiles = Directory.GetFiles(folderPath);

        foreach (var filePath in allFiles)
        {
            var fileName = Path.GetFileName(filePath);

            if (!validFileNames.Contains(fileName, StringComparer.OrdinalIgnoreCase))
            {
                try
                {
                    File.Delete(filePath);
                    deletedFiles.Add(fileName);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting image ==> {ex.ToString()}.");
                }
            }
        }

        return deletedFiles;
    }

    #endregion
}