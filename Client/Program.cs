using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BlazorChat.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorChat.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddAntDesign();
            builder.Services.AddTransient<IProfileViewModel, ProfileViewModel>();
            builder.Services.AddTransient<ISettingsViewModel, SettingsViewModel>();
            builder.Services.AddTransient<IContactsViewModel, ContactsViewModel>();
            builder.Services.AddTransient<ILoginViewModel, LoginViewModel>();
            builder.Services.AddTransient<IRegisterViewModel, RegisterViewModel>();
            builder.Services.AddTransient<IChatPersonViewModel, ChatPersonViewModel>();

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
