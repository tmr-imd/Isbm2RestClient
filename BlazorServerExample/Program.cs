using BlazorServerExample.Data;
using BlazorServerExample.Pages;
using Isbm2Client.Interface;
using Isbm2Client.Model;
using Isbm2Client.Service;
using Isbm2RestClient.Api;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<StructureAssetService>();

// Example of how you would use Microsoft DI with IsbmClient
builder.Services.Configure<ClientConfig>(builder.Configuration.GetSection("Isbm"));

var endPoint = builder.Configuration.GetSection("Isbm").GetValue<string>("EndPoint");

builder.Services.AddScoped<IConsumerRequestServiceApi>( x => new ConsumerRequestServiceApi(endPoint) );
builder.Services.AddScoped<IProviderRequestServiceApi>( x => new ProviderRequestServiceApi(endPoint) );

builder.Services.AddScoped<IChannelManagement, RestChannelManagement>();
builder.Services.AddScoped<IProviderRequest, RestProviderRequest>();
builder.Services.AddScoped<IConsumerRequest, RestConsumerRequest>();
builder.Services.AddScoped<IProviderPublication, RestProviderPublication>();
builder.Services.AddScoped<IConsumerPublication, RestConsumerPublication>();

builder.Services.AddScoped<StructureAssetService>();

builder.Services.AddScoped<RequestViewModel>();
builder.Services.AddScoped<PublicationViewModel>();

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
