﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace AAK.FilArkiv;

public record FilArkivOptions(string BaseAddress, string ClientId, string ClientSecret, string AuthenticationBaseUrl);

public static class RegisterFilArkiv
{
    public static IServiceCollection AddFilArkiv(this IServiceCollection services, FilArkivOptions options)
    {
        const string CLIENT_NAME = "FilArkivHttpClient";

        if (string.IsNullOrWhiteSpace(options.BaseAddress)) throw new ArgumentNullException(nameof(options.BaseAddress));
        if (string.IsNullOrWhiteSpace(options.ClientId)) throw new ArgumentNullException(nameof(options.ClientId));
        if (string.IsNullOrWhiteSpace(options.ClientSecret)) throw new ArgumentNullException(nameof(options.ClientSecret));
        if (string.IsNullOrWhiteSpace(options.AuthenticationBaseUrl)) throw new ArgumentNullException(nameof(options.AuthenticationBaseUrl));

        services.AddHttpClient(CLIENT_NAME, client =>
        {
            client.BaseAddress = new Uri(options.BaseAddress);
        });

        services.AddSingleton(serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient(CLIENT_NAME);

            return new AuthenticationService(client, options.AuthenticationBaseUrl, options.ClientId, options.ClientSecret);
        });

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
