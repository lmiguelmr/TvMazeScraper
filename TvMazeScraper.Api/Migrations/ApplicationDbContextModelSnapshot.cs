﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TvMazeScraper.Infrastructure.Persistence;

#nullable disable

namespace TvMazeScraper.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TvMazeScraper.Domain.CastMembers.CastMember", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("CastMembers", (string)null);
                });

            modelBuilder.Entity("TvMazeScraper.Domain.JointTables.TvShowCastMember", b =>
                {
                    b.Property<int>("TvShowId")
                        .HasColumnType("int");

                    b.Property<int>("CastMemberId")
                        .HasColumnType("int");

                    b.HasKey("TvShowId", "CastMemberId");

                    b.ToTable("TvShowCastMembers", (string)null);
                });

            modelBuilder.Entity("TvMazeScraper.Domain.TvShows.TvShow", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("TvShows", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
