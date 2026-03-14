namespace asisya_api.DTOs;

public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int UnitsInStock { get; set; }
    public bool Discontinued { get; set; }
    public CategoryDto Category { get; set; } = null!;
}
