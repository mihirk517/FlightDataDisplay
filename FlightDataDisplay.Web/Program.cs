using FlightDataDisplay.Web.Components;
using FlightDataDisplay.Application;
using FlightDataDisplay.Infrastructure;
using FlightDataDisplay.Domain;
using FlightDataDisplay.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// ⭐ Register ArrivalsMonitor and subscribe it to BaggageHandler
builder.Services.AddSingleton<ArrivalsMonitor>(name => new ArrivalsMonitor("Security Exit"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
