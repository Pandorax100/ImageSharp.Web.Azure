# Pandorax.ImageSharp.Web.Azure

A library extending [ImageSharp.Web](https://github.com/SixLabors/ImageSharp.Web) to use azure blob storage.

## Note
There is already an [open pull request](https://github.com/SixLabors/ImageSharp.Web/pull/49) to add similar functionality to the [main repo](https://github.com/SixLabors/ImageSharp.Web). As functionality is added to ImageSharp.Web, it will be removed from here until this repo becomes obsolete and is deleted.

## Installation

Install the [NuGet package](https://www.nuget.org/packages/Pandorax.ImageSharp.Web.Azure) to your ASP.NET Core Project

```
Install-Package Pandorax.ImageSharp.Web.Azure -Version 1.0.0-alpha4
```

## Usage

### Image Provider

Register the provider via dependency injection

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
using Pandorax.ImageSharp.Web.Azure.DependencyInjection;

...

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

## Using the Cache

First register the cache via dependency injection

Using the ImageSharpCoreBuilder:
```
builder.SetCache<BlobCache>()
```

Using the service collection:
```
services.AddSingleton<IImageCache, BlobCache>()
```

### Configure the Cache

Via an action
```
services.ConfigureBlobCacheOptions(options =>
{
    options.ConnectionString = configuration["ConnectionString"];
    options.ContainerName = configuration["ContainerName"];
});
```

Or via configuration binding
```
services.ConfigureBlobCacheOptions(configuration.GetSection("BlobCacheOptions"));
```
