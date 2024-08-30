using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using Blazored.LocalStorage;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddServerSideBlazor().AddHubOptions(config => config.MaximumReceiveMessageSize = 1048576);
builder.Services.AddMudServices();
//builder.Services.AddMudPopoverService();
builder.Services.AddSingleton<Mas.Infrastructure.Common.ConnectionManager>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<Allegiance.Blazor.Highcharts.Core.Services.Interfaces.IChartService, Allegiance.Blazor.Highcharts.Core.Services.ChartService>();
//builder.Services.AddTransient<MonicaBlazorUI.Services.MonicaIO>();
//builder.Services.AddTransient<MonicaBlazorUI.Services.RunMonica>();
//builder.Services.AddBlazorGoogleMaps(Environment.GetEnvironmentVariable("GOOGLE_MAPS_API_KEY") ?? "");
//builder.Services.AddBlazorGoogleMaps(new MapApiLoadOptions(Environment.GetEnvironmentVariable("GOOGLE_MAPS_API_KEY") ?? "")
//{
//  Version = "beta", //"3"
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
