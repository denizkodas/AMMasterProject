using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AMMasterProject;
using AMMasterProject.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using AMMasterProject.ViewModel;
using Microsoft.Extensions.Localization;
using AMMasterProject.Helpers; 
using Microsoft.AspNetCore.Authentication.Facebook;
using Google.Cloud.Storage.V1;
using Stripe;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Google;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;


//using Microsoft.AspNetCore.Authentication.Google;


var builder = WebApplication.CreateBuilder(args);

// Add the middleware to redirect www to non-www
builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add(new RedirectToNonWwwFilter());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAnyOriginPolicy",
                      policy =>
                      {
                          policy.AllowAnyOrigin()  // Allow any origin
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "AllowAMTechOriginPolicy",
//                      policy =>
//                      {
//                          policy.WithOrigins("https://localhost:44344",
//                                              "https://localhost:44303",
//                                               "https://localhost:7057",
//                                              "https://amtechnology.info"


//                                              );
//                      });
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "AllowAMTechOriginPolicy",
//                      policy =>
//                      {
//                          policy.WithOrigins("https://localhost:44341",
//                                              "https://localhost:44301",
//                                              "https://localhost:7051",
//                                              "https://amtechnology.info")
//                                .AllowAnyHeader()
//                                .AllowAnyMethod();
//                      });
//});

// Get the web root path from the hosting environment
string webRootPath = builder.Environment.WebRootPath;

// Specify the log folder path within the wwwroot folder
string logFolderPath = Path.Combine(webRootPath, "log");

// Check if the folder exists, create it if not
if (!Directory.Exists(logFolderPath))
{
    Directory.CreateDirectory(logFolderPath);
}

// Configure Serilog
// Configure Serilog with additional details
// Configure Serilog with additional details
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        Path.Combine(logFolderPath, "log-.txt"),
        rollingInterval: RollingInterval.Day,
        outputTemplate: $"{Environment.NewLine}------------------ Log start {{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}} ----------------{Environment.NewLine}[{{Level:u3}}] {{Message:lj}}{{NewLine}}{{Exception}}{{NewLine}}------------------ Log end {{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}}  ------------{Environment.NewLine}")
    .CreateLogger();




builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();


///configuration json files
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//builder.Configuration.AddJsonFile("websettings.json", optional: true, reloadOnChange: true);



builder.Services.AddSingleton<IConfiguration>(builder.Configuration);




//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("EzyCommerceLive")));





// Add the middleware to the pipeline
builder.Services.AddTransient<GlobalMiddleware>();
builder.Services.AddScoped<ProductHelper>();
builder.Services.AddScoped<UserHelper>();
builder.Services.AddScoped<EncryptionHelper>();
builder.Services.AddScoped<InboxHelper>();
builder.Services.AddScoped<NotificationHelper>();
builder.Services.AddScoped<MembershipHelper>();
builder.Services.AddScoped<WebsettingHelper>();
builder.Services.AddScoped<PaymentGatewayHelper>();
builder.Services.AddScoped<OrderHelper>();
builder.Services.AddScoped<GlobalHelper>();




//builder.Services.AddHostedService<SchedulerHelper>();
//builder.Services.AddSingleton<ILicenseValidator, LicenseValidatorImplementation>();
//builder.Services.AddSingleton<IUserVerificationValidator, UserVerificationImplementation>();

builder.Services.AddScoped<FileUploadHelper>();

//hosted service
//builder.Services.AddHostedService<MyScheduledTask>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.Cookie.Name = "UserCookie";
        options.LoginPath = "/login/index";
        options.LogoutPath = "/index?status=logout";
        options.AccessDeniedPath = "/controller/login/AccessDenied";



    });











builder.Services.AddHttpContextAccessor();

// Add the authorization policy for listings
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ListingPolicy", policy =>
//    {
//        policy.RequireClaim("UserType", "Vendor");
//        policy.RequireClaim("UserType", "Admin");
//        policy.RequireClaim("UserType", "Sub Admin");
//    });
//});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ListingPolicy", policy => policy.RequireClaim("UserType"));
//});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ListingPolicy", policy =>
                      policy.RequireClaim("UserType", "Admin", "Vendor"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllUsers", policy =>
                      policy.RequireClaim("UserType", "Admin", "Vendor", "Client"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SellerAdmin", policy =>
                      policy.RequireClaim("UserType", "Admin", "Vendor"));
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Seller", policy =>
                      policy.RequireClaim("UserType",  "Vendor"));
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
                      policy.RequireClaim("UserType", "Admin"));
});

//[Authorize(Policy = "Revenue")]
///Admin Modulars
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Setup", policy =>
                      policy.RequireClaim("UserType", "Admin"));
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Revenue", policy =>
                      policy.RequireClaim("UserType", "Admin"));
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Community", policy =>
                      policy.RequireClaim("UserType", "Admin"));
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Support", policy =>
                      policy.RequireClaim("UserType", "Admin"));
});





//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("UserType", policy => policy.RequireClaim("Admin"));
//});





///External Login
///

builder.Services.AddAuthentication()
     .AddGoogle(options =>
     {
         options.ClientId = "setting from login controller";
         options.ClientSecret = "setting from login controller";
     });

builder.Services.AddAuthentication()
      .AddFacebook(options =>
      {

          options.AppId = "setting from login controller";
          options.AppSecret = "setting from login controller";
      });



builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseStatusCodePagesWithReExecute("/Error");
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();


app.UseAuthentication();
//app.UseAuthorization();

// Get the service provider from the app's services
var serviceProvider = app.Services;
var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

// Sign in the user if their identity is authenticated
if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
{
    var identity = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;
    await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
}



app.UseCors("AllowAMTechOriginPolicy");
app.UseAuthorization();

// Use the middleware in the request pipeline
app.UseMiddleware<GlobalMiddleware>();

app.MapRazorPages();
app.MapControllers();


// Separate method for application configuration


app.Run();


// Custom middleware to redirect www to non-www
public class RedirectToNonWwwFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var host = context.HttpContext.Request.Host;
        if (host.Host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
        {
            var newHost = new HostString(host.Host.Substring(4));
            var newUrl = UriHelper.BuildAbsolute(context.HttpContext.Request.Scheme, newHost, context.HttpContext.Request.PathBase, context.HttpContext.Request.Path, context.HttpContext.Request.QueryString);
            context.Result = new RedirectResult(newUrl, true);
        }
    }
}