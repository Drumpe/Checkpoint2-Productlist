public class Product
{
    public string Category { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Product(string category, string name, decimal price)
    {
        Category = category;
        Name = name;
        Price = price;
    }
}
