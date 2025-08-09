using System;
using System.Collections.Generic;

namespace EventTicketingSystem.CSharp.Database.AppDbContext;

public partial class TblTicket
{
    public string Ticketid { get; set; } = null!;

    public string Ticketcode { get; set; } = null!;

    public string Ticketpricecode { get; set; } = null!;

    public bool Isused { get; set; }

    public bool Soldout { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifiedat { get; set; }

    public bool Deleteflag { get; set; }
}
