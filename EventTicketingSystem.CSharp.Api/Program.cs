string logFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
string logFileName = Path.Combine(logFolderPath, "logs_");

if (!Directory.Exists(logFolderPath))
{
    Directory.CreateDirectory(logFolderPath);
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        path: Path.Combine(logFileName),
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Hour)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddModularServices(builder);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Ticketing System", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Event Ticketing System",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection("JwtSettings"))
    .ValidateDataAnnotations()
    .Validate(settings => !string.IsNullOrWhiteSpace(settings.SecretKey), "SecretKey must be provided.")
    .ValidateOnStart();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
                     ?? throw new InvalidOperationException("JwtSettings configuration is missing or invalid.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Ticketing System v1");
});

app.UseHttpsRedirection();

try
{
    string rootFolder = builder.Configuration.GetSection(Directory.GetCurrentDirectory()).Value!;
    string qr = builder.Configuration.GetSection("Qr").Value!;
    string profile = builder.Configuration.GetSection("Profile").Value!;
    string venue = builder.Configuration.GetSection("Venue").Value!;

    app.UseLogicalFileService(rootFolder, qr);
    app.UseLogicalFileService(rootFolder, profile);
    app.UseLogicalFileService(rootFolder, venue);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

app.UseCors(builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();