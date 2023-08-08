﻿// <auto-generated />
using System;
using EmpresaAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmpresaAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230728191249_EmpresaMigration")]
    partial class EmpresaMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EmpresaAPI.Models.DepartamentModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Dapartament_Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Departament_Acronym")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Departaments");
                });

            modelBuilder.Entity("EmpresaAPI.Models.EmployeeModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DepartamentModelId")
                        .HasColumnType("uuid");

                    b.Property<string>("Employee_Document")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Employee_Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("Id_Departament")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DepartamentModelId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EmpresaAPI.Models.EmployeeModel", b =>
                {
                    b.HasOne("EmpresaAPI.Models.DepartamentModel", null)
                        .WithMany("EmployeesOnDepartament")
                        .HasForeignKey("DepartamentModelId");
                });

            modelBuilder.Entity("EmpresaAPI.Models.DepartamentModel", b =>
                {
                    b.Navigation("EmployeesOnDepartament");
                });
#pragma warning restore 612, 618
        }
    }
}
