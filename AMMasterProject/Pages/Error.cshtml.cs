using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using System.Diagnostics;

namespace AMMasterProject.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]


    //return RedirectToPage("/Error", new { Title = "Access Denied", Message="You do not have permission to access this feature" });
public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public string? Title { get; set; }

        public string? Message { get; set; }

        public string? Body { get; set; }


        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        

        public string? ExceptionMessage { get; set; }

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

           // var exceptionHandlerPathFeature =
           //HttpContext.Features.Get<IExceptionHandlerPathFeature>();

           // if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
           // {
           //     ExceptionMessage = "The file was not found.";
           // }

           // if (exceptionHandlerPathFeature?.Path == "/")
           // {
           //     ExceptionMessage ??= string.Empty;
           //     ExceptionMessage += " Page: Home.";
           // }


            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            
            if (exceptionHandlerPathFeature != null)
            {
                // Get the exception and path details
                var error = exceptionHandlerPathFeature.Error;
                var path = exceptionHandlerPathFeature.Path;

                // Construct an error message with the exception and path details
                ExceptionMessage = $"An error occurred on the path: {path}. Error details: {error}";


                Log.Error(ExceptionMessage, "Error Page");
            }
            if (!Request.Query.ContainsKey("Title"))
            {
                Title = "404";
                Message = "Page Not Found";
                Body = "Resource you are looking for not existing or permanantly removed";
                // Handle the case when exceptionHandlerPathFeature is not available
                //ExceptionMessage = "An unknown error occurred.";

                ExceptionMessage = $"An error occurred on the Title: {Title}. Message: {Message} . Body: {Body}";

                Log.Error(ExceptionMessage, "Error ");

                string CustomExceptionMessage = $"An error occurred on the Title: {Title}. Message: {Message} . Body: {Body}";

                Log.Error(CustomExceptionMessage, "Error ");
            }

            if (Request.Query.ContainsKey("Title"))
            {
                Title = Request.Query["Title"].ToString();
            }

            if (Request.Query.ContainsKey("Message"))
            {
                Message = Request.Query["Message"].ToString();
            }

            if (Request.Query.ContainsKey("Body"))
            {
                Body = Request.Query["Body"].ToString();
            }


          
        }
    }
}