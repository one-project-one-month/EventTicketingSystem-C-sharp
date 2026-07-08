namespace EventTicketingSystem.CSharp.Domain.Features.Auth;

public class DA_Auth
{
    private readonly ILogger<DA_Auth>  _logger;
    private readonly AppDbContext _db;

    public DA_Auth(ILogger<DA_Auth> logger, AppDbContext db, UserContextService userContextService)
    {
        _logger = logger;
        _db = db;
    }
    public async Task<TblAdmin?> GetUserByUsername(string username)
    {
        return await _db.TblAdmins.FirstOrDefaultAsync(x => x.Username == username);
    }
    public async Task CreateLogin(TblLogin login)
    {
        _db.TblLogins.Add(login);
        await _db.SaveChangesAsync();
    }
    
    public async Task UpdateLogin(TblLogin login)
    {
        _db.TblLogins.Update(login);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateUser(TblAdmin admin)
    {
        _db.TblAdmins.Update(admin);
        await _db.SaveChangesAsync();
    }

    public async Task<TblLogin?> GetUserByRefreshToken(string refreshToken)
    {
        return await _db.TblLogins.FirstOrDefaultAsync(x => x.Refreshtoken == refreshToken);
    }
}
