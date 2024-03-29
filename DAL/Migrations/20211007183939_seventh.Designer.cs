﻿// <auto-generated />
using System;
using ASIST.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(SportingContext))]
    [Migration("20211007183939_seventh")]
    partial class seventh
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoachOrganisation", b =>
                {
                    b.Property<long>("CoachesUserId")
                        .HasColumnType("bigint");

                    b.Property<long>("OrganisationsOrganisationId")
                        .HasColumnType("bigint");

                    b.HasKey("CoachesUserId", "OrganisationsOrganisationId");

                    b.HasIndex("OrganisationsOrganisationId");

                    b.ToTable("CoachOrganisation");
                });

            modelBuilder.Entity("Domain.Organisation", b =>
                {
                    b.Property<long>("OrganisationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("OrganisationName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrganisationId");

                    b.ToTable("Organisation");
                });

            modelBuilder.Entity("Domain.Skill", b =>
                {
                    b.Property<long>("SkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("SkillId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("Domain.SkillStudent", b =>
                {
                    b.Property<long>("SkillStudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CoachId")
                        .HasColumnType("bigint");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<long?>("SkillId")
                        .HasColumnType("bigint");

                    b.Property<long>("StudentId")
                        .HasColumnType("bigint");

                    b.Property<float>("TimeOfCompletion")
                        .HasColumnType("real");

                    b.HasKey("SkillStudentId");

                    b.HasIndex("CoachId");

                    b.HasIndex("SkillId");

                    b.HasIndex("StudentId");

                    b.ToTable("SkillStudent");
                });

            modelBuilder.Entity("Domain.Sport", b =>
                {
                    b.Property<long>("SportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SportId");

                    b.ToTable("Sports");
                });

            modelBuilder.Entity("Domain.SportStudent", b =>
                {
                    b.Property<long>("SportStudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<long?>("SportId")
                        .HasColumnType("bigint");

                    b.Property<long?>("StudentUserId")
                        .HasColumnType("bigint");

                    b.HasKey("SportStudentId");

                    b.HasIndex("SportId");

                    b.HasIndex("StudentUserId");

                    b.ToTable("SportStudent");
                });

            modelBuilder.Entity("Domain.UserBase", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("UserBase");
                });

            modelBuilder.Entity("Domain.Admin", b =>
                {
                    b.HasBaseType("Domain.UserBase");

                    b.HasDiscriminator().HasValue("Admin");
                });

            modelBuilder.Entity("Domain.Coach", b =>
                {
                    b.HasBaseType("Domain.UserBase");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Coach");
                });

            modelBuilder.Entity("Domain.Student", b =>
                {
                    b.HasBaseType("Domain.UserBase");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<float>("FinalScore")
                        .HasColumnType("real");

                    b.Property<float>("Height")
                        .HasColumnType("real");

                    b.Property<long?>("OrganisationId")
                        .HasColumnType("bigint");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasIndex("OrganisationId");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("CoachOrganisation", b =>
                {
                    b.HasOne("Domain.Coach", null)
                        .WithMany()
                        .HasForeignKey("CoachesUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Organisation", null)
                        .WithMany()
                        .HasForeignKey("OrganisationsOrganisationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.SkillStudent", b =>
                {
                    b.HasOne("Domain.Coach", "Coach")
                        .WithMany("SkillsAssessed")
                        .HasForeignKey("CoachId");

                    b.HasOne("Domain.Skill", "Skill")
                        .WithMany("Students")
                        .HasForeignKey("SkillId");

                    b.HasOne("Domain.Student", "Student")
                        .WithMany("SkillsPerformed")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coach");

                    b.Navigation("Skill");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Domain.SportStudent", b =>
                {
                    b.HasOne("Domain.Sport", "Sport")
                        .WithMany("SportAdvices")
                        .HasForeignKey("SportId");

                    b.HasOne("Domain.Student", "Student")
                        .WithMany("SportAdvices")
                        .HasForeignKey("StudentUserId");

                    b.Navigation("Sport");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Domain.Student", b =>
                {
                    b.HasOne("Domain.Organisation", "Organisation")
                        .WithMany("Students")
                        .HasForeignKey("OrganisationId");

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("Domain.Organisation", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("Domain.Skill", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("Domain.Sport", b =>
                {
                    b.Navigation("SportAdvices");
                });

            modelBuilder.Entity("Domain.Coach", b =>
                {
                    b.Navigation("SkillsAssessed");
                });

            modelBuilder.Entity("Domain.Student", b =>
                {
                    b.Navigation("SkillsPerformed");

                    b.Navigation("SportAdvices");
                });
#pragma warning restore 612, 618
        }
    }
}
