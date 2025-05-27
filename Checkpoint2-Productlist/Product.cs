namespace Checkpoint2Productlist
{
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
        public override string ToString()
        {
            return $"Category: {Category}, Name: {Name}, Price: {Price:C}";
        }
    }
}