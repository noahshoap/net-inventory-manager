namespace dotnet_inventory_manager;

public class InventoryManager
{
    private readonly bool _commandLine;
    private readonly string _fileName;
    private readonly ActiveInventory _inventory;
    private readonly Login _login;
    private User? _currentUser;
    public InventoryManager(bool commandLine, string inventoryFile)
    {
        this._commandLine = commandLine;
        this._fileName = inventoryFile;
        _inventory = new ActiveInventory();
        _login = new Login();
        // Add Sales later.
    }

    public void ReadCsvFile()
    {
        var lines = File.ReadAllLines(_fileName);

        foreach (var line in lines)
        {
            if (line == lines[0]) continue;
            
            var fields = line.Split(",");

            Item item = fields[2] switch
            {
                "Perishable" => new PerishableItem(fields[0], fields[2], fields[3], fields[4], fields[5], fields[1],
                    fields[6], fields[9], fields[7], fields[11]),
                "NonPerishable" => new NonPerishableItem(fields[0], fields[2], fields[3], fields[4], fields[5], fields[1],
                    fields[6], fields[9], fields[7]),
                _ => throw new FormatException("Bad category")
            };

            _inventory.AddItem(item);
        }
    }

    public int FileOutput()
    {
        var lines = new List<string>();
        
        lines.Add("Name,ID,Category,Sub-Category,Quantity,Backorder,Sale Price,Tax,Total,Price,Buy Cost,Profit,Expiration Date");

        foreach (var entry in _inventory.InvByName)
        {
            lines.Add(entry.Value.PrintAsCsv());
        }

        try
        {
            File.WriteAllLines(_fileName, lines);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            Console.Error.WriteLine($"Unable to open file '{_fileName}' -- inventory WILL BE LOST!");
            return -1;
        }

        return 0;
    }
    
    public int UserInput()
    {
        if (!_commandLine)
        {
            Console.Error.WriteLine("Command line is set to false.  Exiting UserInput()");
            return -1;
        }

        Console.Write("\n(A)dd, (R)emove, (U)pdate, (S)ale, (P)rint, (L)ogout, or (Q)uit: ");

        var line = Console.ReadLine() ?? throw new Exception("couldn't read input");

        var argument = line[0];
        
        switch (argument)
        {
            case 'A':
            case 'a':
            {

                if (_currentUser?.Permission < 3)
                {
                    Console.Error.WriteLine($"User {_currentUser.Username} does n ot have the required permissions to add an Item.");
                    break;
                }
                
                Console.Write("Enter item name: ");
                var name = Console.ReadLine() ?? throw new Exception("Failed to read input");
                Console.Write("Enter item category (Perishable or NonPerishable): ");
                var category = Console.ReadLine() ?? throw new Exception("Failed to read input");
                Console.Write("Enter item sub-category: ");
                var subCategory = Console.ReadLine() ?? throw new Exception("Failed to read input");
                Console.Write("Enter item quantity: ");
                var quantity = Console.ReadLine() ?? throw new Exception("Failed to read input");
                Console.Write("Enter backorder (set to zero unless there is negative stock): ");
                var backorder = Console.ReadLine() ?? throw new Exception("Failed to read input");
                Console.Write("Enter item id: ");
                var id = Console.ReadLine() ?? throw new Exception("Failed to read input");
                Console.Write("Enter sale price: (format xx.xx) $");
                var salePrice = Console.ReadLine() ?? throw new Exception("Failed to read input");
                Console.Write("Enter purchase cost: (format xx.xx) $");
                var buyPrice = Console.ReadLine() ?? throw new Exception("Failed to read input");
                Console.Write("Enter item tax as a decimal value: ");
                var tax = Console.ReadLine() ?? throw new Exception("Failed to read input");
                Console.Write("Enter expiration date (format xx/xx/xxxx) or -1 for NonPerishable: ");
                var expiration = Console.ReadLine() ?? throw new Exception("Failed to read input");
                
                try
                {
                    Item item = category.ToLower() switch
                    {
                        "perishable" => new PerishableItem(name, "Perishable", subCategory, quantity, backorder, id,
                            salePrice, buyPrice, tax, expiration),
                        "nonperishable" => new NonPerishableItem(name, "NonPerishable", subCategory, quantity,
                            backorder, id,
                            salePrice, buyPrice, tax),
                        _ => throw new FormatException("Invalid category.")
                    };

                    if (_inventory.AddItem(item) != -1)
                    {
                        Console.WriteLine($"Added {name} of type {category}");
                    }

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    Console.Error.WriteLine(
                        $"Item '{name}' has not been added to the inventory.  Please correct the input and try again.");
                }

                break;
            }
            case 'R': case 'r':
            {

                if (_currentUser?.Permission < 3)
                {
                    Console.Error.WriteLine($"User {_currentUser.Username} does not the required permissions to remove an Item.");
                    break;
                }
                
                Console.Write("Name: ");
                var name = Console.ReadLine() ?? throw new Exception("Failed to read input");

                if (_inventory.RemoveItem(name) != -1)
                {
                    Console.WriteLine($"Removed {name}");
                }
                
                break;
            }
            case 'S': case 's':
                Console.WriteLine("Sale not implemented yet.");
                break;
            case 'P': case 'p':
                Console.WriteLine("\nPlease select a category to print or enter an item name.\n");
                Console.Write("All | Perishable | NonPerishable | Item Name : ");

                var tmp = Console.ReadLine() ?? throw new Exception("Failed to read input.");
                
                _inventory.PrintItems(tmp);
                break;
            case 'L': case 'l':
                _currentUser = null;
                UserLogin();
                break;
            case 'U':
            case 'u':
            {
                if (_currentUser?.Permission < 3)
                {
                    Console.Error.WriteLine(
                        $"User {_currentUser.Username} does not the required permissions to update an Item.");
                    break;
                }

                Console.Write("Enter name of item to update: ");
                var name = Console.ReadLine() ?? throw new Exception("Failed to read input.");
                Console.Write("Enter field to update (e.g. name, id, tax, etc): ");
                var category = Console.ReadLine() ?? throw new Exception("Failed to read input.");
                Console.Write($"Enter new value for {category}: ");
                var value = Console.ReadLine() ?? throw new Exception("Failed to read input.");

                if (_inventory.UpdateItem(name, category, value) != -1)
                {
                    Console.WriteLine($"Updated {category} of {name} to {value}.");
                }
                
                break;
            }
            case 'Q': case 'q':
                Console.WriteLine("Exiting InventoryManager.");
                return -1;
            default:
                Console.WriteLine("Usage: <(A)dd | (R)emove | (U)pdate | (S)ale | (P)rint | (L)ogout | (Q)uit>");
                break;
        }
        
        return 0;
    }

    public bool UserLogin()
    {
        _currentUser = _login.UserInput();
        _login.OutputCsv();

        if (_currentUser is null) return false;

        return true;
    }
}