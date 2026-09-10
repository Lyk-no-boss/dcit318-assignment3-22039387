using System;
using System.Collections.Generic;

namespace WarehouseInventorySystem
{
    // =========================================================
    // a. Marker Interface for Inventory Items
    // =========================================================

    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }


    // =========================================================
    // b. ElectronicItem
    // =========================================================

    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public int WarrantyMonths { get; set; }


        public ElectronicItem(
            int id,
            string name,
            int quantity,
            string brand,
            int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }
    }


    // =========================================================
    // c. GroceryItem
    // =========================================================

    public class GroceryItem : IInventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }


        public GroceryItem(
            int id,
            string name,
            int quantity,
            DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }
    }


    // =========================================================
    // e. Custom Exceptions
    // =========================================================

    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message)
            : base(message)
        {
        }
    }


    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message)
            : base(message)
        {
        }
    }


    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message)
            : base(message)
        {
        }
    }


    // =========================================================
    // d. Generic Inventory Repository
    // =========================================================

    public class InventoryRepository<T>
        where T : IInventoryItem
    {
        private Dictionary<int, T> _items =
            new Dictionary<int, T>();


        // Add item
        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
            {
                throw new DuplicateItemException(
                    $"An item with ID {item.Id} already exists."
                );
            }

            _items.Add(item.Id, item);
        }


        // Get item by ID
        public T GetItemById(int id)
        {
            if (!_items.ContainsKey(id))
            {
                throw new ItemNotFoundException(
                    $"Item with ID {id} was not found."
                );
            }

            return _items[id];
        }


        // Remove item
        public void RemoveItem(int id)
        {
            if (!_items.ContainsKey(id))
            {
                throw new ItemNotFoundException(
                    $"Cannot remove item. Item with ID {id} was not found."
                );
            }

            _items.Remove(id);
        }


        // Get all items
        public List<T> GetAllItems()
        {
            return new List<T>(_items.Values);
        }


        // Update quantity
        public void UpdateQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new InvalidQuantityException(
                    "Quantity cannot be negative."
                );
            }

            if (!_items.ContainsKey(id))
            {
                throw new ItemNotFoundException(
                    $"Item with ID {id} was not found."
                );
            }

            _items[id].Quantity = newQuantity;
        }
    }


    // =========================================================
    // f. WareHouseManager
    // =========================================================

    public class WareHouseManager
    {
        private InventoryRepository<ElectronicItem> _electronics;
        private InventoryRepository<GroceryItem> _groceries;


        public WareHouseManager()
        {
            _electronics =
                new InventoryRepository<ElectronicItem>();

            _groceries =
                new InventoryRepository<GroceryItem>();
        }


        // =====================================================
        // SeedData()
        // =====================================================

        public void SeedData()
        {
            // Electronic items

            _electronics.AddItem(
                new ElectronicItem(
                    101,
                    "Laptop",
                    10,
                    "Dell",
                    24
                )
            );

            _electronics.AddItem(
                new ElectronicItem(
                    102,
                    "Smartphone",
                    15,
                    "Samsung",
                    12
                )
            );

            _electronics.AddItem(
                new ElectronicItem(
                    103,
                    "Headphones",
                    20,
                    "Sony",
                    6
                )
            );


            // Grocery items

            _groceries.AddItem(
                new GroceryItem(
                    201,
                    "Milk",
                    30,
                    new DateTime(2026, 10, 15)
                )
            );

            _groceries.AddItem(
                new GroceryItem(
                    202,
                    "Bread",
                    25,
                    new DateTime(2026, 9, 20)
                )
            );

            _groceries.AddItem(
                new GroceryItem(
                    203,
                    "Rice",
                    50,
                    new DateTime(2027, 3, 10)
                )
            );
        }


        // =====================================================
        // PrintAllItems<T>()
        // =====================================================

        public void PrintAllItems<T>(
            InventoryRepository<T> repo)
            where T : IInventoryItem
        {
            foreach (T item in repo.GetAllItems())
            {
                Console.WriteLine(
                    $"ID: {item.Id} | " +
                    $"Name: {item.Name} | " +
                    $"Quantity: {item.Quantity}"
                );
            }
        }


        // =====================================================
        // IncreaseStock<T>()
        // =====================================================

        public void IncreaseStock<T>(
            InventoryRepository<T> repo,
            int id,
            int quantity)
            where T : IInventoryItem
        {
            try
            {
                T item = repo.GetItemById(id);

                int newQuantity =
                    item.Quantity + quantity;

                repo.UpdateQuantity(
                    id,
                    newQuantity
                );

                Console.WriteLine(
                    $"Stock increased successfully. " +
                    $"{item.Name} now has {newQuantity} units."
                );
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine(
                    $"Error: {ex.Message}"
                );
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine(
                    $"Error: {ex.Message}"
                );
            }
        }


        // =====================================================
        // RemoveItemById<T>()
        // =====================================================

        public void RemoveItemById<T>(
            InventoryRepository<T> repo,
            int id)
            where T : IInventoryItem
        {
            try
            {
                T item = repo.GetItemById(id);

                repo.RemoveItem(id);

                Console.WriteLine(
                    $"{item.Name} was removed successfully."
                );
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine(
                    $"Error: {ex.Message}"
                );
            }
        }


        // =====================================================
        // Methods used for testing exceptions
        // =====================================================

        public void AddDuplicateElectronic()
        {
            try
            {
                _electronics.AddItem(
                    new ElectronicItem(
                        101,
                        "Another Laptop",
                        5,
                        "HP",
                        12
                    )
                );
            }
            catch (DuplicateItemException ex)
            {
                Console.WriteLine(
                    $"Duplicate Error: {ex.Message}"
                );
            }
        }


        public void RemoveNonExistentGrocery()
        {
            try
            {
                _groceries.RemoveItem(999);
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine(
                    $"Removal Error: {ex.Message}"
                );
            }
        }


        public void UpdateWithInvalidQuantity()
        {
            try
            {
                _electronics.UpdateQuantity(
                    101,
                    -10
                );
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine(
                    $"Quantity Error: {ex.Message}"
                );
            }
        }


        // =====================================================
        // Accessors for demonstration
        // =====================================================

        public InventoryRepository<ElectronicItem>
            GetElectronics()
        {
            return _electronics;
        }


        public InventoryRepository<GroceryItem>
            GetGroceries()
        {
            return _groceries;
        }
    }


    // =========================================================
    // Main Application
    // =========================================================

    class Program
    {
        static void Main(string[] args)
        {
            // i. Instantiate WareHouseManager

            WareHouseManager manager =
                new WareHouseManager();


            // ii. Seed data

            manager.SeedData();


            // iii. Print all grocery items

            Console.WriteLine(
                "======================================"
            );

            Console.WriteLine(
                "           GROCERY ITEMS"
            );

            Console.WriteLine(
                "======================================"
            );

            manager.PrintAllItems(
                manager.GetGroceries()
            );


            // iv. Print all electronic items

            Console.WriteLine();
            Console.WriteLine(
                "======================================"
            );

            Console.WriteLine(
                "         ELECTRONIC ITEMS"
            );

            Console.WriteLine(
                "======================================"
            );

            manager.PrintAllItems(
                manager.GetElectronics()
            );


            // v. Test duplicate item exception

            Console.WriteLine();
            Console.WriteLine(
                "======================================"
            );

            Console.WriteLine(
                "       TESTING EXCEPTIONS"
            );

            Console.WriteLine(
                "======================================"
            );

            Console.WriteLine();
            Console.WriteLine(
                "1. Adding duplicate item:"
            );

            manager.AddDuplicateElectronic();


            // Test non-existent item

            Console.WriteLine();
            Console.WriteLine(
                "2. Removing non-existent item:"
            );

            manager.RemoveNonExistentGrocery();


            // Test invalid quantity

            Console.WriteLine();
            Console.WriteLine(
                "3. Updating with invalid quantity:"
            );

            manager.UpdateWithInvalidQuantity();


            Console.WriteLine();
            Console.WriteLine(
                "Press any key to exit..."
            );

            Console.ReadKey();
        }
    }
}
