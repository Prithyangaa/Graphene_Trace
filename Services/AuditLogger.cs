using GrapheneTrace.Data;
using GrapheneTrace.Model;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Services
{
    public class AuditLogger
    {
        private readonly ApplicationDbContext _context;

        public AuditLogger(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(int? userId, string actionType, string table, int? recordId, string? details = null)
        {
            var log = new AuditLog
            {
                UserId = userId,
                ActionType = actionType,
                TableName = table,
                RecordId = recordId,
                ActionTimestamp = DateTime.UtcNow,
                Details = details
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}

