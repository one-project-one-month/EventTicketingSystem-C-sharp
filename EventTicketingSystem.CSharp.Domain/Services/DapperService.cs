namespace EventTicketingSystem.CSharp.Domain.Services;

public class DapperService
{
    private readonly ILogger<DapperService> _logger;
    private readonly string _connectionString;

    public DapperService(ILogger<DapperService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DbConnection")!;
    }

    public async Task<T?> QueryStoredProcedureFirstOrDefault<T>(string query, object parameters)
    {
        try
        {
            using IDbConnection dbConnection = new NpgsqlConnection(_connectionString);
            dbConnection.Open();

            var lst = await dbConnection.QueryAsync<T>(
                        query,
                        parameters,
                        commandTimeout: 0,
                        commandType: CommandType.Text);

            var result = lst.FirstOrDefault()!;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default;
        }
    }

    public async Task<List<T>> QueryStoredProcedureAsync<T>(string sql, object parameters = null)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var result = await connection.QueryAsync<T>(
            sql,
            parameters
        );

        return result.ToList();
    }
}