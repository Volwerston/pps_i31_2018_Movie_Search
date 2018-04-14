﻿// <auto-generated />
using FilmSearch.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace FilmSearch.Migrations
{
    [DbContext(typeof(FilmSearchContext))]
    partial class FilmSearchContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("FilmSearch.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("FilmSearch.Models.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<long>("CommentRate");

                    b.Property<string>("CreationDate");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<long>("ParentCommentId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ParentCommentId");

                    b.ToTable("Comment");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Comment");
                });

            modelBuilder.Entity("FilmSearch.Models.CommentOpinion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Approval");

                    b.Property<long>("CommentId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("UserId");

                    b.ToTable("CommentOpinions");
                });

            modelBuilder.Entity("FilmSearch.Models.Entities.PersonComment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<string>("CreationDate");

                    b.Property<long>("PersonId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("PersonComments");
                });

            modelBuilder.Entity("FilmSearch.Models.File", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName");

                    b.Property<string>("FileType");

                    b.Property<string>("Path");

                    b.Property<DateTime>("UploadDate");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("FilmSearch.Models.Film", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Performance");

                    b.Property<long?>("PhotoId");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("ShortDescription");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.ToTable("Films");
                });

            modelBuilder.Entity("FilmSearch.Models.FilmGenre", b =>
                {
                    b.Property<long>("FilmId");

                    b.Property<long>("GenreId");

                    b.HasKey("FilmId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("FilmGenres");
                });

            modelBuilder.Entity("FilmSearch.Models.FilmPerformance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("FilmId");

                    b.Property<long>("Performance");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("UserId");

                    b.ToTable("FilmPerformances");
                });

            modelBuilder.Entity("FilmSearch.Models.FilmRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("FilmRoles");
                });

            modelBuilder.Entity("FilmSearch.Models.Genre", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("FilmSearch.Models.Person", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("Country");

                    b.Property<string>("Name");

                    b.Property<long?>("PhotoId");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("FilmSearch.Models.PersonPerformance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Performance");

                    b.Property<long>("PersonRoleId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PersonRoleId");

                    b.HasIndex("UserId");

                    b.ToTable("PersonPerformances");
                });

            modelBuilder.Entity("FilmSearch.Models.PersonRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<long>("FilmId");

                    b.Property<long>("FilmRoleId");

                    b.Property<double>("Performance");

                    b.Property<long>("PersonId");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("FilmRoleId");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonRoles");
                });

            modelBuilder.Entity("FilmSearch.Models.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId");

                    b.Property<DateTime>("CreationTime");

                    b.Property<long>("ImageId");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("FilmSearch.Models.Poster", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AdditionDateTime");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Posters");
                });

            modelBuilder.Entity("FilmSearch.Models.PostOpinion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Approval");

                    b.Property<long>("PostId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostOpinions");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FilmSearch.Models.PostComment", b =>
                {
                    b.HasBaseType("FilmSearch.Models.Comment");

                    b.Property<long>("PostId");

                    b.HasIndex("PostId");

                    b.ToTable("PostComment");

                    b.HasDiscriminator().HasValue("PostComment");
                });

            modelBuilder.Entity("FilmSearch.Models.Comment", b =>
                {
                    b.HasOne("FilmSearch.Models.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("FilmSearch.Models.Comment", "ParentComment")
                        .WithMany("SubComments")
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FilmSearch.Models.CommentOpinion", b =>
                {
                    b.HasOne("FilmSearch.Models.Comment", "Comment")
                        .WithMany()
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FilmSearch.Models.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("FilmSearch.Models.Entities.PersonComment", b =>
                {
                    b.HasOne("FilmSearch.Models.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("FilmSearch.Models.Film", b =>
                {
                    b.HasOne("FilmSearch.Models.File", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("FilmSearch.Models.FilmGenre", b =>
                {
                    b.HasOne("FilmSearch.Models.Film", "Film")
                        .WithMany("Genres")
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FilmSearch.Models.Genre", "Genre")
                        .WithMany("Films")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FilmSearch.Models.FilmPerformance", b =>
                {
                    b.HasOne("FilmSearch.Models.Film", "Film")
                        .WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FilmSearch.Models.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("FilmSearch.Models.Person", b =>
                {
                    b.HasOne("FilmSearch.Models.File", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("FilmSearch.Models.PersonPerformance", b =>
                {
                    b.HasOne("FilmSearch.Models.PersonRole", "PersonRole")
                        .WithMany()
                        .HasForeignKey("PersonRoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FilmSearch.Models.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("FilmSearch.Models.PersonRole", b =>
                {
                    b.HasOne("FilmSearch.Models.Film", "Film")
                        .WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FilmSearch.Models.FilmRole", "FilmRole")
                        .WithMany("PersonRoles")
                        .HasForeignKey("FilmRoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FilmSearch.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FilmSearch.Models.Post", b =>
                {
                    b.HasOne("FilmSearch.Models.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("FilmSearch.Models.PostOpinion", b =>
                {
                    b.HasOne("FilmSearch.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FilmSearch.Models.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FilmSearch.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FilmSearch.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FilmSearch.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FilmSearch.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FilmSearch.Models.PostComment", b =>
                {
                    b.HasOne("FilmSearch.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
