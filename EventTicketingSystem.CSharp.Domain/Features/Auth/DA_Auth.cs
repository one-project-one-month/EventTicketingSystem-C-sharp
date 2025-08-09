namespace EventTicketingSystem.CSharp.Domain.Features.Auth;

public class DA_Auth : AuthorizationService
{
    private readonly ILogger<DA_Auth>  _logger;
    private readonly AppDbContext _db;

    public DA_Auth(IHttpContextAccessor httpContextAccessor, 
                   ILogger<DA_Auth> logger, 
                   AppDbContext db, 
                   UserContextService userContextService) : base(httpContextAccessor)
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

    public async Task<TblLogin?> GetUserByRefreshToken(string refreshToken)
    {
        return await _db.TblLogins.FirstOrDefaultAsync(x => x.Refreshtoken == refreshToken);
    }
    
    public async Task SetIsFirstTimeToFalse(string username)
    {
        var user = await _db.TblAdmins.FirstOrDefaultAsync(x => x.Username == username);
        if (user != null && user.Isfirsttime)
        {
            user.Isfirsttime = false;
            
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();
        }
    }
    
    public async Task UpdatePassword(string username, string hashedPassword)
    {
        var admin = await _db.TblAdmins.FirstOrDefaultAsync(x => x.Username == username);
        if (admin != null)
        {
            admin.Password = hashedPassword;
            admin.Modifiedat = DateTime.Now;
            admin.Modifiedby = CurrentUserId; 
            
            _db.Entry(admin).State = EntityState.Modified;
            await _db.SaveAndDetachAsync();
        }
    }


}