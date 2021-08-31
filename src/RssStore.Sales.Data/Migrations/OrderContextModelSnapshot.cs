﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RssStore.Sales.Data;

namespace RssStore.Sales.Data.Migrations
{
    [DbContext(typeof(SalesContext))]
    partial class OrderContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("Relational:Sequence:.MySequence", "'MySequence', '', '1000', '1', '', '', 'Int32', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RssStore.Sales.Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR MySequence")
                        .HasMaxLength(10);

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<decimal>("TotalValue")
                        .HasColumnType("decimal(10,2)");

                    b.Property<bool>("VoucherApplyed")
                        .HasColumnType("bit");

                    b.Property<Guid?>("VoucherId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("VoucherId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("RssStore.Sales.Domain.Entities.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10,2)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<decimal>("UnitValue")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("RssStore.Sales.Domain.Entities.Voucher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10,2)");

                    b.Property<bool>("Applyed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ApplyedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<decimal?>("DiscountValue")
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime");

                    b.Property<decimal?>("Percentual")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("VoucherDiscountType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Vouchers");
                });

            modelBuilder.Entity("RssStore.Sales.Domain.Entities.Order", b =>
                {
                    b.HasOne("RssStore.Sales.Domain.Entities.Voucher", "Voucher")
                        .WithMany("Orders")
                        .HasForeignKey("VoucherId");
                });

            modelBuilder.Entity("RssStore.Sales.Domain.Entities.OrderItem", b =>
                {
                    b.HasOne("RssStore.Sales.Domain.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
