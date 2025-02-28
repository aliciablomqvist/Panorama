// <copyright file="ApplicationDbContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PanoramaApp.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using PanoramaApp.Models;

    /// <summary>
    /// Represents the application's database context.
    /// Inherits from <see cref="IdentityDbContext{TUser}"/> to provide identity functionality for user management.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<GroupInvitation> GroupInvitations { get; set; } = default!;

        public DbSet<MovieList> MovieLists { get; set; }

        public DbSet<MovieListItem> MovieListItems { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<MovieCalendar> MovieCalendars { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<GroupChatContext> GroupChatContexts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(e => e.LoginProvider).HasMaxLength(450);
                entity.Property(e => e.ProviderKey).HasMaxLength(450);
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.Property(e => e.LoginProvider).HasMaxLength(450);
                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<MovieList>()
               .HasOne(ml => ml.Owner)
               .WithMany()
               .HasForeignKey(ml => ml.OwnerId);

            modelBuilder.Entity<MovieListItem>()
                .HasOne(ml => ml.Movie)
                .WithMany()
                .HasForeignKey(ml => ml.MovieId);

            modelBuilder.Entity<MovieListItem>()
                .HasOne(ml => ml.MovieList)
                .WithMany(ml => ml.Movies)
                .HasForeignKey(ml => ml.MovieListId);

            modelBuilder.Entity<MovieList>()
                .HasMany(ml => ml.SharedWithGroups)
                .WithMany(g => g.MovieLists);

            modelBuilder.Entity<MovieListItem>()
                .HasKey(mli => new { mli.MovieListId, mli.MovieId });

            modelBuilder.Entity<MovieListItem>()
                .HasOne(mli => mli.MovieList)
                .WithMany(ml => ml.Movies)
                .HasForeignKey(mli => mli.MovieListId);

            modelBuilder.Entity<MovieListItem>()
                .HasOne(mli => mli.Movie)
                .WithMany(m => m.MovieListItems)
                .HasForeignKey(mli => mli.MovieId);
        }
    }
}
