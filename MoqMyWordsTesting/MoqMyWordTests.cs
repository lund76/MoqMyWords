using Moq;
using Xunit.Abstractions;

namespace MoqMyWordsTesting
{
    public class MoqMyWordTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public MoqMyWordTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

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

            //For the sake of the argument a silly way to evaluate any price less than 4 is valid
            //This is only to make sure the test fails.
            service.Verify(u => u.UpdateAsync(It.Is<Product>(p => EvaluatePrice(p))),Times.Once);

            await Task.CompletedTask;
        }   

        private bool EvaluatePrice(Product product)
        {
            _outputHelper.WriteLine($"product price is {product.Price}");
            return product.Price < 4;
        }
    }
}