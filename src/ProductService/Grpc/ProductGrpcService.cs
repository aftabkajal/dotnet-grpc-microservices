using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProductService.Models;
using ProductService.Protos;
using ProductService.Services;

namespace ProductService.Grpc;

public class ProductGrpcService : Protos.ProductService.ProductServiceBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductGrpcService> _logger;

    public ProductGrpcService(IProductService productService, IMapper mapper, ILogger<ProductGrpcService> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<ProductModel> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        var product = await _productService.GetProductByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID={request.ProductId} not found."));
        }

        return _mapper.Map<ProductModel>(product);
    }

    public override async Task GetAllProducts(GetAllProductsRequest request,
                                              IServerStreamWriter<ProductModel> responseStream,
                                              ServerCallContext context)
    {
        var products = await _productService.GetAllProductsAsync();
        foreach (var product in products)
        {
            var productModel = _mapper.Map<ProductModel>(product);
            await responseStream.WriteAsync(productModel);
        }
    }

    public override async Task<ProductModel> AddProduct(AddProductRequest request, ServerCallContext context)
    {
        var product = _mapper.Map<Product>(request.Product);
        var addedProduct = await _productService.AddProductAsync(product);

        _logger.LogInformation("Added Product: {Id}_{Name}", addedProduct.ProductId, addedProduct.Name);

        return _mapper.Map<ProductModel>(addedProduct);
    }

    public override async Task<ProductModel> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
    {
        var product = _mapper.Map<Product>(request.Product);

        var exists = await _productService.ProductExistsAsync(product.ProductId);
        if (!exists)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID={product.ProductId} not found."));
        }

        var updatedProduct = await _productService.UpdateProductAsync(product);
        return _mapper.Map<ProductModel>(updatedProduct);
    }

    public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
    {
        var success = await _productService.DeleteProductAsync(request.ProductId);

        return new DeleteProductResponse
        {
            Success = success
        };
    }

    public override async Task<InsertBulkProductResponse> InsertBulkProduct(IAsyncStreamReader<ProductModel> requestStream, ServerCallContext context)
    {
        var products = new List<Product>();

        await foreach (var item in requestStream.ReadAllAsync())
        {
            products.Add(_mapper.Map<Product>(item));
        }

        var insertedCount = await _productService.InsertBulkProductsAsync(products);

        return new InsertBulkProductResponse
        {
            Success = insertedCount > 0,
            InsertCount = insertedCount
        };
    }

    public override Task<Empty> Test(Empty request, ServerCallContext context)
    {
        _logger.LogInformation("Test method called.");
        return Task.FromResult(new Empty());
    }
}
