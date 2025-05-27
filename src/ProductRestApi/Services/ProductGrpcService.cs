using Grpc.Core;
using Grpc.Net.Client;
using ProductGrpc.Protos;

namespace ProductRestApi.Services
{
    public class ProductsGrpcServiceDriver
    {
        private readonly ProductProtoService.ProductProtoServiceClient _client;

        public ProductsGrpcServiceDriver(IConfiguration config)
        {
            var grpcUrl = config["GrpcSettings:ProductUrl"]!;
            var channel = GrpcChannel.ForAddress(grpcUrl);
            _client = new ProductProtoService.ProductProtoServiceClient(channel);
        }

        public async Task<ProductModel> GetProductAsync(int id)
            => await _client.GetProductAsync(new GetProductRequest { ProductId = id });

        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
            var result = new List<ProductModel>();
            using var call = _client.GetAllProducts(new GetAllProductsRequest());
            await foreach (var response in call.ResponseStream.ReadAllAsync())
            {
                result.Add(response);
            }
            return result;
        }

        public async Task<ProductModel> AddProductAsync(ProductModel model)
        {
            var product = await _client.AddProductAsync(new AddProductRequest { Product = model });
            return product;
        }

        public async Task<ProductModel> UpdateProductAsync(ProductModel model)
        {
            var product = await _client.UpdateProductAsync(new UpdateProductRequest { Product = model });
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
            => (await _client.DeleteProductAsync(new DeleteProductRequest { ProductId = id })).Success;

        public async Task<(bool success, int count)> InsertBulkProductAsync(IEnumerable<ProductModel> products)
        {
            using var call = _client.InsertBulkProduct();
            foreach (var product in products)
            {
                await call.RequestStream.WriteAsync(product);
            }
            await call.RequestStream.CompleteAsync();
            var result = await call;
            return (result.Success, result.InsertCount);
        }
    }
}
