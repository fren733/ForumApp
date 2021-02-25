using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ForumApplication.Migrations;

namespace ForumApplication.Models
{
    public class ForumContext : DbContext
    {
        public ForumContext()
        { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<IsLiked> Likes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                        .HasRequired<User>(x => x.User)
                        .WithMany(y => y.Posts)
                        .HasForeignKey<int>(x => x.UserId);

            modelBuilder.Entity<Post>()
                        .HasMany(x => x.Posts);

        }
    }
}