namespace JWTAuth.WebApi.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; } = default!;
    public virtual DbSet<UserInfo> UserInfos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable(nameof(UserInfo));

            entity.Property(e => e.UserId).HasColumnName(nameof(UserInfo.UserId));

            entity.Property(e => e.DisplayName)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.CreatedDate)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable(nameof(Employee));

            entity.Property(e => e.EmployeeId).HasColumnName(nameof(Employee.EmployeeId));

            entity.Property(e => e.NationalIdNumber)
                  .HasMaxLength(15)
                  .IsUnicode(false);

            entity.Property(e => e.EmployeeName)
                  .HasMaxLength(100)
                  .IsUnicode(false);

            entity.Property(e => e.LoginId)
                  .HasMaxLength(256)
                  .IsUnicode(false);

            entity.Property(e => e.JobTitle)
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.BirthDate)
                  .IsUnicode(false);

            entity.Property(e => e.MaritalStatus)
                  .HasMaxLength(1)
                  .IsUnicode(false);

            entity.Property(e => e.Gender)
                  .HasMaxLength(1)
                  .IsUnicode(false);

            entity.Property(e => e.HireDate)
                  .IsUnicode(false);

            entity.Property(e => e.VacationHours)
                  .IsUnicode(false);

            entity.Property(e => e.SickLeaveHours)
                  .IsUnicode(false);

            entity.Property(e => e.RowGuid)
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.ModifiedDate)
                  .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
