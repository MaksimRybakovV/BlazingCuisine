using BlazingCuisine.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BlazingCuisine.Server.Data
{
    public class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>().Property(r => r.Ingredients)
                .HasConversion(
                    l => JsonSerializer.Serialize(l, (JsonSerializerOptions)null!),
                    l => JsonSerializer.Deserialize<List<Ingredient>>(l, (JsonSerializerOptions)null!)!);
            modelBuilder.Entity<Recipe>().Property(r => r.Owner)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"DataSource={Environment.CurrentDirectory}/Data/DataBase/BlazingCuisineDB.db");
        }

        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Category> Categories => Set<Category>();
    }
}
