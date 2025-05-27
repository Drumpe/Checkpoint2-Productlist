using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

class Product
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

class ProductList
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

class Menu
{
    public void Show()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n--- Product List Menu ---");
        Console.ResetColor();
        Console.WriteLine("A - Add Product");
        Console.WriteLine("P - List Products");
        Console.WriteLine("F - Find Product");
        Console.WriteLine("E - Edit Product");
        Console.WriteLine("S - Save");
        Console.WriteLine("L - Load");
        Console.WriteLine("Q - Quit");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Choose an option: ");
        Console.ResetColor();
    }

    public char GetChoice()
    {
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            return '\0';
        return char.ToUpper(input.Trim()[0]);
    }
}

class Program
{
    private const string ProductsFileName = "products.json";

    static void Main()
    {
        var productList = new ProductList();
        var menu = new Menu();
        bool running = true;

        while (running)
        {
            menu.Show();
            switch (menu.GetChoice())
            {
                case 'A':
                    AddProduct(productList);
                    break;
                case 'P':
                    productList.DisplayProducts();
                    break;
                case 'F':
                    SearchProduct(productList);
                    break;
                case 'E':
                    EditProduct(productList);
                    break;
                case 'S':
                    SaveProducts(productList);
                    break;
                case 'L':
                    LoadProducts(productList);
                    break;
                case 'Q':
                    running = false;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice.");
                    Console.ResetColor();
                    break;
            }
        }
    }

    static void AddProduct(ProductList productList)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--- Add Product ---");
        Console.ResetColor();
        Console.Write("Enter category: ");
        string category = Console.ReadLine() ?? "";
        Console.Write("Enter name: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Enter price: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid price.");
            Console.ResetColor();
            return;
        }
        productList.AddProduct(new Product(category, name, price));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Product added.");
        Console.ResetColor();
    }

    static void SearchProduct(ProductList productList)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--- Find Product ---");
        Console.ResetColor();
        Console.Write("Enter a name or product: ");
        string term = Console.ReadLine() ?? "";
        var found = productList.Search(term);

        // Always display the full list, highlighting found items
        if (productList.GetAll().Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No products to display.");
            Console.ResetColor();
        }
        else
        {
            productList.DisplayProducts(found);
            if (found.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No products found.");
                Console.ResetColor();
            }
        }
    }

    static void EditProduct(ProductList productList)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--- Edit Product ---");
        Console.ResetColor();
        Console.Write("Enter product name to edit: ");
        string name = Console.ReadLine() ?? "";
        var found = productList.SearchByName(name);
        if (found == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Product not found.");
            Console.ResetColor();
            return;
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Editing: " + found);
        Console.ResetColor();
        Console.Write("New category (leave blank to keep): ");
        string category = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(category))
            found.Category = category;
        Console.Write("New name (leave blank to keep): ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName))
            found.Name = newName;
        Console.Write("New price (leave blank to keep): ");
        string priceInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out decimal newPrice))
            found.Price = newPrice;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Product updated.");
        Console.ResetColor();
    }

    static void SaveProducts(ProductList productList)
    {
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(productList.GetAll());
            File.WriteAllText(ProductsFileName, json);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Products saved to {ProductsFileName}.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error saving products: " + ex.Message);
            Console.ResetColor();
        }
    }

    static void LoadProducts(ProductList productList)
    {
        try
        {
            if (!File.Exists(ProductsFileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File not found.");
                Console.ResetColor();
                return;
            }
            var json = File.ReadAllText(ProductsFileName);
            var products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json);
            if (products != null)
            {
                productList.SetAll(products);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Products loaded.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error loading products: " + ex.Message);
            Console.ResetColor();
        }
    }
}