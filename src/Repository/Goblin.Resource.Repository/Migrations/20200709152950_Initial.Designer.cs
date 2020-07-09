﻿// <auto-generated />
using System;
using Goblin.Resource.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Goblin.Resource.Repository.Migrations
{
    [DbContext(typeof(GoblinDbContext))]
    [Migration("20200709152950_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Goblin.Resource.Contract.Repository.Models.FileEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ContentLength")
                        .HasColumnType("bigint");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<long?>("DeletedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Folder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hash")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImageDominantHexColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ImageHeightPx")
                        .HasColumnType("int");

                    b.Property<int>("ImageWidthPx")
                        .HasColumnType("int");

                    b.Property<bool>("IsCompressedImage")
                        .HasColumnType("bit");

                    b.Property<bool>("IsImage")
                        .HasColumnType("bit");

                    b.Property<long?>("LastUpdatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MimeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedTime");

                    b.HasIndex("DeletedTime");

                    b.HasIndex("Hash");

                    b.HasIndex("Id");

                    b.HasIndex("LastUpdatedTime");

                    b.ToTable("File");
                });
#pragma warning restore 612, 618
        }
    }
}
