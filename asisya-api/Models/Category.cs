namespace asisya_api.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string PhotoUrl { get; set; }

    public List<Product>? Products { get; set; }
}