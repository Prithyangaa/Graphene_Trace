using GrapheneTrace.Data;
using GrapheneTrace.Model;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Services
{
    public class AlertGeneratorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AlertGeneratorService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var latest = db.SensorReadings
                    .GroupBy(r => r.PatientId)
                    .Select(g => g.OrderByDescending(r => r.Timestamp).First())
                    .ToList();

                foreach (var r in latest)
                {
                    string? severity =
                        r.Pressure > 85 || r.Temperature > 39 ? "Critical" :
                        r.Pressure > 70 || r.Temperature > 38 ? "High" :
                        r.Pressure > 60 ? "Medium" :
                        null;

                    if (severity == null)
                        continue;

                    bool exists = db.Alerts.Any(a =>
                        a.PatientId == r.PatientId &&
                        a.Severity == severity &&
                        a.CreatedAt > DateTime.UtcNow.AddMinutes(-10));

                    if (!exists)
                    {
                        db.Alerts.Add(new Alert
                        {
                            PatientId = r.PatientId,
                            AlertType = "Pressure/Thermal Risk",
                            Severity = severity,
                            Status = "Open",
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                await db.SaveChangesAsync();
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}

