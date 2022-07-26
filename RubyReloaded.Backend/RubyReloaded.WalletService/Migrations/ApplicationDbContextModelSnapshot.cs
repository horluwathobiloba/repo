﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RubyReloaded.WalletService.Infrastructure.Persistence;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("RubyReloaded.WalletService.Domain.Enitities.BankConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedByEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<string>("OrganisationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BankConfigs");
                });

            modelBuilder.Entity("RubyReloaded.WalletService.Domain.Enitities.ProductCategoryConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedByEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<string>("OrganisationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductClass")
                        .HasColumnType("int");

                    b.Property<string>("ProductClassDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductCategoryConfigs");
                });

            modelBuilder.Entity("RubyReloaded.WalletService.Domain.Enitities.ProductCategoryConfigServiceConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ProductCategoryConfigId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceConfigId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductCategoryConfigId");

                    b.HasIndex("ServiceConfigId");

                    b.ToTable("ProductCategoryConfigServiceConfig");
                });

            modelBuilder.Entity("RubyReloaded.WalletService.Domain.Enitities.ProductConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedByEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("MaxAmount")
                        .HasColumnType("float");

                    b.Property<double>("MinAmount")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<string>("OrganisationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ProductType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TransactionFee")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("ProductConfigs");
                });

            modelBuilder.Entity("RubyReloaded.WalletService.Domain.Enitities.ServiceConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Biller")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedByEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Fee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("FeeType")
                        .HasColumnType("int");

                    b.Property<string>("FeeTypeDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<string>("OrganisationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServiceCategory")
                        .HasColumnType("int");

                    b.Property<string>("ServiceCategoryDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServiceType")
                        .HasColumnType("int");

                    b.Property<string>("ServiceTypeDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ServiceConfigs");
                });

            modelBuilder.Entity("RubyReloaded.WalletService.Domain.Enitities.VirtualAccountConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("BankId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedByEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<string>("OrganisationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SettlementAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VirtualAccountConfigs");
                });

            modelBuilder.Entity("RubyReloaded.WalletService.Domain.Enitities.ProductCategoryConfigServiceConfig", b =>
                {
                    b.HasOne("RubyReloaded.WalletService.Domain.Enitities.ProductCategoryConfig", "ProductCategoryConfig")
                        .WithMany("ProductCategoryConfigServiceConfigs")
                        .HasForeignKey("ProductCategoryConfigId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RubyReloaded.WalletService.Domain.Enitities.ServiceConfig", "ServiceConfig")
                        .WithMany("ProductCategoryConfigServiceConfigs")
                        .HasForeignKey("ServiceConfigId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductCategoryConfig");

                    b.Navigation("ServiceConfig");
                });

            modelBuilder.Entity("RubyReloaded.WalletService.Domain.Enitities.ProductCategoryConfig", b =>
                {
                    b.Navigation("ProductCategoryConfigServiceConfigs");
                });

            modelBuilder.Entity("RubyReloaded.WalletService.Domain.Enitities.ServiceConfig", b =>
                {
                    b.Navigation("ProductCategoryConfigServiceConfigs");
                });
#pragma warning restore 612, 618
        }
    }
}