using AuthAPI.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AuthAPI.Services
{
    public interface IAuthDataProvider
    {
        IQueryable<User> Users { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<Language> Languages { get; }
        void Add<T>(T obj);
        void Delete<T>(T obj);
        void Update<T>(T obj);
        void SaveChanges();
        void Attach(object obj);
        Task SaveChangesAsync();
        void ApplyChanges();
    }

    public class AuthDataProvider : DbContext, IAuthDataProvider
    {
        public AuthDataProvider(DbContextOptions options) : base(options)
        {
        }

        void IAuthDataProvider.Add<T>(T obj) => this.Add(obj);
        public void Delete<T>(T obj) => base.Remove(obj);
        void IAuthDataProvider.SaveChanges() => base.SaveChanges();
        public Task SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().Property(x => x.Name).HasMaxLength(100);
            modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(400);

            modelBuilder.Entity<Role>().HasIndex(x => x.Name);
            modelBuilder.Entity<User>().HasIndex(x => x.Email);

            modelBuilder.Entity<Language>().HasIndex(x => x.Name);

            modelBuilder.Entity<Language>().HasData(new[] {
                new Language{ Id = 1, Name = "English", Code = "en", UICode = "en-US" },
                new Language{ Id = 2, Name = "Русский", Code = "ru", UICode = "ru-RU" }
            });

            modelBuilder.Entity<User>().Property("LanguageId").HasDefaultValue(1);
            modelBuilder.Entity<User>().Property("RoleId").HasDefaultValue(1);

            modelBuilder.Entity<Role>().HasData(new[] {
                new Role{ Id = 1, Name = "User" },
                new Role{ Id = 2, Name = "Admin" },
                new Role{ Id = 3, Name = "Service" },
            });

            modelBuilder.Entity<User>().HasData(new[] { 
                new User{ Email = "admin@admin.ru", LanguageId = 1, Nickname = "admin", Id = 1, PasswordHash = CryptService.CreateHash("admin"), RoleId = 2 },
                new User{ Email = "service@service.ru", LanguageId = 1, Nickname = "service", Id = 2, PasswordHash = CryptService.CreateHash("service"), RoleId = 3 },
            });
        }

        void IAuthDataProvider.Update<T>(T obj) => base.Update(obj);
        void IAuthDataProvider.Attach(object obj) => base.Attach(obj);

        public void ApplyChanges()
        {
            Database.Migrate();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Language> Languages { get; set; }

        IQueryable<User> IAuthDataProvider.Users => this.Users;
        IQueryable<Role> IAuthDataProvider.Roles => this.Roles;
        IQueryable<Language> IAuthDataProvider.Languages => this.Languages;
    }
}
