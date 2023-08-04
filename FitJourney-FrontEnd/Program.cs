using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FitJourney_FrontEnd;
using Index = FitJourney_FrontEnd.Pages.Index;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Index>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();