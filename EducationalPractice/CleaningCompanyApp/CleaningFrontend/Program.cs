using System;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Blazored.LocalStorage;
using CleaningFrontend;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5004/") });
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<CleaningFrontend.ApiRequests.Services.UserApiService>();
builder.Services.AddScoped<CleaningFrontend.ApiRequests.Services.OrderApiService>();
builder.Services.AddScoped<CleaningFrontend.ApiRequests.Services.BrigadeApiService>();
builder.Services.AddScoped<CleaningFrontend.ApiRequests.Services.MaterialApiService>();
builder.Services.AddScoped<CleaningFrontend.ApiRequests.Services.ObjectApiService>();
builder.Services.AddScoped<CleaningFrontend.ApiRequests.Services.SupervisorApiService>();

await builder.Build().RunAsync();
