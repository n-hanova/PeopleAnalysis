using Microsoft.EntityFrameworkCore;
using PeopleAnalysis.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleAnalysis.Services
{
    public interface IDatabaseContext
    {
        IQueryable<Request> Requests { get; }
        IQueryable<Result> Results { get; }
        IQueryable<AnalysObject> AnalysObjects { get; }
        IQueryable<ResultObject> ResultObjects { get; }
        Task SaveChangesAsync();
        void SaveChanges();

        void Add(object obj);
        void Remove(object obj);
        void Update(object obj);
        void ApplyMigration();
    }

    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Request> Requests { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<AnalysObject> AnalysObjects { get; set; }
        public DbSet<ResultObject> ResultObjects { get; set; }

        IQueryable<Request> IDatabaseContext.Requests => Requests;
        IQueryable<Result> IDatabaseContext.Results => Results;
        IQueryable<AnalysObject> IDatabaseContext.AnalysObjects => AnalysObjects;
        IQueryable<ResultObject> IDatabaseContext.ResultObjects => ResultObjects;

        public void ApplyMigration() => Database.Migrate();

        public Task SaveChangesAsync() => base.SaveChangesAsync();

        void IDatabaseContext.Add(object obj) => base.Add(obj);
        void IDatabaseContext.Remove(object obj) => base.Remove(obj);

        void IDatabaseContext.SaveChanges() => base.SaveChanges();

        void IDatabaseContext.Update(object obj) => base.Update(obj);
    }
}
