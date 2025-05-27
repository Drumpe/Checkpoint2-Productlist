namespace Checkpoint2Productlist
{
	public class Menu
	{
		public static void Show()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("\n--- Product List Menu ---");
			Console.ResetColor();
			Console.WriteLine("A - Add Product");
			Console.WriteLine("P - List Products");
			Console.WriteLine("H - Highlight Product");
			Console.WriteLine("F - Find Product");
			Console.WriteLine("E - Edit Product");
			Console.WriteLine("S - Save");
			Console.WriteLine("L - Load");
			Console.WriteLine("Q - Quit");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("Choose an option: ");
			Console.ResetColor();
		}

		public static char GetChoice()
		{
			string? input = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(input))
				return '\0';
			return char.ToUpper(input.Trim()[0]);
		}
	}

	public static class Program
	{
		private const string ProductsFileName = "products.json";

		static void Main()
		{
			var productList = new ProductList();
			bool running = true;

			while (running)
			{
				Menu.Show();
				switch (Menu.GetChoice())
				{
					case 'A':
						AddProduct(productList);
						break;
					case 'P':
						productList.DisplayProducts();
						break;
					case 'H':
						HighlightProduct(productList);
						break;
					case 'F':
						FindProduct(productList);
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

		static void HighlightProduct(ProductList productList)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("--- Highlight Product ---");
			Console.ResetColor();
			Console.Write("Enter a name or category: ");
			string term = Console.ReadLine() ?? "";
			var found = productList.Search(term);

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

		static void FindProduct(ProductList productList)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("--- Find Product ---");
			Console.ResetColor();
			Console.Write("Enter a name or category: ");
			string term = Console.ReadLine() ?? "";
			var found = productList.Search(term);

			if (found.Count == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("No products found.");
				Console.ResetColor();
			}
			else
			{
				productList.DisplayProduct(found);
			}
		}

		static void EditProduct(ProductList productList)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("--- Edit Product ---");
			Console.ResetColor();
			Console.Write("Enter product name to edit: ");
			string name = Console.ReadLine() ?? "";
			var found = productList.SearchByNameOrCategory(name);
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
}