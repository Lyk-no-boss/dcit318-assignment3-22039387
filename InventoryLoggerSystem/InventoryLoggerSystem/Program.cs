using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace InventoryLoggerSystem
{
    // Marker interface for inventory entities
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // Immutable inventory record
    public record InventoryItem(
        int Id,
        string Name,
        int Quantity,
        DateTime DateAdded
    ) : IInventoryEntity;

    // Generic inventory logger
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private List<T> _log = new List<T>();
        private string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        // Add an item to the log
        public void Add(T item)
        {
            _log.Add(item);
        }

        // Return all items
        public List<T> GetAll()
        {
            return new List<T>(_log);
        }

        // Save items to JSON file
        public void SaveToFile()
        {
            try
            {
                string json = JsonSerializer.Serialize(
                    _log,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }
                );

                using (StreamWriter writer = new StreamWriter(_filePath))
                {
                    writer.Write(json);
                }

                Console.WriteLine("Inventory data saved successfully.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(
                    "Error: You do not have permission to write to the file."
                );
            }
            catch (IOException ex)
            {
                Console.WriteLine(
                    $"File error while saving data: {ex.Message}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Unexpected error while saving data: {ex.Message}"
                );
            }
        }

        // Load items from JSON file
        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine(
                        "Error: Inventory file does not exist."
                    );
                    return;
                }

                using (StreamReader reader = new StreamReader(_filePath))
                {
                    string json = reader.ReadToEnd();

                    List<T>? loadedItems =
                        JsonSerializer.Deserialize<List<T>>(json);

                    if (loadedItems != null)
                    {
                        _log = loadedItems;
                    }
                }

                Console.WriteLine("Inventory data loaded successfully.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(
                    "Error: You do not have permission to read the file."
                );
            }
            catch (IOException ex)
            {
                Console.WriteLine(
                    $"File error while loading data: {ex.Message}"
                );
            }
            catch (JsonException ex)
            {
                Console.WriteLine(
                    $"Error: The inventory file contains invalid JSON. {ex.Message}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Unexpected error while loading data: {ex.Message}"
                );
            }
        }
    }

    // Integration layer
    public class InventoryApp
    {
        private InventoryLogger<InventoryItem> _logger;

        public InventoryApp()
        {
            string filePath = "inventory.json";
            _logger = new InventoryLogger<InventoryItem>(filePath);
        }

        // Add sample inventory data
        public void SeedSampleData()
        {
            _logger.Add(
                new InventoryItem(
                    101,
                    "Laptop",
                    10,
                    new DateTime(2026, 9, 1)
                )
            );

            _logger.Add(
                new InventoryItem(
                    102,
                    "Keyboard",
                    25,
                    new DateTime(2026, 9, 2)
                )
            );

            _logger.Add(
                new InventoryItem(
                    103,
                    "Mouse",
                    40,
                    new DateTime(2026, 9, 3)
                )
            );

            _logger.Add(
                new InventoryItem(
                    104,
                    "Monitor",
                    15,
                    new DateTime(2026, 9, 4)
                )
            );

            _logger.Add(
                new InventoryItem(
                    105,
                    "Printer",
                    8,
                    new DateTime(2026, 9, 5)
                )
            );

            Console.WriteLine("Sample inventory data added.");
        }

        public void SaveData()
        {
            _logger.SaveToFile();
        }

        public void LoadData()
        {
            _logger.LoadFromFile();
        }

        public void PrintAllItems()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("       RECOVERED INVENTORY DATA");
            Console.WriteLine("======================================");

            List<InventoryItem> items = _logger.GetAll();

            foreach (InventoryItem item in items)
            {
                Console.WriteLine(
                    $"ID: {item.Id} | " +
                    $"Name: {item.Name} | " +
                    $"Quantity: {item.Quantity} | " +
                    $"Date Added: {item.DateAdded:yyyy-MM-dd}"
                );
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("======================================");
            Console.WriteLine("       INVENTORY LOGGER SYSTEM");
            Console.WriteLine("======================================");

            // Create application
            InventoryApp app = new InventoryApp();

            // Add sample data
            app.SeedSampleData();

            // Save data to file
            app.SaveData();

            Console.WriteLine();
            Console.WriteLine("Simulating a new application session...");
            Console.WriteLine();

            // Create a new application instance.
            // This starts with an empty in-memory list.
            app = new InventoryApp();

            // Load saved data from file
            app.LoadData();

            // Display recovered data
            app.PrintAllItems();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
