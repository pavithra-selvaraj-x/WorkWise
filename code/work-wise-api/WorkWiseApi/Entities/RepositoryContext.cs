using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Entities.Util;

namespace Entities;

public class RepositoryContext : DbContext
{

    public RepositoryContext(DbContextOptions options) : base(options)
    {

    }

    /// <summary>
    /// This method used to define the properties of data model and attributes.
    /// It will be invoked before creating the tables from model during the migrations
    /// and the configured properties will be applied to the table.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName().ConvertToSnakeCase());

            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableProperty property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ConvertToSnakeCase());
            }

            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableKey key in entity.GetKeys())
            {
                key.SetName(key.GetName().ConvertToSnakeCase());
            }

            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey key in entity.GetForeignKeys())
            {
                key.SetConstraintName(key.GetConstraintName().ConvertToSnakeCase());
            }
        }
    }

    /// <summary>
    /// This method used to override the model attributes.
    /// It will be invoked before the database transactions.
    /// </summary>
    public void OnBeforeSaving(Guid UserId)
    {
        System.Collections.Generic.IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> entries = ChangeTracker.Entries();
        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in entries)
        {
            if (entry.Entity is BaseModel trackable)
            {
                DateTime now = DateTime.UtcNow;
                Guid user = UserId;
                switch (entry.State)
                {
                    case EntityState.Modified:
                        trackable.DateUpdated = now;
                        trackable.UpdatedBy = user;
                        break;

                    case EntityState.Added:
                        trackable.DateCreated = now;
                        trackable.CreatedBy = user;
                        trackable.DateUpdated = now;
                        trackable.UpdatedBy = user;
                        trackable.IsActive = true;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// The DbSet of the User
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// The DbSet of the UserSecret
    /// </summary>
    public DbSet<UserSecret> UserSecrets { get; set; }

    /// <summary>
    /// The DbSet of the UserSession
    /// </summary>
    public DbSet<UserSession> UserSessions { get; set; }

    /// <summary>
    /// The DbSet of the Goal
    /// </summary>
    public DbSet<Goal> Goals { get; set; }

    /// <summary>
    /// The DbSet of the Task
    /// </summary>
    public DbSet<Models.Task> Tasks { get; set; }
}