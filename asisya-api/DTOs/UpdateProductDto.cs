public class UpdateProductDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int UnitsInStock { get; set; }
    public bool Discontinued { get; set; }
    public int CategoryId { get; set; }
}
    