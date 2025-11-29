using GrapheneTrace.Data;
using GrapheneTrace.Services;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Services
{
    public class TrackedDbContext : ApplicationDbContext
    {
        private readonly AuditLogger _logger;

        public TrackedDbContext(DbContextOptions<ApplicationDbContext> options, AuditLogger logger)
            : base(options)
        {
            _logger = logger;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            int? currentUser = 1; // You can replace with session later

            foreach (var entry in entries)
            {
                string table = entry.Metadata.GetTableName() ?? "Unknown";
                int? recordId = null;

                var key = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
                if (key != null)
                    recordId = key.CurrentValue as int?;

                string action = entry.State switch
                {
                    EntityState.Added => "INSERT",
                    EntityState.Modified => "UPDATE",
                    EntityState.Deleted => "DELETE",
                    _ => "UNKNOWN"
                };

                await _logger.LogAsync(
                    userId: currentUser,
                    actionType: action,
                    table: table,
                    recordId: recordId,
                    details: entry.ToString()
                );
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

