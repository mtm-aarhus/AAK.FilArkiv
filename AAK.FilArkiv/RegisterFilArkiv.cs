using Microsoft.Extensions.DependencyInjection;

namespace AAK.FilArkiv;

public record FilArkivOptions(string BaseAddress, string ClientId, string ClientSecret, string AuthenticationUrl);

public static class RegisterFilArkiv
{
    public static IServiceCollection AddFilArkiv(this IServiceCollection services, FilArkivOptions options)
    {
        const string CLIENT_NAME = "FilArkivHttpClient";

        if (string.IsNullOrWhiteSpace(options.BaseAddress)) throw new ArgumentNullException(nameof(options.BaseAddress));
        if (string.IsNullOrWhiteSpace(options.ClientId)) throw new ArgumentNullException(nameof(options.ClientId));
        if (string.IsNullOrWhiteSpace(options.ClientSecret)) throw new ArgumentNullException(nameof(options.ClientSecret));
        if (string.IsNullOrWhiteSpace(options.AuthenticationUrl)) throw new ArgumentNullException(nameof(options.AuthenticationUrl));

        services.AddHttpClient(CLIENT_NAME, client =>
        {
            client.BaseAddress = new Uri(options.BaseAddress);
        });

        services.AddSingleton<AuthenticationService>();

        services.AddTransient<IFilArkiv>(serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient(CLIENT_NAME);

            var authenticationService = serviceProvider.GetRequiredService<AuthenticationService>();

            return new FilArkivClient(client, authenticationService);
        });

        return services;
    }
}
