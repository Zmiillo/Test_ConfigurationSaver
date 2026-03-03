using CS.DataLibrary.Models;
using CS.DataLibrary.Models.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace CS.DataLibrary
{
    public partial class CSContext : DbContext
    {
        public CSContext()
        {
        }

        public CSContext(DbContextOptions<CSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CSColor> Colors { get; set; }
        public virtual DbSet<CSFont> Fonts { get; set; }
        public virtual DbSet<CSUserConfiguration> UserConfigurations { get; set; }
        public virtual DbSet<CSUser> Users { get; set; }
        public virtual DbSet<CSSubscribeType> SubscribeTypes { get; set; }
        public virtual DbSet<CSMessage> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CSUserConfiguration>(entity =>
            {
                entity.ToTable("UserConfigurations", "main");               

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserConfigurations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserConfigurations_Users");

                entity.HasOne(d => d.Font)
                    .WithMany(p => p.UserConfigurations)
                    .HasForeignKey(d => d.FontId)
                    .HasConstraintName("FK_UserConfigurations_Fonts");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.UserConfigurations)
                    .HasForeignKey(d => d.ColorId)
                    .HasConstraintName("FK_UserConfigurations_Colors");
            });

            modelBuilder.Entity<CSColor>(entity =>
            {
                entity.HasComment("Справочник доступных цветовых палитр");

                entity.ToTable("Colors", "main");
            });

            modelBuilder.Entity<CSFont>(entity =>
            {
                entity.HasComment("Справочник доступных шрифтов");

                entity.ToTable("Fonts", "main");
            });

            modelBuilder.Entity<CSUser>(entity =>
            {
                entity.HasComment("Пользователи");

                entity.ToTable("Users", "main");
            });

            modelBuilder.Entity<CSSubscribeType>(entity =>
            {
                entity.HasComment("Типы подписок");

                entity.ToTable("SubscribeTypes", "main");
            });

            modelBuilder.Entity<CSMessage>(entity =>
            {
                entity.HasComment("Хранимые сообщения");

                entity.ToTable("Messages", "main");

                entity.HasOne(d => d.SubscribeType)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.SubscribeId)
                    .HasConstraintName("FK_Messages_SubscribeTypes");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
