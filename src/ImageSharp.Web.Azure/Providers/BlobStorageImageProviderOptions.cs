namespace Pandorax.ImageSharp.Web.Azure.Providers
{
    /// <summary>
    /// Configuration options for the Azure Blob Storage provider.
    /// </summary>
    public class BlobStorageImageProviderOptions
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
