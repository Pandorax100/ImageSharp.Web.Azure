using System;
using System.Collections.Generic;
using System.Text;

namespace Pandorax.ImageSharp.Web.Azure.Caching
{
    /// <summary>
    /// Configuration options for the Azure Blob Cache.
    /// </summary>
    public class BlobCacheOptions
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the container name.
        /// </summary>
        public string ContainerName { get; set; }
    }
}
