using Moq;

namespace MoqMyWordsTesting
{
    public class MoqMyWordTests
    {
        [Fact]
        public async Task MoqMyWord_UpdateService_OnlyCalledOnce()
        {

            var productList = new List<Product>
            {
                new() { Price = 1, Created = DateTime.UtcNow.AddDays(-1) },
                new() { Price = 2, Created = DateTime.UtcNow.AddDays(-2) },
                new() { Price = 3, Created = DateTime.UtcNow.AddDays(-3) }
            };

            Mock<IInventoryService> service = new Mock<IInventoryService>();

            var priceService = new PriceService(service.Object);

            await priceService.UpdateOnlyNewestPrice(productList);

            service.Verify(u => u.UpdateAsync(It.Is<Product>(p => EvaluatePrice(p))),Times.Once);

            await Task.CompletedTask;
        }   

        private bool EvaluatePrice(Product product)
        {
            return product.Price == 1;
        }
    }
}