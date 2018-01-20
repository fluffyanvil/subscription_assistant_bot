using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SubscriptionAssistantBot.Db.Model
{
    public partial class SubscriptionsDbContext : DbContext
    {
        public virtual DbSet<Subscription> Subscription { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql(@"%your_db_connection_string%");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Subscriptions_Id_seq\"'::regclass)");
                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.Subscription)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TagToSubscription");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Tags_Id_seq\"'::regclass)");
            });

            modelBuilder.HasSequence("Subscriptions_Id_seq")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("Tags_Id_seq")
                .HasMin(1)
                .HasMax(2147483647);
        }
    }
}
