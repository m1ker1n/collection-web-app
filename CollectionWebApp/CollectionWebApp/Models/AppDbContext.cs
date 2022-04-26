using Microsoft.EntityFrameworkCore;

namespace CollectionWebApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserCollection> UserCollections { get; set; } = null!;
        public DbSet<Theme> Themes { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<Field> Fields { get; set; } = null!;
        public DbSet<Like> Likes { get; set; } = null!;
        public DbSet<Commentary> Commentaries { get; set; } = null!;

        public readonly int UserRoleId = 1;
        public readonly int AdminRoleId = 2;
        public readonly string ImagePlaceholder = "files/image-placeholder.png";

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureEntities(modelBuilder);
            ConfigureRelationships(modelBuilder);
            Initialization(modelBuilder);
        }

        #region [Initialization]
        private void Initialization(ModelBuilder modelBuilder)
        {
            InitRoles(modelBuilder);
            InitUsers(modelBuilder);
            InitThemes(modelBuilder);
            //InitTest(modelBuilder);
        }

        private void InitRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasData(new Role[]
                { 
                    new Role { Id = UserRoleId, Name = "User" },
                    new Role { Id = AdminRoleId, Name = "Admin" }
                });
        }

        private void InitUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(new User
                { 
                    Id = 1,
                    Email = "admin@adm.in",
                    Name = "admin",
                    Password = "admin",
                    RoleId = 2
                });
        }

        private void InitThemes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Theme>()
                .HasData(new Theme[]
                {
                    new Theme { Id = 1, Name = "Alcohol" },
                    new Theme { Id = 2, Name = "Books" },
                    new Theme { Id = 3, Name = "Coins" }
                });
        }
        #endregion

        private void InitTest(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCollection>()
                .HasData(new UserCollection[]
                {
                    new UserCollection { Id = 1, Name = "First", Description = "Description 1", ThemeId = 1, UserId = 1},
                    new UserCollection { Id = 2, Name = "Second", Description = "Description 2", ThemeId = 2, UserId = 1}
                });

            modelBuilder.Entity<Item>()
                .HasData(new Item[]
                {
                    new Item { Id = 1, Name = "Item 1", UserCollectionId = 1},
                    new Item { Id = 2, Name = "Item 2", UserCollectionId = 1},
                    new Item { Id = 3, Name = "Item 3", UserCollectionId = 1},
                    new Item { Id = 4, Name = "Item 4", UserCollectionId = 2},
                    new Item { Id = 5, Name = "Item 5", UserCollectionId = 2}
                });

            modelBuilder.Entity<Tag>()
                .HasData(new Tag[]
                {
                    new Tag { Id = 1, Name = "Tag 1"},
                    new Tag { Id = 2, Name = "Tag 2"},
                    new Tag { Id = 3, Name = "Tag 3"},
                    new Tag { Id = 4, Name = "Tag 4"}
                });

            modelBuilder.Entity<TagItem>()
                .HasData(new TagItem[]
                {
                    new TagItem { TagId = 1, ItemId = 1},
                    new TagItem { TagId = 2, ItemId = 1},
                    new TagItem { TagId = 3, ItemId = 1},
                    new TagItem { TagId = 4, ItemId = 1},

                    new TagItem { TagId = 1, ItemId = 2},
                    new TagItem { TagId = 2, ItemId = 3},
                    new TagItem { TagId = 3, ItemId = 4}
                });
        }

        #region [Configuring entities]
        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
            ConfigureFieldItems(modelBuilder);
            ConfigureCommentaries(modelBuilder);
            ConfigureLikes(modelBuilder);
            ConfigureUsers(modelBuilder);
            ConfigureItems(modelBuilder);
            ConfigureUserCollections(modelBuilder);
        }

        private void ConfigureFieldItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FieldItem>()
                .Ignore(fi => fi.Value);
        }

        private void ConfigureCommentaries(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Commentary>()
                .HasKey(c => new { c.UserId, c.ItemId });
            modelBuilder.Entity<Commentary>()
                .Property(c => c.Created)
                .HasDefaultValueSql("getdate()");
        }

        private void ConfigureLikes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
                .HasKey(l => new { l.UserId, l.ItemId });
            modelBuilder.Entity<Like>()
                .Property(l => l.Created)
                .HasDefaultValueSql("getdate()");
        }

        private void ConfigureUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Created)
                .HasDefaultValueSql("getdate()");
        }

        private void ConfigureItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .Property(u => u.Created)
                .HasDefaultValueSql("getdate()");
        }

        private void ConfigureUserCollections(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCollection>()
                .Property(u => u.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<UserCollection>()
                .Property(u => u.ImageUrl)
                .HasDefaultValue(ImagePlaceholder);
        }
        #endregion

        #region [Configuring Relationships]
        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            ConfigureItemsAndCommentaries(modelBuilder);
            ConfigureItemsAndFields(modelBuilder);
            ConfigureItemsAndLikes(modelBuilder);
            ConfigureItemsAndTags(modelBuilder);
            ConfigureUserCollectionsAndFields(modelBuilder);
            ConfigureUserCollectionsAndItems(modelBuilder);
            ConfigureUserCollectionsAndThemes(modelBuilder);
            ConfigureUsersAndCommentaries(modelBuilder);
            ConfigureUsersAndLikes(modelBuilder);
            ConfigureUsersAndRoles(modelBuilder);
            ConfigureUsersAndUserCollections(modelBuilder);
        }

        private void ConfigureUsersAndRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureUsersAndUserCollections(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCollection>()
                .HasOne(c => c.User)
                .WithMany(u => u.UserCollections)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureUsersAndLikes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureUsersAndCommentaries(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Commentary>()
                .HasOne(c => c.User)
                .WithMany(u => u.Commentaries)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureUserCollectionsAndThemes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCollection>()
                .HasOne(c => c.Theme)
                .WithMany(t => t.UserCollections)
                .HasForeignKey(c => c.ThemeId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureUserCollectionsAndFields(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Field>()
                .HasOne(f => f.UserCollection)
                .WithMany(c => c.Fields)
                .HasForeignKey(f => f.UserCollectionId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureUserCollectionsAndItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasOne(i => i.UserCollection)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.UserCollectionId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureItemsAndTags(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasMany(i => i.Tags)
                .WithMany(t => t.Items)
                .UsingEntity<TagItem>(
                    j => j
                        .HasOne(ti => ti.Tag)
                        .WithMany(t => t.TagItems)
                        .HasForeignKey(ti => ti.TagId)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne(ti => ti.Item)
                        .WithMany(i => i.TagItems)
                        .HasForeignKey(ti => ti.ItemId)
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey(t => new { t.ItemId, t.TagId});
                    }
                );
        }

        private void ConfigureItemsAndFields(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasMany(i => i.Fields)
                .WithMany(f => f.Items)
                .UsingEntity<FieldItem>(
                    j => j
                        .HasOne(fi => fi.Field)
                        .WithMany(f => f.FieldItems)
                        .HasForeignKey(fi => fi.FieldId)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne(fi => fi.Item)
                        .WithMany(i => i.FieldItems)
                        .HasForeignKey(fi => fi.ItemId)
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey(t => new { t.ItemId, t.FieldId });
                    }
                ) ;
        }

        private void ConfigureItemsAndLikes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Item)
                .WithMany(i => i.Likes)
                .HasForeignKey(l => l.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureItemsAndCommentaries(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Commentary>()
                .HasOne(c => c.Item)
                .WithMany(i => i.Commentaries)
                .HasForeignKey(c => c.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        #endregion
    }
}
