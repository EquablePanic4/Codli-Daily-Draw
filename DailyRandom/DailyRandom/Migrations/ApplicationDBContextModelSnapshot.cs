﻿// <auto-generated />
using System;
using System.Collections.Generic;
using DailyRandom.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DailyRandom.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    partial class ApplicationDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DailyRandom.Data.Tables.ApplicationClient", b =>
                {
                    b.Property<Guid>("applicationClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ClientDescription")
                        .HasColumnType("text");

                    b.HasKey("applicationClientId");

                    b.ToTable("ApplicationClients");
                });

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

                    b.Property<List<string>>("userIdsOrder")
                        .HasColumnType("text[]");

                    b.HasKey("drawId");

                    b.ToTable("Draws");
                });

            modelBuilder.Entity("DailyRandom.Data.Tables.User", b =>
                {
                    b.Property<Guid>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("forname")
                        .HasColumnType("text");

                    b.Property<string>("surname")
                        .HasColumnType("text");

                    b.HasKey("userId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
