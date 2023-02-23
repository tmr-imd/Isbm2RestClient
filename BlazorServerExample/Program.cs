using BlazorServerExample.Data;
using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<StructureAssetService>();

// Example of how you would use Microsoft DI with IsbmClient
builder.Services.Configure<ClientConfig>(builder.Configuration.GetSection("Isbm"));
builder.Services.AddScoped<IChannelManagement, RestChannelManagement>();

builder.Services.AddScoped<StructureAssetService>();

// Once the client grows, we could create a single helper method like this:
//builder.Services.Configure<ClientConfig>(builder.Configuration.GetSection("Isbm"));
//builder.Service.AddIsbmClient();

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
