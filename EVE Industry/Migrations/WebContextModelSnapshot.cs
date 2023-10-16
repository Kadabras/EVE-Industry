﻿// <auto-generated />
using System;
using EVE_Industry.EfStuff;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EVE_Industry.Migrations
{
    [DbContext(typeof(WebContext))]
    partial class WebContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EVE_Industry.EfStuff.DbModel.DumpCell", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParsedId")
                        .HasColumnType("int");

                    b.Property<int>("Profit")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DumpCells");
                });

            modelBuilder.Entity("EVE_Industry.EfStuff.DbModel.MainIndustryCell", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<TimeSpan>("ManufacturingTime")
                        .HasColumnType("time");

                    b.Property<int>("MaterialEfficiency")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParsedId")
                        .HasColumnType("int");

                    b.Property<long>("Profit")
                        .HasColumnType("bigint");

                    b.Property<long>("ProfitPerHour")
                        .HasColumnType("bigint");

                    b.Property<int>("TimeEfficiency")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<int>("TypeItem")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MainIndustryCells");
                });
#pragma warning restore 612, 618
        }
    }
}
