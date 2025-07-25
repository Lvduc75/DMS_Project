using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DMS.Models.Entities;

public partial class DormManagementContext : DbContext
{
    public DormManagementContext()
    {
    }

    public DormManagementContext(DbContextOptions<DormManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ConfigPrice> ConfigPrices { get; set; }

    public virtual DbSet<DormFacility> DormFacilities { get; set; }

    public virtual DbSet<Dormitory> Dormitories { get; set; }

    public virtual DbSet<Facility> Facilities { get; set; }

    public virtual DbSet<Fee> Fees { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomFacility> RoomFacilities { get; set; }

    public virtual DbSet<StudentRoom> StudentRooms { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UtilityReading> UtilityReadings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=dorm_management; User Id=sa;Password=123;Encrypt=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfigPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ConfigPr__3213E83F67E2F535");

            entity.ToTable("ConfigPrice");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EffectiveFrom)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("effective_from");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("unit_price");
        });

        modelBuilder.Entity<DormFacility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dorm_Fac__3213E83F8E1FBDDC");

            entity.ToTable("Dorm_Facility");

            entity.HasIndex(e => new { e.DormitoryId, e.FacilityId }, "uq_dorm_fac").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DormitoryId).HasColumnName("dormitory_id");
            entity.Property(e => e.FacilityId).HasColumnName("facility_id");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");

            entity.HasOne(d => d.Dormitory).WithMany(p => p.DormFacilities)
                .HasForeignKey(d => d.DormitoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dorm_Faci__dormi__47DBAE45");

            entity.HasOne(d => d.Facility).WithMany(p => p.DormFacilities)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dorm_Faci__facil__48CFD27E");
        });

        modelBuilder.Entity<Dormitory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dormitor__3213E83FE32D84D8");

            entity.ToTable("Dormitory");

            entity.HasIndex(e => e.Name, "UQ__Dormitor__72E12F1BADA67E6E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Facility__3213E83FCEB2BBAE");

            entity.ToTable("Facility");

            entity.HasIndex(e => e.Name, "UQ__Facility__72E12F1B30744E69").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("unit_price");
        });

        modelBuilder.Entity<Fee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Fee__3213E83F8D1FD571");

            entity.ToTable("Fee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("unpaid")
                .HasColumnName("status");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");

            entity.HasOne(d => d.Student).WithMany(p => p.Fees)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Fee__student_id__5DCAEF64");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request__3213E83FC498BC45");

            entity.ToTable("Request");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("new")
                .HasColumnName("status");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");

            entity.HasOne(d => d.Manager).WithMany(p => p.RequestManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Request__manager__5441852A");

            entity.HasOne(d => d.Student).WithMany(p => p.RequestStudents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request__student__534D60F1");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3213E83FF5CDEC8C");

            entity.ToTable("Room");

            entity.HasIndex(e => e.Code, "UQ__Room__357D4CF9FD669FBA").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.DormitoryId).HasColumnName("dormitory_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("available")
                .HasColumnName("status");

            entity.HasOne(d => d.Dormitory).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.DormitoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room__dormitory___403A8C7D");
        });

        modelBuilder.Entity<RoomFacility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room_Fac__3213E83F45D6EAF4");

            entity.ToTable("Room_Facility");

            entity.HasIndex(e => new { e.RoomId, e.FacilityId }, "uq_room_fac").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FacilityId).HasColumnName("facility_id");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");
            entity.Property(e => e.RoomId).HasColumnName("room_id");

            entity.HasOne(d => d.Facility).WithMany(p => p.RoomFacilities)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room_Faci__facil__4E88ABD4");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomFacilities)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room_Faci__room___4D94879B");
        });

        modelBuilder.Entity<StudentRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student___3213E83F014AD0B3");

            entity.ToTable("Student_Room");

            entity.HasIndex(e => new { e.StudentId, e.RoomId, e.StartDate }, "uq_student_room").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Room).WithMany(p => p.StudentRooms)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student_R__room___59063A47");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentRooms)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student_R__stude__5812160E");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3213E83F7452F6D4");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.FeeId).HasColumnName("fee_id");
            entity.Property(e => e.PayerName)
                .HasMaxLength(100)
                .HasColumnName("payer_name");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("payment_date");

            entity.HasOne(d => d.Fee).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.FeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__fee_i__619B8048");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F16C79421");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E61649457BAB8").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("student")
                .HasColumnName("role");
        });

        modelBuilder.Entity<UtilityReading>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UtilityR__3213E83FAAD9E5EE");

            entity.ToTable("UtilityReading");

            entity.HasIndex(e => new { e.RoomId, e.ReadingMonth }, "uq_utility").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Electric).HasColumnName("electric");
            entity.Property(e => e.ImportedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("imported_at");
            entity.Property(e => e.ReadingMonth).HasColumnName("reading_month");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.Water).HasColumnName("water");

            entity.HasOne(d => d.Room).WithMany(p => p.UtilityReadings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UtilityRe__room___66603565");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
