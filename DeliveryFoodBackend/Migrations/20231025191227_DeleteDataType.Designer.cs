﻿// <auto-generated />
using System;
using DeliveryFoodBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeliveryFoodBackend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231025191227_DeleteDataType")]
    partial class DeleteDataType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.Basket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.HasIndex("OrderId");

                    b.HasIndex("UserId", "OrderId", "DishId")
                        .IsUnique();

                    b.ToTable("Baskets");
                });

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<double?>("Rating")
                        .HasColumnType("double precision");

                    b.Property<bool>("Vegetarian")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.Rating", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<double>("RatingScore")
                        .HasColumnType("double precision");

                    b.HasKey("UserId", "DishId");

                    b.HasIndex("DishId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.Token", b =>
                {
                    b.Property<string>("InvalideToken")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpiredDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("InvalideToken");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Genders")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.Basket", b =>
                {
                    b.HasOne("DeliveryFoodBackend.Data.Models.Dish", "Dish")
                        .WithMany("Baskets")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryFoodBackend.Data.Models.Order", "Order")
                        .WithMany("Baskets")
                        .HasForeignKey("OrderId");

                    b.HasOne("DeliveryFoodBackend.Data.Models.User", "User")
                        .WithMany("Baskets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.Order", b =>
                {
                    b.HasOne("DeliveryFoodBackend.Data.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.Rating", b =>
                {
                    b.HasOne("DeliveryFoodBackend.Data.Models.Dish", "Dish")
                        .WithMany("Ratings")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryFoodBackend.Data.Models.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.Dish", b =>
                {
                    b.Navigation("Baskets");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.Order", b =>
                {
                    b.Navigation("Baskets");
                });

            modelBuilder.Entity("DeliveryFoodBackend.Data.Models.User", b =>
                {
                    b.Navigation("Baskets");

                    b.Navigation("Orders");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
