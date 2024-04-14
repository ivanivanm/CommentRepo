using DataBase.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base()
        {

        }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=city-reporter;user=root;password=godinlol3;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentDbModel>().Property(c => c.Id).UseIdentityColumn(100000);
            modelBuilder.Entity<UserDbModel>().Property(u => u.Id).UseIdentityColumn(100000);
            modelBuilder.Entity<ReportDbModel>().Property(r => r.IdReport).UseIdentityColumn(100000);

            modelBuilder.Entity<CommentDbModel>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CommentDbModel>()
                .HasOne(c => c.Report)
                .WithMany()
                .HasForeignKey(c => c.ReportId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
            
        public DbSet<UserDbModel> UserDbModels { get; set; }
        public DbSet<CommentDbModel> CommentDbModels { get; set; }

        public DbSet<ReportDbModel> ReportDbModels { get; set; }
    }
}
