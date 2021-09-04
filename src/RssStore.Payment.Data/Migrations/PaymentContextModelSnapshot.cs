﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RssStore.Payment.Data.Context;

namespace RssStore.Payment.Data.Migrations
{
    [DbContext(typeof(PaymentContext))]
    partial class PaymentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RssStore.Payment.Business.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CardCvv")
                        .IsRequired()
                        .HasColumnType("varchar(4)");

                    b.Property<string>("CardExpirationDate")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("CardName")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("varchar(16)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("RssStore.Payment.Business.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("TransactionStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("RssStore.Payment.Business.Entities.Transaction", b =>
                {
                    b.HasOne("RssStore.Payment.Business.Entities.Payment", "Payment")
                        .WithOne("Transaction")
                        .HasForeignKey("RssStore.Payment.Business.Entities.Transaction", "PaymentId")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}