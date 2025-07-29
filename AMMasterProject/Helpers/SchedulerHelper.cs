using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace AMMasterProject.Helpers
{
    #region Scheduler

   
    public class SchedulerHelper : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;
      
        private Timer _licenseCheckTimer;
        private Timer _userVerificationTimer;
      

      
        public SchedulerHelper(IServiceProvider services)
        {
            _services = services;
           
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Run the license check every 10 days (adjust the interval as needed)
            //_licenseCheckTimer = new Timer(DoLicenseCheck, null, TimeSpan.Zero, TimeSpan.FromDays(10));

            // Run the user verification every 2 minutes (adjust the interval as needed)
            //_userVerificationTimer = new Timer(DoUserVerification, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));

            return Task.CompletedTask;
        }


        private void SimulateError()
        {
            throw new InvalidOperationException("This is a simulated error for testing purposes.");
        }
        private void DoLicenseCheck(object state)
         {
            try
            {
                using (var scope = _services.CreateScope())
                {
                    var licenseValidator = scope.ServiceProvider.GetRequiredService<ILicenseValidator>();
                    bool islicensevalid = false;

                    if (!islicensevalid)
                    {
                        // Log the license validation error
                        //Log.Error("License validation failed: {ErrorMessage}", validationResult.ErrorMessage);

                        // Redirect the user to the "/license" page
                        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                        var context = httpContextAccessor.HttpContext;

                        if (context != null)
                        {
                            context.Response.Redirect("/license"); // Adjust the URL accordingly
                            return; // Stop further processing
                        }
                    }

                    // Continue with other logic if the license is valid
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your application's error-handling strategy
                Log.Error(ex, "Error in license check");
            }
        }


        private void DoUserVerification(object state)
        {
            try
            {
                using (var scope = _services.CreateScope())
                {
                    // Perform user verification logic here
                    var userVerificationService = scope.ServiceProvider.GetRequiredService<IUserVerificationValidator>();
                    userVerificationService.VerifyUsers();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your application's error-handling strategy
                Console.WriteLine($"Error in User Verification: {ex.Message}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _licenseCheckTimer?.Change(Timeout.Infinite, 0);
            _userVerificationTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _licenseCheckTimer?.Dispose();
            _userVerificationTimer?.Dispose();
        }
    }
    #endregion


    #region License

    
   
    public interface IUserVerificationValidator
    {
        void VerifyUsers();
    }

    public interface ILicenseValidator
    {
        void CheckLicense();
    }
    #endregion

    #region UserVerification

   
    public class UserVerificationImplementation : IUserVerificationValidator
    {
        public void VerifyUsers()
        {
            try
            {
                // Your user verification logic here
                // For example, check and update user statuses
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your application's error-handling strategy
                Console.WriteLine($"Error in User Verification Logic: {ex.Message}");
            }
        }
    }

    public class LicenseValidatorImplementation : ILicenseValidator
    {
        public void CheckLicense()
        {
            try
            {
                // Your license validation logic here
                // Make API call to license.com to check license status
                // Update your application's internal state based on the license status

                // Assuming the license check fails, return false and throw an exception or set an error message
              
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your application's error-handling strategy
                Console.WriteLine($"Error in License Validation Logic: {ex.Message}");

                // Throw an exception or set an error message to indicate the failure
                throw new InvalidOperationException("License validation failed.", ex);
            }
        }
    }
    #endregion

}