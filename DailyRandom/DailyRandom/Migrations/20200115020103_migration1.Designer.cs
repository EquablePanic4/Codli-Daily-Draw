﻿// <auto-generated />
using System;
using DailyRandom.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DailyRandom.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20200115020103_migration1")]
    partial class migration1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DailyRandom.Data.Tables.CodliOption", b =>
                {
                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("value")
                        .HasColumnType("text");

                    b.HasKey("name");

                    b.ToTable("CodliOptions");
                });

            modelBuilder.Entity("DailyRandom.Data.Tables.Draw", b =>
                {
                    b.Property<Guid>("drawId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("date")
                        .HasColumnType("integer");

                    b.HasKey("drawId");

                    b.ToTable("Draws");
                });

            modelBuilder.Entity("DailyRandom.Data.Tables.User", b =>
                {
                    b.Property<Guid>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("drawId")
                        .HasColumnType("uuid");

                    b.Property<string>("forname")
                        .HasColumnType("text");

                    b.Property<string>("surname")
                        .HasColumnType("text");

                    b.HasKey("userId");

                    b.HasIndex("drawId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DailyRandom.Data.Tables.User", b =>
                {
                    b.HasOne("DailyRandom.Data.Tables.Draw", null)
                        .WithMany("usersOrder")
                        .HasForeignKey("drawId");
                });
#pragma warning restore 612, 618
        }
    }
}
