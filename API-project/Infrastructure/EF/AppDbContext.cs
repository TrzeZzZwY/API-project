using AppCore.Models;
using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF
{
    public class AppDbContext : IdentityDbContext<UserEntity, UserRoleEntity, string>
    {
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<PublishAlbumEntity> Albums { get; set; }
        public DbSet<PublishEntity> Publishes { get; set; }
        public DbSet<PublishTagEntity> Tags { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserEntity>().HasMany(e => e.Publishes).WithOne(e => e.User);
            builder.Entity<UserEntity>().HasMany(e => e.PublishLikes).WithMany(e => e.UserLikes).UsingEntity(j => j.ToTable("UserLikes"));
            builder.Entity<PublishTagEntity>().HasMany(e => e.Publishes).WithMany(e => e.PublishTags).UsingEntity(j => j.ToTable("Publish_PublishTag"));

            builder.Entity<UserRoleEntity>().HasData(
                new UserRoleEntity() { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" },
                new UserRoleEntity() { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER" }
            );

            base.OnModelCreating(builder);
        }
    }
}
