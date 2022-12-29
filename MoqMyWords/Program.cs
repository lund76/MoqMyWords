{
}

public class PriceService
{
    private readonly IInventoryService _inventoryService;

    public PriceService(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;

    }

    public async Task<Product> UpdateOnlyNewestPrice(List<Product> products)
    {
        foreach (var p in products.OrderByDescending(o => o.Created).Take(1))
        {
            await _inventoryService.UpdateAsync(p);
        }

        return products.FirstOrDefault() ?? new Product();
    }

}

public interface IInventoryService
{
    public Task<DateTime> UpdateAsync(Product product);
}

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public DateTime Created { get; set; }
}

