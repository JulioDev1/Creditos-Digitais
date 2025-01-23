﻿// <auto-generated />
using System;
using Carteiras_Digitais.Infrasctruture.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Carteiras_Digitais.Infrasctruture.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250123162821_AddMigration")]
    partial class AddMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Carteiras_Digitais.Api.Domain.Models.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("ReceiverWalletId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SenderWalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SenderWalletId");

                    b.ToTable("transactions");
                });

            modelBuilder.Entity("Carteiras_Digitais.Api.Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Carteiras_Digitais.Api.Domain.Models.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("wallets");
                });

            modelBuilder.Entity("Carteiras_Digitais.Api.Domain.Models.Transaction", b =>
                {
                    b.HasOne("Carteiras_Digitais.Api.Domain.Models.Wallet", "Wallet")
                        .WithMany("Transactions")
                        .HasForeignKey("SenderWalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Carteiras_Digitais.Api.Domain.Models.Wallet", b =>
                {
                    b.HasOne("Carteiras_Digitais.Api.Domain.Models.User", "user")
                        .WithOne("wallet")
                        .HasForeignKey("Carteiras_Digitais.Api.Domain.Models.Wallet", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("Carteiras_Digitais.Api.Domain.Models.User", b =>
                {
                    b.Navigation("wallet")
                        .IsRequired();
                });

            modelBuilder.Entity("Carteiras_Digitais.Api.Domain.Models.Wallet", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
