﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pandorax.ImageSharp.Web.Azure.Caching;
using Pandorax.ImageSharp.Web.Azure.Providers;

namespace Pandorax.ImageSharp.Web.Azure.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding services to a <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceConfigurationExtensions
    {
        /// <summary>
        /// Registers an action used to configure <see cref="BlobStorageImageProviderOptions"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/> so that additional calls can be chained.
        /// </returns>
        public static IServiceCollection ConfigureBlobProviderOptions(this IServiceCollection services, Action<BlobStorageImageProviderOptions> configureOptions)
        {
            services.Configure(configureOptions);

            return services;
        }

        /// <summary>
        /// Registeres a configuration instance which <see cref="BlobStorageImageProviderOptions"/> will bind to.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="configuration">The configuration being bound.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/> so that additional calls can be chained.
        /// </returns>
        public static IServiceCollection ConfigureBlobProviderOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlobStorageImageProviderOptions>(configuration);

            return services;
        }

        /// <summary>
        /// Registers an action used to configure <see cref="BlobCacheOptions"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/> so that additional calls can be chained.
        /// </returns>
        public static IServiceCollection ConfigureBlobCacheOptions(this IServiceCollection services, Action<BlobCacheOptions> configureOptions)
        {
            services.Configure(configureOptions);

            return services;
        }

        /// <summary>
        /// Registeres a configuration instance which <see cref="BlobCacheOptions"/> will bind to.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="configuration">The configuration being bound.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/> so that additional calls can be chained.
        /// </returns>
        public static IServiceCollection ConfigureBlobCacheOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlobCacheOptions>(configuration);

            return services;
        }
    }
}
