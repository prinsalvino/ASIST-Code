using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class SportingContext : DbContext
    {
        public DbSet<UserBase> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        
        
        public SportingContext(DbContextOptions<SportingContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBase>().HasIndex(u => u.EmailAddress).IsUnique();
        }
    }
}