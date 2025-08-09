using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EventTicketingSystem.CSharp.Database.AppDbContext;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAdmin> TblAdmins { get; set; }

    public virtual DbSet<TblBusinessemail> TblBusinessemails { get; set; }

    public virtual DbSet<TblBusinessowner> TblBusinessowners { get; set; }

    public virtual DbSet<TblEvent> TblEvents { get; set; }

    public virtual DbSet<TblEventcategory> TblEventcategories { get; set; }

    public virtual DbSet<TblLogin> TblLogins { get; set; }

    public virtual DbSet<TblSequence> TblSequences { get; set; }

    public virtual DbSet<TblTicket> TblTickets { get; set; }

    public virtual DbSet<TblTicketprice> TblTicketprices { get; set; }

    public virtual DbSet<TblTickettype> TblTickettypes { get; set; }

    public virtual DbSet<TblTransaction> TblTransactions { get; set; }

    public virtual DbSet<TblTransactionticket> TblTransactiontickets { get; set; }

    public virtual DbSet<TblVenue> TblVenues { get; set; }

    public virtual DbSet<TblVenuetype> TblVenuetypes { get; set; }

    public virtual DbSet<TblVerification> TblVerifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAdmin>(entity =>
        {
            entity.HasKey(e => e.Adminid).HasName("tbl_admin_pk");

            entity.ToTable("tbl_admin");

            entity.Property(e => e.Adminid)
                .HasColumnType("character varying")
                .HasColumnName("adminid");
            entity.Property(e => e.Admincode)
                .HasColumnType("character varying")
                .HasColumnName("admincode");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasColumnType("character varying")
                .HasColumnName("fullname");
            entity.Property(e => e.Isfirsttime).HasColumnName("isfirsttime");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.Profileimage)
                .HasColumnType("character varying")
                .HasColumnName("profileimage");
            entity.Property(e => e.Username)
                .HasColumnType("character varying")
                .HasColumnName("username");
        });

        modelBuilder.Entity<TblBusinessemail>(entity =>
        {
            entity.HasKey(e => e.Businessemailid).HasName("tbl_businessemail_pk");

            entity.ToTable("tbl_businessemail");

            entity.Property(e => e.Businessemailid)
                .HasColumnType("character varying")
                .HasColumnName("businessemailid");
            entity.Property(e => e.Businessemailcode)
                .HasColumnType("character varying")
                .HasColumnName("businessemailcode");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasColumnType("character varying")
                .HasColumnName("fullname");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
        });

        modelBuilder.Entity<TblBusinessowner>(entity =>
        {
            entity.HasKey(e => e.Businessownerid).HasName("tbl_businessowner_pk");

            entity.ToTable("tbl_businessowner");

            entity.Property(e => e.Businessownerid)
                .HasColumnType("character varying")
                .HasColumnName("businessownerid");
            entity.Property(e => e.Businessownercode)
                .HasColumnType("character varying")
                .HasColumnName("businessownercode");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasColumnType("character varying")
                .HasColumnName("fullname");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
        });

        modelBuilder.Entity<TblEvent>(entity =>
        {
            entity.HasKey(e => e.Eventid).HasName("tbl_event_pk");

            entity.ToTable("tbl_event");

            entity.Property(e => e.Eventid)
                .HasColumnType("character varying")
                .HasColumnName("eventid");
            entity.Property(e => e.Businessownercode)
                .HasColumnType("character varying")
                .HasColumnName("businessownercode");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Enddate).HasColumnName("enddate");
            entity.Property(e => e.Eventcategorycode)
                .HasColumnType("character varying")
                .HasColumnName("eventcategorycode");
            entity.Property(e => e.Eventcode)
                .HasColumnType("character varying")
                .HasColumnName("eventcode");
            entity.Property(e => e.Eventname)
                .HasColumnType("character varying")
                .HasColumnName("eventname");
            entity.Property(e => e.Eventstatus)
                .HasColumnType("character varying")
                .HasColumnName("eventstatus");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Soldoutcount).HasColumnName("soldoutcount");
            entity.Property(e => e.Startdate).HasColumnName("startdate");
            entity.Property(e => e.Totalticketquantity).HasColumnName("totalticketquantity");
            entity.Property(e => e.Uniquename)
                .HasColumnType("character varying")
                .HasColumnName("uniquename");
            entity.Property(e => e.Venuecode)
                .HasColumnType("character varying")
                .HasColumnName("venuecode");
        });

        modelBuilder.Entity<TblEventcategory>(entity =>
        {
            entity.HasKey(e => e.Eventcategoryid).HasName("tbl_eventcategory_pk");

            entity.ToTable("tbl_eventcategory");

            entity.Property(e => e.Eventcategoryid)
                .HasColumnType("character varying")
                .HasColumnName("eventcategoryid");
            entity.Property(e => e.Categoryname)
                .HasColumnType("character varying")
                .HasColumnName("categoryname");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Eventcategorycode)
                .HasColumnType("character varying")
                .HasColumnName("eventcategorycode");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
        });

        modelBuilder.Entity<TblLogin>(entity =>
        {
            entity.HasKey(e => e.Loginid).HasName("tbl_login_pk");

            entity.ToTable("tbl_login");

            entity.Property(e => e.Loginid)
                .HasColumnType("character varying")
                .HasColumnName("loginid");
            entity.Property(e => e.Admincode)
                .HasColumnType("character varying")
                .HasColumnName("admincode");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Loginstatus)
                .HasColumnType("character varying")
                .HasColumnName("loginstatus");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Refreshtoken)
                .HasColumnType("character varying")
                .HasColumnName("refreshtoken");
            entity.Property(e => e.Refreshtokenexpiresat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("refreshtokenexpiresat");
            entity.Property(e => e.Sessionid)
                .HasColumnType("character varying")
                .HasColumnName("sessionid");
            entity.Property(e => e.Username)
                .HasColumnType("character varying")
                .HasColumnName("username");
        });

        modelBuilder.Entity<TblSequence>(entity =>
        {
            entity.HasKey(e => e.Sequenceid).HasName("tbl_sequence_pk");

            entity.ToTable("tbl_sequence");

            entity.Property(e => e.Sequenceid).HasColumnName("sequenceid");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Eventcode)
                .HasColumnType("character varying")
                .HasColumnName("eventcode");
            entity.Property(e => e.Sequencedate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sequencedate");
            entity.Property(e => e.Sequenceno)
                .HasColumnType("character varying")
                .HasColumnName("sequenceno");
            entity.Property(e => e.Sequencetype)
                .HasColumnType("character varying")
                .HasColumnName("sequencetype");
            entity.Property(e => e.Uniquename)
                .HasColumnType("character varying")
                .HasColumnName("uniquename");
        });

        modelBuilder.Entity<TblTicket>(entity =>
        {
            entity.HasKey(e => e.Ticketid).HasName("tbl_ticket_pk");

            entity.ToTable("tbl_ticket");

            entity.Property(e => e.Ticketid)
                .HasColumnType("character varying")
                .HasColumnName("ticketid");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Isused).HasColumnName("isused");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Soldout).HasColumnName("soldout");
            entity.Property(e => e.Ticketcode)
                .HasColumnType("character varying")
                .HasColumnName("ticketcode");
            entity.Property(e => e.Ticketpricecode)
                .HasColumnType("character varying")
                .HasColumnName("ticketpricecode");
        });

        modelBuilder.Entity<TblTicketprice>(entity =>
        {
            entity.HasKey(e => e.Ticketpriceid).HasName("tbl_ticketprice_pk");

            entity.ToTable("tbl_ticketprice");

            entity.Property(e => e.Ticketpriceid)
                .HasColumnType("character varying")
                .HasColumnName("ticketpriceid");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Ticketprice)
                .HasPrecision(20, 2)
                .HasColumnName("ticketprice");
            entity.Property(e => e.Ticketpricecode)
                .HasColumnType("character varying")
                .HasColumnName("ticketpricecode");
            entity.Property(e => e.Ticketquantity).HasColumnName("ticketquantity");
            entity.Property(e => e.Tickettypecode)
                .HasColumnType("character varying")
                .HasColumnName("tickettypecode");
        });

        modelBuilder.Entity<TblTickettype>(entity =>
        {
            entity.HasKey(e => e.Tickettypeid).HasName("tbl_tickettype_pk");

            entity.ToTable("tbl_tickettype");

            entity.Property(e => e.Tickettypeid)
                .HasColumnType("character varying")
                .HasColumnName("tickettypeid");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Eventcode)
                .HasColumnType("character varying")
                .HasColumnName("eventcode");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Tickettypecode)
                .HasColumnType("character varying")
                .HasColumnName("tickettypecode");
            entity.Property(e => e.Tickettypename)
                .HasColumnType("character varying")
                .HasColumnName("tickettypename");
        });

        modelBuilder.Entity<TblTransaction>(entity =>
        {
            entity.HasKey(e => e.Transactionid).HasName("tbl_transaction_pk");

            entity.ToTable("tbl_transaction");

            entity.Property(e => e.Transactionid)
                .HasColumnType("character varying")
                .HasColumnName("transactionid");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Eventcode)
                .HasColumnType("character varying")
                .HasColumnName("eventcode");
            entity.Property(e => e.Fullname)
                .HasColumnType("character varying")
                .HasColumnName("fullname");
            entity.Property(e => e.Gender)
                .HasColumnType("character varying")
                .HasColumnName("gender");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Paymenttype)
                .HasColumnType("character varying")
                .HasColumnName("paymenttype");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
            entity.Property(e => e.Totalamount)
                .HasPrecision(20, 2)
                .HasColumnName("totalamount");
            entity.Property(e => e.Transactioncode)
                .HasColumnType("character varying")
                .HasColumnName("transactioncode");
            entity.Property(e => e.Transactiondate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("transactiondate");
        });

        modelBuilder.Entity<TblTransactionticket>(entity =>
        {
            entity.HasKey(e => e.Transactionticketid).HasName("tbl_transactionticket_pk");

            entity.ToTable("tbl_transactionticket");

            entity.Property(e => e.Transactionticketid)
                .HasColumnType("character varying")
                .HasColumnName("transactionticketid");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Price)
                .HasPrecision(20, 2)
                .HasColumnName("price");
            entity.Property(e => e.Qrimage)
                .HasColumnType("character varying")
                .HasColumnName("qrimage");
            entity.Property(e => e.Ticketcode)
                .HasColumnType("character varying")
                .HasColumnName("ticketcode");
            entity.Property(e => e.Transactioncode)
                .HasColumnType("character varying")
                .HasColumnName("transactioncode");
            entity.Property(e => e.Transactionticketcode)
                .HasColumnType("character varying")
                .HasColumnName("transactionticketcode");
        });

        modelBuilder.Entity<TblVenue>(entity =>
        {
            entity.HasKey(e => e.Venueid).HasName("tbl_venue_pk");

            entity.ToTable("tbl_venue");

            entity.Property(e => e.Venueid)
                .HasColumnType("character varying")
                .HasColumnName("venueid");
            entity.Property(e => e.Addons).HasColumnName("addons");
            entity.Property(e => e.Address)
                .HasColumnType("character varying")
                .HasColumnName("address");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Facilities).HasColumnName("facilities");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Venuecode)
                .HasColumnType("character varying")
                .HasColumnName("venuecode");
            entity.Property(e => e.Venueimage)
                .HasColumnType("character varying")
                .HasColumnName("venueimage");
            entity.Property(e => e.Venuename)
                .HasColumnType("character varying")
                .HasColumnName("venuename");
            entity.Property(e => e.Venuetypecode)
                .HasColumnType("character varying")
                .HasColumnName("venuetypecode");
        });

        modelBuilder.Entity<TblVenuetype>(entity =>
        {
            entity.HasKey(e => e.Venuetypeid).HasName("tbl_venuetype_pk");

            entity.ToTable("tbl_venuetype");

            entity.Property(e => e.Venuetypeid)
                .HasColumnType("character varying")
                .HasColumnName("venuetypeid");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Venuetypecode)
                .HasColumnType("character varying")
                .HasColumnName("venuetypecode");
            entity.Property(e => e.Venuetypename)
                .HasColumnType("character varying")
                .HasColumnName("venuetypename");
        });

        modelBuilder.Entity<TblVerification>(entity =>
        {
            entity.HasKey(e => e.Verificationid).HasName("tbl_verification_pk");

            entity.ToTable("tbl_verification");

            entity.Property(e => e.Verificationid)
                .HasColumnType("character varying")
                .HasColumnName("verificationid");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Createdby)
                .HasColumnType("character varying")
                .HasColumnName("createdby");
            entity.Property(e => e.Deleteflag).HasColumnName("deleteflag");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Expiredtime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expiredtime");
            entity.Property(e => e.Isused).HasColumnName("isused");
            entity.Property(e => e.Modifiedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Modifiedby)
                .HasColumnType("character varying")
                .HasColumnName("modifiedby");
            entity.Property(e => e.Verificationcode)
                .HasColumnType("character varying")
                .HasColumnName("verificationcode");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
