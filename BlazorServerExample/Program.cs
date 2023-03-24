using BlazorServerExample.Data;
using BlazorServerExample.Extensions;
using BlazorServerExample.Pages;
using Isbm2Client.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<StructureAssetService>();

var clientConfig = builder.Configuration.GetSection("Isbm");
builder.Services.Configure<ClientConfig>(clientConfig);

builder.Services.AddIsbmRestClient( clientConfig.Get<ClientConfig>() );

builder.Services.AddScoped<StructureAssetService>();

builder.Services.AddScoped<RequestViewModel>();
builder.Services.AddScoped<PublicationViewModel>();


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
