using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using DemoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Override AzureAd config from environment variables (flat structure)
builder.Configuration["AzureAd:TenantId"] = Environment.GetEnvironmentVariable("AZURE_AD_TENANT_ID") ?? "common";
builder.Configuration["AzureAd:ClientId"] = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_ID") ?? "";
builder.Configuration["AzureAd:ClientSecret"] = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_SECRET") ?? "";
builder.Configuration["AzureAd:Instance"] = Environment.GetEnvironmentVariable("AZURE_AD_INSTANCE") ?? "https://login.microsoftonline.com/";

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = null;
});

// Register ACS services
var acsConnectionString = Environment.GetEnvironmentVariable("ACS_CONNECTION_STRING") ?? "";
var acsPhoneNumber = Environment.GetEnvironmentVariable("ACS_PHONE_NUMBER") ?? "";

builder.Services.AddSingleton<IIdentityService>(sp => new IdentityService(acsConnectionString));
builder.Services.AddSingleton<IChatService>(sp => new ChatService(acsConnectionString));
builder.Services.AddSingleton<IEmailService>(sp => new EmailService(acsConnectionString));
builder.Services.AddSingleton<ISmsService>(sp => new SmsService(acsConnectionString, acsPhoneNumber));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
