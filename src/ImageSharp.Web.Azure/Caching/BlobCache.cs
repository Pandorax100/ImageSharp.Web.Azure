using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;
using Pandorax.ImageSharp.Web.Azure.Resolvers;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.Resolvers;

namespace Pandorax.ImageSharp.Web.Azure.Caching
{
    /// <summary>
    /// Implements an image cache using azure blob storage.
    /// </summary>
    public class BlobCache : IImageCache
    {
        private readonly BlobCacheOptions _options;
        private readonly CloudBlobContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobCache"/> class.
        /// </summary>
        /// <param name="options">The blob cache configuration options.</param>
        public BlobCache(IOptions<BlobCacheOptions> options)
        {
            if (options == null)
            {
                throw new System.ArgumentNullException(nameof(options));
            }

            _options = options.Value;

            _container = CloudStorageAccount.Parse(_options.ConnectionString)
                .CreateCloudBlobClient()
                .GetContainerReference(_options.ContainerName);
        }

        /// <inheritdoc/>
        public IDictionary<string, string> Settings { get; set; }

        /// <inheritdoc/>
        public async Task<IImageResolver> GetAsync(string key)
        {
            var blob = _container.GetBlobReference(key);

            if (!await blob.ExistsAsync().ConfigureAwait(false))
            {
                return null;
            }

            return new BlobStorageImageResolver(blob);
        }

        /// <inheritdoc/>
        public async Task SetAsync(string key, Stream stream, ImageMetaData metadata)
        {
            var blob = _container.GetBlockBlobReference(key);

            await blob.UploadFromStreamAsync(stream).ConfigureAwait(false);

            await blob.SetStandardBlobTierAsync(StandardBlobTier.Hot).ConfigureAwait(false);

            blob.Properties.ContentType = metadata.ContentType;

            blob.Properties.CacheControl = $"max-age={(int)metadata.CacheControlMaxAge.TotalSeconds}";

            await blob.SetPropertiesAsync().ConfigureAwait(false);
        }
    }
}
