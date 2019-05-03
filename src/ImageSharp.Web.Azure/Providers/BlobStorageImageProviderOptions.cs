namespace Pandorax.ImageSharp.Web.Azure.Providers
{
    /// <summary>
    /// Configuration options for the Azure Blob Storage provider.
    /// </summary>
    public class BlobStorageImageProviderOptions
    {
        private string _urlPrefix;

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the container name.
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// Gets or sets the url prefix used to match this provider.
        /// </summary>
        /// <remarks>
        /// The leading slash is removed from any value entered.
        /// </remarks>
        /// <example>
        /// "blobs/"
        /// </example>
        public string UrlPrefix
        {
            get => _urlPrefix;
            set => _urlPrefix = value?.TrimStart('\\', '/');
        }
    }
}
