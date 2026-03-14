public class CreateProductDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int UnitsInStock { get; set; }
    public bool Discontinued { get; set; }
    public int CategoryId { get; set; }
}
