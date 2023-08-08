using System;
using EmpresaAPI.Enums;
using EmpresaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpresaAPI.Persistence
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}

		public DbSet<DepartamentModel> Departaments { get; set; }
		public DbSet<EmployeeModel> Employees { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EmployeeModel>(e =>
            {
                e.ToTable("Employees");

                e.HasKey(cx => cx.Id);

                e.Property(cx => cx.Employee_Name)
                    .HasColumnType("varChar(255)");

                e.Property(cx => cx.Employee_Document)
                    .HasColumnType("varchar(255)");

                e.Property(cx => cx.Status)
                    .HasConversion(
                        cv => cv.ToString(),
                        cv => (StatusEnum)Enum.Parse(typeof(StatusEnum), cv))
                    .IsRequired();



            });


            builder.Entity<DepartamentModel>(e =>
            {
                e.ToTable("Departaments");

                e.HasKey(cx => cx.Id).HasName("Id");

                e.Property(cx => cx.Departament_Name)
                    .HasColumnType("varChar(255)");

                e.Property(cx => cx.Status)
                    .HasConversion(
                        cv => cv.ToString(),
                        cv => (StatusEnum)Enum.Parse(typeof(StatusEnum), cv))
                    .IsRequired();

                e.HasMany(cx => cx.EmployeesOnDepartament)
                    .WithOne()
                    .HasForeignKey(cx => cx.Id_Departament);


            });
        }
    }
}

