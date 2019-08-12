using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;
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
        private readonly CloudBlob _blob;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageImageResolver"/> class.
        /// </summary>
        /// <param name="blob">The Azure blob.</param>
        public BlobStorageImageResolver(CloudBlob blob) => _blob = blob;

        /// <inheritdoc/>
        public async Task<ImageMetaData> GetMetaDataAsync()
        {
            await _blob.FetchAttributesAsync().ConfigureAwait(false);

            BlobProperties properties = _blob.Properties;

            return new ImageMetaData(
                properties?.LastModified?.UtcDateTime ?? DateTime.UtcNow,
                properties.ContentType);
        }

        /// <inheritdoc/>
        public async Task<Stream> OpenReadAsync()
        {
            var memStream = new MemoryStream();
            await _blob.DownloadToStreamAsync(memStream);

            memStream.Position = 0;

            return memStream;
        }
    }
}
