public class ProductList
{
    private List<Product> products = new List<Product>();
    public void AddProduct(Product product)
    {
        products.Add(product);
    }
    public void DisplayProducts(IEnumerable<Product>? highlight = null)
    {
        if (products.Count == 0)
        {
            Console.WriteLine("No products to display.");
            return;
        }

        // Sort products by price (low to high)
        var sortedProducts = products.OrderBy(p => p.Price).ToList();

        // Calculate max widths for each column
        int catWidth = Math.Max(8, sortedProducts.Max(p => p.Category.Length));
        int nameWidth = Math.Max(8, sortedProducts.Max(p => p.Name.Length));
        int priceWidth = 10;

        // Print header
        Console.WriteLine(
            "Category".PadRight(catWidth) + " | " +
            "Name".PadRight(nameWidth) + " | " +
            "Price".PadRight(priceWidth)
        );
        Console.WriteLine(new string('-', catWidth + nameWidth + priceWidth + 6));

        // Print products, highlighting if needed
        decimal total = 0;
        foreach (var product in sortedProducts)
        {
            bool isHighlighted = highlight != null && highlight.Contains(product);
            if (isHighlighted)
                Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine(
                product.Category.PadRight(catWidth) + " | " +
                product.Name.PadRight(nameWidth) + " | " +
                product.Price.ToString("C").PadRight(priceWidth)
            );

            if (isHighlighted)
                Console.ResetColor();

            total += product.Price;
        }

        // Print total price
        Console.WriteLine(new string('-', catWidth + nameWidth + priceWidth + 6));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Total price: ".PadLeft(catWidth + nameWidth + 3) + total.ToString("C").PadLeft(priceWidth));
        Console.ResetColor();
    }


    public List<Product> Search(string term)
    {
        return products.Where(p => p.Name.Contains(term, StringComparison.CurrentCultureIgnoreCase) || p.Category.Contains(term, StringComparison.CurrentCultureIgnoreCase)).ToList();
    }

    public Product? SearchByName(string name)
    {
        return products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public List<Product> GetAll() => new List<Product>(products);

    public void SetAll(List<Product> newProducts)
    {
        products = newProducts ?? new List<Product>();
    }
}
