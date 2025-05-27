public class ProductList
{
	private List<Product> products = [];
	public void AddProduct(Product product)
	{
		products.Add(product);
	}
	public void DisplayProducts(IEnumerable<Product>? highlight = null)
	{
		Console.WriteLine();
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
		int priceWidth = Math.Max(10, sortedProducts.Max(p => p.Price.ToString("C").Length));

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
			{
				Console.ForegroundColor = ConsoleColor.Black;
				Console.BackgroundColor = ConsoleColor.Yellow;
			}
			Console.Write(
				product.Category.PadRight(catWidth) + " | " +
				product.Name.PadRight(nameWidth) + " | " +
				product.Price.ToString("C").PadLeft(priceWidth)
			);

			if (isHighlighted)
				Console.ResetColor();
			Console.WriteLine();

			total += product.Price;
		}

		// Print total price
		Console.WriteLine(new string('-', catWidth + nameWidth + priceWidth + 6));
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("Total price: ".PadLeft(catWidth + nameWidth + 3) + total.ToString("C").PadLeft(priceWidth));
		Console.ResetColor();
	}

	public void DisplayProduct(IEnumerable<Product>? products = null)
	{
		if (products == null || !products.Any())
		{
			Console.WriteLine("Product not found.");
			return;
		}
		Console.WriteLine();
		foreach (var product in products)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			//Console.BackgroundColor = ConsoleColor.Yellow;
			Console.Write(product.ToString());
			Console.ResetColor();
			Console.WriteLine();
		}
	}

	public List<Product> Search(string term)
	{
		return products.Where(p => 
		p.Name.Contains(term, StringComparison.CurrentCultureIgnoreCase) || 
		p.Category.Contains(term, StringComparison.CurrentCultureIgnoreCase)).ToList();
	}

	public Product? SearchByName(string name)
	{
		return products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
	}
	
	public Product? SearchByCategory(string cat)
	{
		return products.FirstOrDefault(p => p.Category.Equals(cat, StringComparison.OrdinalIgnoreCase));
	}
	
	public Product? SearchByNameOrCategory(string term)
	{
		return products.FirstOrDefault(p =>
			p.Name.Equals(term, StringComparison.OrdinalIgnoreCase) ||
			p.Category.Equals(term, StringComparison.OrdinalIgnoreCase));
	}
	
	public List<Product> GetAll() => new List<Product>(products);

	public void SetAll(List<Product> newProducts)
	{
		products = newProducts ?? new List<Product>();
	}
}
