﻿// <auto-generated />
using System;
using DictionaryBack.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DictionaryBack.DAL.Migrations
{
    [DbContext(typeof(DictionaryContext))]
    partial class DictionaryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("DictionaryBack.Domain.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("topics");
                });

            modelBuilder.Entity("DictionaryBack.Domain.Translation", b =>
                {
                    b.Property<string>("Term")
                        .HasColumnType("character varying(200)")
                        .HasColumnName("term");

                    b.Property<string>("Meaning")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("meaning");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                    b.HasKey("Term", "Meaning");

                    b.ToTable("translations");
                });

            modelBuilder.Entity("DictionaryBack.Domain.Word", b =>
                {
                    b.Property<string>("Term")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("term");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                    b.Property<DateTime?>("LastRepetition")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_repetition");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("status");

                    b.Property<int>("TopicId")
                        .HasColumnType("integer")
                        .HasColumnName("topic_id");

                    b.HasKey("Term");

                    b.HasIndex("TopicId");

                    b.ToTable("words");
                });

            modelBuilder.Entity("DictionaryBack.Domain.Translation", b =>
                {
                    b.HasOne("DictionaryBack.Domain.Word", null)
                        .WithMany("Translations")
                        .HasForeignKey("Term")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DictionaryBack.Domain.Word", b =>
                {
                    b.HasOne("DictionaryBack.Domain.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("DictionaryBack.Domain.Word", b =>
                {
                    b.Navigation("Translations");
                });
#pragma warning restore 612, 618
        }
    }
}
