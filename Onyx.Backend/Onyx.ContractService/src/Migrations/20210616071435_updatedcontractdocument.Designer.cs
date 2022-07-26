﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Onyx.ContractService.Infrastructure.Persistence;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210616071435_updatedcontractdocument")]
    partial class updatedcontractdocument
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ContractExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ContractStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ContractStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContractTypeId")
                        .HasColumnType("int");

                    b.Property<decimal>("ContractValue")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsComplianceDueDiligence")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTouchPointVendor")
                        .HasColumnType("bit");

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

                    b.Property<int>("PaymentPlanId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductServiceTypeId")
                        .HasColumnType("int");

                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Signature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaxId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VendorId")
                        .HasColumnType("int");

                    b.Property<int>("VendorTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PaymentPlanId");

                    b.HasIndex("ProductServiceTypeId");

                    b.HasIndex("VendorId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.ContractComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Attachment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CommentById")
                        .HasColumnType("int");

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
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

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.ToTable("ContractComments");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.ContractDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentSigningUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("File")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsSigned")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSupportingDocument")
                        .HasColumnType("bit");

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

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.ToTable("ContractDocuments");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.ContractType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ContractTypeStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EnableContractTypeInitiatorRestriction")
                        .HasColumnType("bit");

                    b.Property<bool>("EnableExternalSignatories")
                        .HasColumnType("bit");

                    b.Property<bool>("EnableInternalSignatories")
                        .HasColumnType("bit");

                    b.Property<bool>("EnableThirdPartySignatories")
                        .HasColumnType("bit");

                    b.Property<bool>("EnableWitnessSignatories")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfExternalSignatories")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfInternalSignatories")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfThirdPartySignatories")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfWitnessSignatories")
                        .HasColumnType("int");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<string>("OrganisationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateFilePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ContractTypes");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.ContractTypeInitiator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ContractTypeId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
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

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContractTypeId");

                    b.ToTable("ContractTypeInitiators");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.Dimension", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("File")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Height")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Left")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<string>("OrganisationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Top")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Width")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Dimensions");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.PaymentPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
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

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PaymentPlans");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.ProductServiceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
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

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VendorTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VendorTypeId");

                    b.ToTable("ProductServiceTypes");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.Vendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
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

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaxId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TouchPointVendor")
                        .HasColumnType("bit");

                    b.Property<int>("VendorTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VendorTypeId");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.VendorType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
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

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VendorTypes");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.WorkflowLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedBy")
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

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkflowLevelAction")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkflowPhaseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowPhaseId");

                    b.ToTable("WorkflowLevels");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.WorkflowPhase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ContractTypeId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
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
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<string>("OrganisationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkflowSequence")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkflowUserCategory")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContractTypeId");

                    b.HasIndex("Name");

                    b.ToTable("WorkflowPhases");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.Contract", b =>
                {
                    b.HasOne("Onyx.ContractService.Domain.Entities.PaymentPlan", "PaymentPlan")
                        .WithMany()
                        .HasForeignKey("PaymentPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Onyx.ContractService.Domain.Entities.ProductServiceType", "ProductServiceType")
                        .WithMany()
                        .HasForeignKey("ProductServiceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Onyx.ContractService.Domain.Entities.Vendor", "Vendor")
                        .WithMany("Contracts")
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("PaymentPlan");

                    b.Navigation("ProductServiceType");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.ContractComment", b =>
                {
                    b.HasOne("Onyx.ContractService.Domain.Entities.Contract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.ContractDocument", b =>
                {
                    b.HasOne("Onyx.ContractService.Domain.Entities.Contract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.ContractTypeInitiator", b =>
                {
                    b.HasOne("Onyx.ContractService.Domain.Entities.ContractType", "ContractType")
                        .WithMany("ContractTypeInitiators")
                        .HasForeignKey("ContractTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContractType");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.ProductServiceType", b =>
                {
                    b.HasOne("Onyx.ContractService.Domain.Entities.VendorType", "VendorType")
                        .WithMany()
                        .HasForeignKey("VendorTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VendorType");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.Vendor", b =>
                {
                    b.HasOne("Onyx.ContractService.Domain.Entities.VendorType", "VendorType")
                        .WithMany()
                        .HasForeignKey("VendorTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VendorType");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.WorkflowLevel", b =>
                {
                    b.HasOne("Onyx.ContractService.Domain.Entities.WorkflowPhase", "WorkflowPhase")
                        .WithMany("WorkflowLevels")
                        .HasForeignKey("WorkflowPhaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkflowPhase");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.WorkflowPhase", b =>
                {
                    b.HasOne("Onyx.ContractService.Domain.Entities.ContractType", "ContractType")
                        .WithMany("WorkflowPhases")
                        .HasForeignKey("ContractTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContractType");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.ContractType", b =>
                {
                    b.Navigation("ContractTypeInitiators");

                    b.Navigation("WorkflowPhases");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.Vendor", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("Onyx.ContractService.Domain.Entities.WorkflowPhase", b =>
                {
                    b.Navigation("WorkflowLevels");
                });
#pragma warning restore 612, 618
        }
    }
}
