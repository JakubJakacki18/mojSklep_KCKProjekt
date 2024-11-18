﻿// <auto-generated />
using System;
using Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Library.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241118051200_RestrictUserCart")]
    partial class RestrictUserCart
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence<int>("ProductIds", "shared")
                .StartsAt(10000000L);

            modelBuilder.Entity("Library.Models.CartProductModel", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartId"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("ShoppingCartHistoryModelId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CartId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShoppingCartHistoryModelId");

                    b.HasIndex("UserId");

                    b.ToTable("CartProductModel");
                });

            modelBuilder.Entity("Library.Models.ProductModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR shared.ProductIds");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("shelfColumn")
                        .HasColumnType("int");

                    b.Property<int>("shelfRow")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Library.Models.ShoppingCartHistoryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PaymentMethod")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ShoppingCartHistoryModel");
                });

            modelBuilder.Entity("Library.Models.UserModel", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Login = "admin",
                            Password = "admin",
                            Role = 7
                        },
                        new
                        {
                            UserId = 2,
                            Login = "user",
                            Password = "user",
                            Role = 1
                        });
                });

            modelBuilder.Entity("Library.Models.CartProductModel", b =>
                {
                    b.HasOne("Library.Models.ProductModel", "OriginalProduct")
                        .WithMany("CartProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Library.Models.ShoppingCartHistoryModel", null)
                        .WithMany("CartProducts")
                        .HasForeignKey("ShoppingCartHistoryModelId");

                    b.HasOne("Library.Models.UserModel", "User")
                        .WithMany("ProductsInCart")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("OriginalProduct");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Library.Models.ShoppingCartHistoryModel", b =>
                {
                    b.HasOne("Library.Models.UserModel", "User")
                        .WithMany("ShoppingCartHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Library.Models.ProductModel", b =>
                {
                    b.Navigation("CartProducts");
                });

            modelBuilder.Entity("Library.Models.ShoppingCartHistoryModel", b =>
                {
                    b.Navigation("CartProducts");
                });

            modelBuilder.Entity("Library.Models.UserModel", b =>
                {
                    b.Navigation("ProductsInCart");

                    b.Navigation("ShoppingCartHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
