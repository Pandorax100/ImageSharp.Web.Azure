using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Pandorax.ImageSharp.Web.Azure.Resolvers;
using SixLabors.ImageSharp.Web.Helpers;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.ImageSharp.Web.Resolvers;

namespace Pandorax.ImageSharp.Web.Azure.Providers
{
    /// <summary>
    /// Returns images stored in Azure Blob Storage.
    /// </summary>
    public class BlobStorageImageProvider : IImageProvider
    {
        /// <summary>
        /// The container in the blob service.
        /// </summary>
        private readonly BlobContainerClient _container;

        /// <summary>
        /// The blob storage options.
        /// </summary>
        private readonly BlobStorageImageProviderOptions _storageOptions;

        /// <summary>
        /// Contains various helper methods based on the current configuration.
        /// </summary>
        private readonly FormatUtilities _formatUtilities;

        private Func<HttpContext, bool> _match;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageImageProvider"/> class.
        /// </summary>
        /// <param name="options">The middleware configuration options.</param>
        /// <param name="storageOptions">The blob storage options.</param>
        public BlobStorageImageProvider(IOptions<ImageSharpMiddlewareOptions> options, IOptions<BlobStorageImageProviderOptions> storageOptions)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (storageOptions == null)
            {
                throw new ArgumentNullException(nameof(storageOptions));
            }

            _storageOptions = storageOptions.Value;
            _formatUtilities = new FormatUtilities(options.Value.Configuration);

            _container = new BlobContainerClient(
                _storageOptions.ConnectionString,
                _storageOptions.ContainerName);
        }

        /// <inheritdoc/>
        public Func<HttpContext, bool> Match
        {
            get => _match ?? IsMatch;
            set => _match = value;
        }

        /// <inheritdoc/>
        public IDictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public bool IsValidRequest(HttpContext context)
        {
            return _formatUtilities.GetExtensionFromUri(context.Request.GetDisplayUrl()) != null;
        }

        /// <inheritdoc/>
        public async Task<IImageResolver> GetAsync(HttpContext context)
        {
            string blobName = GetBlobName(context);

            if (string.IsNullOrWhiteSpace(blobName))
            {
                return null;
            }

            BlobClient blob = _container.GetBlobClient(blobName);
            if (!await blob.ExistsAsync().ConfigureAwait(false))
            {
                return null;
            }

            return new BlobStorageImageResolver(blob);
        }

        private string GetBlobName(HttpContext context)
        {
            var path = context.Request.Path.Value.TrimStart('\\', '/');

            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            if (_storageOptions.UrlPrefix == null)
            {
                return path;
            }

            // We have already matched with the prefix so we know
            // that the path starts with the prefix
            return path.Substring(_storageOptions.UrlPrefix.Length);
        }

        private bool IsMatch(HttpContext context)
        {
            if (_storageOptions.UrlPrefix == null)
            {
                return true;
            }

            var path = context.Request.Path.Value.TrimStart('\\', '/');

            return path.StartsWith(_storageOptions.UrlPrefix);
        }
    }
}
