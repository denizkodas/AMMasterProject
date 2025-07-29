using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AMMasterProject.Helpers
{
    public class MyScheduledTask : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;

        public MyScheduledTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create and start the timer
            _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));

            return Task.CompletedTask;
        }

        private void ExecuteTask(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var notificationHelper = scope.ServiceProvider.GetRequiredService<NotificationHelper>();

                try
                {
                    notificationHelper.PendingNotifications();
                }
                catch (Exception ex)
                {
                    // Log the error

                    string logMessage = ex.Message + " - " + DateTime.Now;

                    // Determine the path to the log file
                    string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "schedulerlog.txt");

                    // Check if the file exists
                    if (!File.Exists(logFilePath))
                    {
                        // Create the file if it doesn't exist
                        using (StreamWriter fileStream = File.CreateText(logFilePath))
                        {
                            // Write the log message to the file
                            fileStream.WriteLine(logMessage);
                        }
                    }
                    else
                    {
                        // Append the log message to the existing file
                        File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop and dispose the timer
            _timer?.Change(Timeout.Infinite, 0);
            _timer?.Dispose();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
