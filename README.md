# Pandorax.ImageSharp.Web.Azure

A library extending [ImageSharp.Web](https://github.com/SixLabors/ImageSharp.Web) to use azure blob storage.

## Installation

Install the [NuGet package](https://) to your ASP.NET Core Project

```
Install-Package Pandorax.ImageSharp.Web.Azure -Version 1.0.0-beta1
```

## Usage

### Register the provider via dependency injection

```
services.AddImageSharpCore()
    .SetRequestParser<QueryCollectionRequestParser>()
    .SetMemoryAllocatorFromMiddlewareOptions()
    .SetCache<PhysicalFileSystemCache>()
    .SetCacheHash<CacheHash>()
    .AddProvider<BlobStorageImageProvider>()
    .AddProvider<PhysicalFileSystemProvider>()
    .AddProcessor<ResizeWebProcessor>()
    .AddProcessor<FormatWebProcessor>()
    .AddProcessor<BackgroundColorWebProcessor>();
```

The providers are matched in the order they are registered, so it is important that the BlobStorageImageProvider is registered before the PhysicalFileSystemProvider.

### Configure the Provider

Via an action
```
services.ConfigureBlobProviderOptions(options =>
{
    options.ConnectionString = configuration["ConnectionString"];
    options.ContainerName = configuration["ContainerName"];
    options.UrlPrefix = configuration["UrlPrefix"];
});
```

Or via configuration binding
```
services.ConfigureBlobProviderOptions(configuration.GetSection("BlobProviderOptions"));
```

