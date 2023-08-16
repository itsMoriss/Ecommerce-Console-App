using System;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static async Task Main(string[] args)
    {
        var productRepository = new ProductRepository();
        var orderRepository = new OrderRepository();

        Console.WriteLine("Welcome to the Ecommerce Console App!");
        Console.WriteLine("=====================================");

        while (true)
        {
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View All Products");
            Console.WriteLine("3. Update Product");
            Console.WriteLine("4. Delete Product");
            Console.WriteLine("5. Make an Order");
            Console.WriteLine("6. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter product name: ");
                    var productName = Console.ReadLine();

                    Console.Write("Enter product price: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal productPrice))
                    {
                        var newProduct = await productRepository.CreateAsync(new Product { Name = productName, Price = productPrice });
                        Console.WriteLine($"New Product created: {newProduct.Name}, Price: {newProduct.Price}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid price format.");
                    }
                    break;

                case "2":
                     await ViewAllProductsAsync(productRepository); // Call the method to view all products
                    break;

                case "3":
                    Console.Write("Enter the ID of the product to update: ");
                    if (int.TryParse(Console.ReadLine(), out int productIdToUpdate))
                    {
                        var productToUpdate = await productRepository.GetByIdAsync(productIdToUpdate);
                        if (productToUpdate != null)
                        {
                            Console.Write("Enter new product name: ");
                            var updatedProductName = Console.ReadLine();

                            Console.Write("Enter new product price: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal updatedProductPrice))
                            {
                                productToUpdate.Name = updatedProductName;
                                productToUpdate.Price = updatedProductPrice;

                                var updatedProduct = await productRepository.UpdateAsync(productIdToUpdate, productToUpdate);
                                Console.WriteLine($"Updated Product: {updatedProduct.Name}, Price: {updatedProduct.Price}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid price format.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Product not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid product ID.");
                    }
                    break;

                case "4":
                    Console.Write("Enter the ID of the product to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int productIdToDelete))
                    {
                        var productToDelete = await productRepository.GetByIdAsync(productIdToDelete);
                        if (productToDelete != null)
                        {
                            await productRepository.DeleteAsync(productIdToDelete);
                            Console.WriteLine("Product deleted.");
                        }
                        else
                        {
                            Console.WriteLine("Product not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid product ID.");
                    }
                    break;

                case "5":
                    Console.Write("Enter a comma-separated list of product IDs for the order: ");
                    var productIdsForOrder = Console.ReadLine()?.Split(',');
                    var productsForOrder = new List<Product>();

                    foreach (var productId in productIdsForOrder)
                    {
                        if (int.TryParse(productId.Trim(), out int id))
                        {
                            var product = await productRepository.GetByIdAsync(id);
                            if (product != null)
                            {
                                productsForOrder.Add(product);
                            }
                            else
                            {
                                Console.WriteLine($"Product with ID {id} not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Invalid product ID: {productId}");
                        }
                    }

                    if (productsForOrder.Count > 0)
                    {
                        var orderTotalAmount = productsForOrder.Sum(product => product.Price);
                        var newOrder = await orderRepository.CreateAsync(new Order { Products = productsForOrder });
                        Console.WriteLine($"New Order created with Total Amount: {orderTotalAmount}");
                    }
                    break;


                case "6":
                    Console.WriteLine("Exiting the program. Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please choose a valid option.");
                    break;
            }
        }
    }

    // Method to retrieve and display all products
    private static async Task ViewAllProductsAsync(ProductRepository productRepository)
    {
        var allProducts = await productRepository.GetAllProductsAsync();
        Console.WriteLine("All Products:");
        foreach (var product in allProducts)
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}");
        }
    }
}

