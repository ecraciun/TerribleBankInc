﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TerribleBankInc.Data;

namespace TerribleBankInc.Data.Migrations
{
    [DbContext(typeof(TerribleBankDbContext))]
    partial class TerribleBankDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TerribleBankInc.Models.Entities.BankAccount", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Approved")
                        .HasColumnType("bit");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("ClientId");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("TerribleBankInc.Models.Entities.BankTransaction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Approved")
                        .HasColumnType("bit");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<int?>("DestinationAccountId")
                        .HasColumnType("int");

                    b.Property<string>("DestinationAccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DestinationClientEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DestinationClientId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SourceAccountId")
                        .HasColumnType("int");

                    b.Property<int>("SourceClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("DestinationAccountId");

                    b.HasIndex("DestinationClientId");

                    b.HasIndex("SourceAccountId");

                    b.HasIndex("SourceClientId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("TerribleBankInc.Models.Entities.Client", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("TerribleBankInc.Models.Entities.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ForgotPasswordExpiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("ForgotPasswordToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TerribleBankInc.Models.Entities.BankAccount", b =>
                {
                    b.HasOne("TerribleBankInc.Models.Entities.Client", "Client")
                        .WithMany("BankAccounts")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TerribleBankInc.Models.Entities.BankTransaction", b =>
                {
                    b.HasOne("TerribleBankInc.Models.Entities.BankAccount", "DestinationBankAccount")
                        .WithMany("IncomingTransactions")
                        .HasForeignKey("DestinationAccountId");

                    b.HasOne("TerribleBankInc.Models.Entities.Client", "DestinationClient")
                        .WithMany()
                        .HasForeignKey("DestinationClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TerribleBankInc.Models.Entities.BankAccount", "SourceBankAccount")
                        .WithMany("OutgoingTransactions")
                        .HasForeignKey("SourceAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TerribleBankInc.Models.Entities.Client", "SourceClient")
                        .WithMany()
                        .HasForeignKey("SourceClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TerribleBankInc.Models.Entities.User", b =>
                {
                    b.HasOne("TerribleBankInc.Models.Entities.Client", "Client")
                        .WithOne("User")
                        .HasForeignKey("TerribleBankInc.Models.Entities.User", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
