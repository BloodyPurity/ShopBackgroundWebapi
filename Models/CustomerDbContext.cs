using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShopBackgroundSystem.Models
{
    public partial class CustomerDbContext : DbContext
    {
        public CustomerDbContext()
        {
        }

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Announcement> Announcements { get; set; } = null!;
        public virtual DbSet<Announcementuser> Announcementusers { get; set; } = null!;
        public virtual DbSet<Asset> Assets { get; set; } = null!;
        public virtual DbSet<Assetrecord> Assetrecords { get; set; } = null!;
        public virtual DbSet<Good> Goods { get; set; } = null!;
        public virtual DbSet<Goodsstockinlog> Goodsstockinlogs { get; set; } = null!;
        public virtual DbSet<Goodsstockoutlog> Goodsstockoutlogs { get; set; } = null!;
        public virtual DbSet<Goodstype> Goodstypes { get; set; } = null!;
        public virtual DbSet<Provider> Providers { get; set; } = null!;
        public virtual DbSet<Usalary> Usalaries { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<VAnnouncementuser> VAnnouncementusers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.ToTable("announcement");

                entity.HasIndex(e => e.Owner, "owner");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Detail)
                    .HasMaxLength(4095)
                    .HasColumnName("detail");

                entity.Property(e => e.Isdeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("isdeleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Owner).HasColumnName("owner");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("time");

                entity.Property(e => e.Uid).HasColumnName("uid");
            });

            modelBuilder.Entity<Announcementuser>(entity =>
            {
                entity.HasKey(e => e.Rid)
                    .HasName("PRIMARY");

                entity.ToTable("announcementusers");

                entity.Property(e => e.Rid).HasColumnName("rid");

                entity.Property(e => e.Announcementid).HasColumnName("announcementid");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("asset");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Isincome)
                    .HasColumnType("bit(1)")
                    .HasColumnName("isincome");

                entity.Property(e => e.Money)
                    .HasColumnType("double(255,0)")
                    .HasColumnName("money");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Assetrecord>(entity =>
            {
                entity.ToTable("assetrecords");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Reason)
                    .HasMaxLength(255)
                    .HasColumnName("reason");

                entity.Property(e => e.Records).HasColumnName("records");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("time");
            });

            modelBuilder.Entity<Good>(entity =>
            {
                entity.ToTable("goods");

                entity.HasIndex(e => e.Gname, "gname");

                entity.HasIndex(e => e.Gtype, "gtype");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.Gcount).HasColumnName("gcount");

                entity.Property(e => e.Gicon)
                    .HasMaxLength(255)
                    .HasColumnName("gicon");

                entity.Property(e => e.Gname).HasColumnName("gname");

                entity.Property(e => e.Gtype).HasColumnName("gtype");

                entity.Property(e => e.Isdeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("isdeleted");

                entity.Property(e => e.Notes)
                    .HasMaxLength(255)
                    .HasColumnName("notes");

                entity.Property(e => e.Price)
                    .HasPrecision(10, 2)
                    .HasColumnName("price");
            });

            modelBuilder.Entity<Goodsstockinlog>(entity =>
            {
                entity.ToTable("goodsstockinlog");

                entity.HasIndex(e => e.Gname, "gname");

                entity.HasIndex(e => e.Pname, "pid");

                entity.HasIndex(e => e.Uaccount, "uid");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Gname).HasColumnName("gname");

                entity.Property(e => e.Ischecked)
                    .HasColumnType("bit(1)")
                    .HasColumnName("ischecked");

                entity.Property(e => e.Notes)
                    .HasMaxLength(255)
                    .HasColumnName("notes");

                entity.Property(e => e.Percost).HasColumnName("percost");

                entity.Property(e => e.Pname).HasColumnName("pname");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.Property(e => e.Uaccount).HasColumnName("uaccount");
            });

            modelBuilder.Entity<Goodsstockoutlog>(entity =>
            {
                entity.ToTable("goodsstockoutlog");

                entity.HasIndex(e => e.Gname, "gname");

                entity.HasIndex(e => e.Uaccount, "uaccount");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Gname).HasColumnName("gname");

                entity.Property(e => e.Ischecked)
                    .HasColumnType("bit(1)")
                    .HasColumnName("ischecked");

                entity.Property(e => e.Notes)
                    .HasMaxLength(255)
                    .HasColumnName("notes");

                entity.Property(e => e.Perprice).HasColumnName("perprice");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.Property(e => e.Uaccount).HasColumnName("uaccount");
            });

            modelBuilder.Entity<Goodstype>(entity =>
            {
                entity.ToTable("goodstype");

                entity.HasIndex(e => e.Name, "name");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.ToTable("provider");

                entity.HasIndex(e => e.Goodstype, "goodstype");

                entity.HasIndex(e => e.Holder, "holder");

                entity.HasIndex(e => e.Name, "name");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Goodstype).HasColumnName("goodstype");

                entity.Property(e => e.Holder).HasColumnName("holder");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Usalary>(entity =>
            {
                entity.ToTable("usalary");

                entity.HasIndex(e => e.Uid, "uid");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Countdown).HasColumnName("countdown");

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("time");

                entity.Property(e => e.Uid).HasColumnName("uid");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .HasName("PRIMARY");

                entity.ToTable("user");

                entity.HasIndex(e => e.Uaccount, "Uaccount");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Uaccount).HasMaxLength(30);

                entity.Property(e => e.Uadress)
                    .HasMaxLength(255)
                    .HasColumnName("UAdress");

                entity.Property(e => e.Ubirth).HasColumnType("datetime");

                entity.Property(e => e.Uicon).HasMaxLength(255);

                entity.Property(e => e.Uname)
                    .HasMaxLength(255)
                    .HasColumnName("UName");

                entity.Property(e => e.Uphone).HasMaxLength(11);

                entity.Property(e => e.Upwd).HasMaxLength(255);

                entity.Property(e => e.Usalary)
                    .HasPrecision(10, 2)
                    .HasColumnName("USalary");

                entity.Property(e => e.Usex)
                    .HasMaxLength(10)
                    .HasColumnName("USex");

                entity.Property(e => e.Utype).HasMaxLength(255);
            });

            modelBuilder.Entity<VAnnouncementuser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v_announcementuser");

                entity.Property(e => e.Announcementid).HasColumnName("announcementid");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Uaccount).HasMaxLength(30);

                entity.Property(e => e.Uname)
                    .HasMaxLength(255)
                    .HasColumnName("UName");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
