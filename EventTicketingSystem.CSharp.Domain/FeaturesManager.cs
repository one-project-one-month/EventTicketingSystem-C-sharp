namespace EventTicketingSystem.CSharp.Domain;

public static class FeaturesManager
{
    public static IServiceCollection AddModularServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddBuilderServies(builder)
            .AddServices()
            .AddBusinessLogic()
            .AddDataAccessLogic();

        return services;
    }

    public static void UseLogicalFileService(this IApplicationBuilder app, string physicalPath, string folderName)
    {
        Directory.CreateDirectory(Path.Combine(physicalPath, folderName));

        app.UseFileServer(new FileServerOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(physicalPath, folderName)),
            RequestPath = new PathString("/" + folderName),
            EnableDirectoryBrowsing = false
        });
    }

    public static IServiceCollection AddBuilderServies(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
                ServiceLifetime.Transient,
                ServiceLifetime.Transient
        );

        builder.Services
            .AddFluentEmail("eventticketingsystem.opom@gmail.com")
            .AddSmtpSender(new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                        "eventticketingsystem.opom@gmail.com",
                        "qpqo kczf gffk bycz"),
                EnableSsl = true,
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 10000
            });

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<DapperService>();
        services.AddScoped<CommonService>();
        services.AddScoped<EmailService>();
        services.AddScoped<ExportService>();
        services.AddScoped<JwtService>();
                
        return services;
    }

    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddScoped<BL_Admin>();
        services.AddScoped<BL_Auth>();
        services.AddScoped<BL_BusinessEmail>();
        services.AddScoped<BL_BusinessOwner>();
        services.AddScoped<BL_Dashboard>();
        services.AddScoped<BL_Event>();
        services.AddScoped<BL_EventCategory>();
        services.AddScoped<BL_QrCode>();
        services.AddScoped<BL_SearchEventsAndVenues>();
        services.AddScoped<BL_Ticket>();
        services.AddScoped<BL_TicketType>();
        services.AddScoped<BL_Venue>();
        services.AddScoped<BL_VenueType>();
        services.AddScoped<BL_VerificationCode>();

        return services;
    }

    public static IServiceCollection AddDataAccessLogic(this IServiceCollection services)
    {
        services.AddScoped<DA_Admin>();
        services.AddScoped<DA_Auth>();
        services.AddScoped<DA_BusinessEmail>();
        services.AddScoped<DA_BusinessOwner>();
        services.AddScoped<DA_Dashboard>();
        services.AddScoped<DA_Event>();
        services.AddScoped<DA_EventCategory>();
        services.AddScoped<DA_QrCode>();
        services.AddScoped<DA_SearchEventsAndVenues>();
        services.AddScoped<DA_Ticket>();
        services.AddScoped<DA_TicketType>();
        services.AddScoped<DA_Venue>();
        services.AddScoped<DA_VenueType>();
        services.AddScoped<DA_VerificationCode>();

        return services;
    }
}