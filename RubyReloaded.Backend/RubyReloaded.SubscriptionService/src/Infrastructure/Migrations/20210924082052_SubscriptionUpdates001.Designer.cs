﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RubyReloaded.SubscriptionService.Infrastructure.Persistence;

namespace RubyReloaded.SubscriptionService.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210924082052_SubscriptionUpdates001")]
    partial class SubscriptionUpdates001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.Entities.Currency", b =>
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

                    b.Property<int>("CurrencyCode")
                        .HasColumnType("int");

                    b.Property<string>("CurrencyCodeDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.Entities.PaymentChannel", b =>
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

                    b.Property<int>("CurrencyCode")
                        .HasColumnType("int");

                    b.Property<string>("CurrencyCodeDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

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

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TransactionFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TransactionRateType")
                        .HasColumnType("int");

                    b.Property<string>("TransactionRateTypeDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("PaymentChannels");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.Entities.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedByEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("InitialSubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaymentChannelId")
                        .HasColumnType("int");

                    b.Property<string>("PaymentChannelReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentChannelResponse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentChannelStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("int");

                    b.Property<string>("PaymentStatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RenewedSubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<int>("SubscriptionType")
                        .HasColumnType("int");

                    b.Property<string>("SubscriptionTypeDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransactionReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("SubscriberId");

                    b.HasIndex("SubscriptionPlanId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.Entities.SubscriptionPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("AllowDiscount")
                        .HasColumnType("bit");

                    b.Property<bool>("AllowFreeTrial")
                        .HasColumnType("bit");

                    b.Property<bool>("AllowMonthlyPricing")
                        .HasColumnType("bit");

                    b.Property<bool>("AllowYearlyPricing")
                        .HasColumnType("bit");

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

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfTemplates")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfUsers")
                        .HasColumnType("int");

                    b.Property<string>("Period")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ShowContactUsButton")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowSubscribeButton")
                        .HasColumnType("bit");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StorageSize")
                        .HasColumnType("int");

                    b.Property<int>("StorageSizeType")
                        .HasColumnType("int");

                    b.Property<string>("StorageSizeTypeDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriptionType")
                        .HasColumnType("int");

                    b.Property<string>("SubscriptionTypeDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SubscriptionPlans");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.Entities.SubscriptionPlanFeature", b =>
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

                    b.Property<int>("FeatureId")
                        .HasColumnType("int");

                    b.Property<string>("FeatureName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParentFeatureId")
                        .HasColumnType("int");

                    b.Property<int>("ParentFeatureName")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("SubscriptionPlanId");

                    b.ToTable("SubscriptionPlanFeatures");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.Entities.SubscriptionPlanPricing", b =>
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

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EnableMonthlyPricingPlan")
                        .HasColumnType("bit");

                    b.Property<bool>("EnableYearlyPricingPlan")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("MonthlyAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MonthlyDiscount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("YearlyAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("YearlyDiscount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("SubscriptionPlanId");

                    b.ToTable("SubscriptionPlanPricings");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.ViewModels.FeatureDto", b =>
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

                    b.Property<int>("ParentFeatureId")
                        .HasColumnType("int");

                    b.Property<int>("ParentFeatureName")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserAccessLevel")
                        .HasColumnType("int");

                    b.Property<string>("UserAccessLevelDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkflowUserCategory")
                        .HasColumnType("int");

                    b.Property<string>("WorkflowUserCategoryDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SubscriberId");

                    b.ToTable("FeatureDto");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.ViewModels.SubscriberDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
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

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Latitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Longitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Referrer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StaffSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriberAccessLevel")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberAccessLevelDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubscriberCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriberId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubscriberType")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberTypeDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubscriptionPurpose")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SubscriberDto");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.Entities.PaymentChannel", b =>
                {
                    b.HasOne("RubyReloaded.SubscriptionService.Domain.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.Entities.Subscription", b =>
                {
                    b.HasOne("RubyReloaded.SubscriptionService.Domain.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RubyReloaded.SubscriptionService.Domain.ViewModels.SubscriberDto", "Subscriber")
                        .WithMany()
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RubyReloaded.SubscriptionService.Domain.Entities.SubscriptionPlan", "SubscriptionPlan")
                        .WithMany()
                        .HasForeignKey("SubscriptionPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Subscriber");

                    b.Navigation("SubscriptionPlan");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.Entities.SubscriptionPlanFeature", b =>
                {
                    b.HasOne("RubyReloaded.SubscriptionService.Domain.ViewModels.FeatureDto", "Feature")
                        .WithMany()
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RubyReloaded.SubscriptionService.Domain.Entities.SubscriptionPlan", "SuscriptionPlan")
                        .WithMany()
                        .HasForeignKey("SubscriptionPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feature");

                    b.Navigation("SuscriptionPlan");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.Entities.SubscriptionPlanPricing", b =>
                {
                    b.HasOne("RubyReloaded.SubscriptionService.Domain.Entities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RubyReloaded.SubscriptionService.Domain.Entities.SubscriptionPlan", "SubscriptionPlan")
                        .WithMany("SubscriptionPlanPricings")
                        .HasForeignKey("SubscriptionPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("SubscriptionPlan");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.ViewModels.FeatureDto", b =>
                {
                    b.HasOne("RubyReloaded.SubscriptionService.Domain.ViewModels.SubscriberDto", "Subscriber")
                        .WithMany()
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("RubyReloaded.SubscriptionService.Domain.Entities.SubscriptionPlan", b =>
                {
                    b.Navigation("SubscriptionPlanPricings");
                });
#pragma warning restore 612, 618
        }
    }
}
