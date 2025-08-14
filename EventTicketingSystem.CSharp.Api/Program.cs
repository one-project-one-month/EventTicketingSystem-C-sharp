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

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    //options.AddPolicy("AllowedOrigins", policy =>
    //{
    //    policy.WithOrigins(allowedOrigins!)
    //          .WithMethods("GET", "POST")
    //          .WithHeaders("Content-Type", "Authorization");
    //});

    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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

app.UseStaticFiles();

try
{
    string wwwroot = builder.Configuration.GetValue("RootFolder", "wwwroot")!;
    string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), wwwroot);

    Directory.CreateDirectory(rootFolder);

    var mediaFolders = new Dictionary<string, string>
    {
        ["Qr"] = builder.Configuration.GetValue("Qr", "Qrimage")!,
        ["Profile"] = builder.Configuration.GetValue("Profile", "ProfileImage")!,
        ["Venue"] = builder.Configuration.GetValue("Venue", "VenueImage")!
    };

    foreach (var folder in mediaFolders)
    {
        string fullPath = Path.Combine(rootFolder, folder.Value);
        Directory.CreateDirectory(fullPath);

        app.UseLogicalFileService(rootFolder, folder.Value);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

app.UseCors("AllowedOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();