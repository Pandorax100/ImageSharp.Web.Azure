using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Resolvers;

namespace Pandorax.ImageSharp.Web.Azure.Resolvers
{
    /// <summary>
    /// Provides means to manage image buffers within the Azure Blob file system.
    /// </summary>
    public class BlobStorageImageResolver : IImageResolver
    {
        /// <summary>
        /// The Azure blob.
        /// </summary>
        private readonly BlobClient _blob;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageImageResolver"/> class.
        /// </summary>
        /// <param name="blob">The Azure blob.</param>
        public BlobStorageImageResolver(BlobClient blob) => _blob = blob;

        /// <inheritdoc/>
        public async Task<ImageMetaData> GetMetaDataAsync()
        {
            Response<BlobProperties> properties = await _blob.GetPropertiesAsync().ConfigureAwait(false);

            return new ImageMetaData(
                properties.Value?.LastModified.UtcDateTime ?? DateTime.UtcNow,
                properties.Value.ContentType);
        }

        /// <inheritdoc/>
        public async Task<Stream> OpenReadAsync()
        {
            // There are errors that occur consuming the blob stream
            // directly. This is a memory intensive hack to prevent that.
            var outStream = new MemoryStream();
            await _blob.DownloadToAsync(outStream);

            outStream.Position = 0;

            return outStream;
        }
    }
}
