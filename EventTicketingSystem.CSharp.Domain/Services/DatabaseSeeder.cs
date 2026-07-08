namespace EventTicketingSystem.CSharp.Domain.Services;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.EnsureCreatedAsync();

        if (await db.TblAdmins.AnyAsync())
        {
            return;
        }

        var now = DateTime.Now;
        var eventCode = "EV0000001";
        var venueCode = "VE0000001";
        var venueTypeCode = "VT0000001";
        var ownerCode = "BO0000001";
        var eventCategoryCode = "EC0000001";
        var uniqueName = "SUM";

        await db.TblSequences.AddRangeAsync(CreateTableSequences(now));
        await db.TblSequences.AddRangeAsync(
            new TblSequence
            {
                Uniquename = uniqueName,
                Sequenceno = "0000000",
                Sequencedate = now,
                Sequencetype = EnumSequenceType.Event.ToString(),
                Eventcode = eventCode,
                Deleteflag = false
            },
            new TblSequence
            {
                Uniquename = uniqueName,
                Sequenceno = "0000000",
                Sequencedate = now,
                Sequencetype = EnumSequenceType.Transaction.ToString(),
                Eventcode = eventCode,
                Deleteflag = false
            });

        await db.TblAdmins.AddAsync(new TblAdmin
        {
            Adminid = GenerateUlid(),
            Admincode = "AD0000001",
            Fullname = "System Admin",
            Username = "admin",
            Email = "admin@example.com",
            Phone = "09123456789",
            Password = "Admin@123".HashPassword("admin"),
            Isfirsttime = false,
            Createdby = "SYSTEM",
            Createdat = now,
            Deleteflag = false
        });

        await db.TblVenuetypes.AddAsync(new TblVenuetype
        {
            Venuetypeid = GenerateUlid(),
            Venuetypecode = venueTypeCode,
            Venuetypename = "Hall",
            Createdby = "SYSTEM",
            Createdat = now,
            Deleteflag = false
        });

        await db.TblVenues.AddAsync(new TblVenue
        {
            Venueid = GenerateUlid(),
            Venuecode = venueCode,
            Venuetypecode = venueTypeCode,
            Venuename = "Main Convention Hall",
            Description = "Seed venue for first run.",
            Address = "Yangon",
            Capacity = 500,
            Facilities = "Stage,Sound,Lighting",
            Addons = "Projector,Security",
            Venueimage = string.Empty,
            Createdby = "SYSTEM",
            Createdat = now,
            Deleteflag = false
        });

        await db.TblBusinessowners.AddAsync(new TblBusinessowner
        {
            Businessownerid = GenerateUlid(),
            Businessownercode = ownerCode,
            Fullname = "Default Business Owner",
            Email = "owner@example.com",
            Phone = "09987654321",
            Createdby = "SYSTEM",
            Createdat = now,
            Deleteflag = false
        });

        await db.TblEventcategories.AddAsync(new TblEventcategory
        {
            Eventcategoryid = GenerateUlid(),
            Eventcategorycode = eventCategoryCode,
            Categoryname = "Conference",
            Createdby = "SYSTEM",
            Createdat = now,
            Deleteflag = false
        });

        await db.TblEvents.AddAsync(new TblEvent
        {
            Eventid = GenerateUlid(),
            Eventcode = eventCode,
            Venuecode = venueCode,
            Eventname = "Summer Tech Summit",
            Eventcategorycode = eventCategoryCode,
            Description = "Seed event for first run.",
            Address = "Yangon",
            Startdate = now.AddDays(7),
            Enddate = now.AddDays(7).AddHours(4),
            Isactive = true,
            Eventstatus = EnumEventStatus.Upcoming.ToString(),
            Businessownercode = ownerCode,
            Totalticketquantity = 20,
            Soldoutcount = 0,
            Uniquename = uniqueName,
            Createdby = "SYSTEM",
            Createdat = now,
            Deleteflag = false
        });

        await SeedTicketType(db, "TT0000001", "TP0000001", eventCode, "Standard", 10000, 10, now, 1);
        await SeedTicketType(db, "TT0000002", "TP0000002", eventCode, "VIP", 25000, 10, now, 11);

        await db.SaveChangesAsync();
    }

    private static IEnumerable<TblSequence> CreateTableSequences(DateTime now)
    {
        var lastNumbers = new Dictionary<EnumTableUniqueName, int>
        {
            [EnumTableUniqueName.Tbl_Admin] = 1,
            [EnumTableUniqueName.Tbl_BusinessOwner] = 1,
            [EnumTableUniqueName.Tbl_EventCategory] = 1,
            [EnumTableUniqueName.Tbl_Event] = 1,
            [EnumTableUniqueName.Tbl_Ticket] = 20,
            [EnumTableUniqueName.Tbl_TicketPrice] = 2,
            [EnumTableUniqueName.Tbl_TicketType] = 2,
            [EnumTableUniqueName.Tbl_Venue] = 1,
            [EnumTableUniqueName.Tbl_VenueType] = 1
        };

        return Enum.GetValues<EnumTableUniqueName>().Select(name => new TblSequence
        {
            Uniquename = name.GetEnumDescription(),
            Sequenceno = lastNumbers.GetValueOrDefault(name).ToString("D7"),
            Sequencedate = now,
            Sequencetype = EnumSequenceType.Table.ToString(),
            Deleteflag = false
        });
    }

    private static async Task SeedTicketType(
        AppDbContext db,
        string ticketTypeCode,
        string ticketPriceCode,
        string eventCode,
        string ticketTypeName,
        decimal ticketPrice,
        int ticketQuantity,
        DateTime now,
        int firstTicketNo)
    {
        await db.TblTickettypes.AddAsync(new TblTickettype
        {
            Tickettypeid = GenerateUlid(),
            Tickettypecode = ticketTypeCode,
            Eventcode = eventCode,
            Tickettypename = ticketTypeName,
            Createdby = "SYSTEM",
            Createdat = now,
            Deleteflag = false
        });

        await db.TblTicketprices.AddAsync(new TblTicketprice
        {
            Ticketpriceid = GenerateUlid(),
            Ticketpricecode = ticketPriceCode,
            Tickettypecode = ticketTypeCode,
            Ticketprice = ticketPrice,
            Ticketquantity = ticketQuantity,
            Createdby = "SYSTEM",
            Createdat = now,
            Deleteflag = false
        });

        for (var i = 0; i < ticketQuantity; i++)
        {
            await db.TblTickets.AddAsync(new TblTicket
            {
                Ticketid = GenerateUlid(),
                Ticketcode = $"TI{firstTicketNo + i:D7}",
                Ticketpricecode = ticketPriceCode,
                Isused = false,
                Createdby = "SYSTEM",
                Createdat = now,
                Deleteflag = false
            });
        }
    }
}
