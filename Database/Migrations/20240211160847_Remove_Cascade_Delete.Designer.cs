﻿// <auto-generated />
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(SampleDbContext))]
    [Migration("20240211160847_Remove_Cascade_Delete")]
    partial class Remove_Cascade_Delete
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("Entities.BlogPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BlogPosts");
                });

            modelBuilder.Entity("Entities.BlogPostAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttachmentId")
                        .HasColumnType("int");

                    b.Property<int>("BlogPostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("BlogPostId");

                    b.ToTable("BlogPostAttachments");
                });

            modelBuilder.Entity("Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Entities.MessageAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttachmentId")
                        .HasColumnType("int");

                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("MessageId");

                    b.ToTable("MessageAttachments");
                });

            modelBuilder.Entity("Entities.BlogPostAttachment", b =>
                {
                    b.HasOne("Entities.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Entities.BlogPost", "BlogPost")
                        .WithMany()
                        .HasForeignKey("BlogPostId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Attachment");

                    b.Navigation("BlogPost");
                });

            modelBuilder.Entity("Entities.MessageAttachment", b =>
                {
                    b.HasOne("Entities.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Entities.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Attachment");

                    b.Navigation("Message");
                });
#pragma warning restore 612, 618
        }
    }
}
