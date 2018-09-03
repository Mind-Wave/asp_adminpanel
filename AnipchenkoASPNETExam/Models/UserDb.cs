namespace AnipchenkoASPNETExam.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UserDb : DbContext
    {
        public UserDb()
            : base("name=UserDb")
        {
        }

        public virtual DbSet<admins> admins { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<users>()
                .Property(e => e.login)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.pass)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.surname)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.imageurl)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .HasMany(e => e.admins)
                .WithOptional(e => e.users)
                .HasForeignKey(e => e.userID);
        }
    }
}
