﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserManagementService.DAL.Context;

#nullable disable

namespace UserManagementService.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.InboxState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Consumed")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ConsumerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("LockId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReceiveCount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Received")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("Delivered");

                    b.ToTable("InboxState");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxMessage", b =>
                {
                    b.Property<long>("SequenceNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ConversationId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DestinationAddress")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EnqueueTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("FaultAddress")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Headers")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("InboxConsumerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("InboxMessageId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("InitiatorId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OutboxId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Properties")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("RequestId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResponseAddress")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourceAddress")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("SequenceNumber");

                    b.HasIndex("EnqueueTime");

                    b.HasIndex("ExpirationTime");

                    b.HasIndex("OutboxId", "SequenceNumber")
                        .IsUnique();

                    b.HasIndex("InboxMessageId", "InboxConsumerId", "SequenceNumber")
                        .IsUnique();

                    b.ToTable("OutboxMessage");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxState", b =>
                {
                    b.Property<Guid>("OutboxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("LockId")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("OutboxId");

                    b.HasIndex("Created");

                    b.ToTable("OutboxState");
                });

            modelBuilder.Entity("UserManagementService.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxMessage", b =>
                {
                    b.HasOne("MassTransit.EntityFrameworkCoreIntegration.OutboxState", null)
                        .WithMany()
                        .HasForeignKey("OutboxId");

                    b.HasOne("MassTransit.EntityFrameworkCoreIntegration.InboxState", null)
                        .WithMany()
                        .HasForeignKey("InboxMessageId", "InboxConsumerId")
                        .HasPrincipalKey("MessageId", "ConsumerId");
                });
#pragma warning restore 612, 618
        }
    }
}
