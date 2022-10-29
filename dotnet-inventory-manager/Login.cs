namespace dotnet_inventory_manager;

public class Login
{
    private const string FileName = "accounts.csv";
    private SortedDictionary<string, User> _users;

    public Login()
    {
        _users = new SortedDictionary<string, User>();
        ReadCsv();
    }

    public bool CreateUser(string username, string password, string account)
    {
        account = account.ToLower();

        if (account != "manager" && account != "owner" && account != "employee")
        {
            Console.Error.WriteLine($"Invalid account type '{account}'.");
            return false;
        }

        if (_users.ContainsKey(username))
        {
            Console.Error.WriteLine($"User '{username}' already exists.");
            return false;
        }
        
        // Otherwise, create user.
        var user = new User(username, password, account);
        _users.Add(username, user);
        
        return true;
    }

    public User? UserInput()
    {
        string name, password;

        while (true)
        {
            Console.Write("\n(L)ogin or (C)reate User: ");
            var input = Console.ReadLine() ?? throw new Exception("Failed to read input.");

            var argument = input[0];
            
            switch (argument)
            {
                case 'L':
                case 'l':
                {
                    Console.Write("Name: ");
                    name = Console.ReadLine() ?? throw new Exception("Failed to read input.");
                    Console.Write("Password: ");
                    password = Console.ReadLine() ?? throw new Exception("Failed to read input.");

                    var user = VerifyUser(name, password);

                    if (user is not null) return user;

                    Console.WriteLine("Invalid username or password.");
                    break;
                }
                case 'C':
                case 'c':
                {
                    Console.Write("Name: ");
                    name = Console.ReadLine() ?? throw new Exception("Failed to read input.");
                    Console.Write("Password: ");
                    password = Console.ReadLine() ?? throw new Exception("Failed to read input.");
                    Console.Write("Account Type: ");
                    var accountType = Console.ReadLine() ?? throw new Exception("Failed to read input.");

                    CreateUser(name, password, accountType);
                    
                    break;
                }
                default:
                    break;
            }

        }
        
        return null;
    }

    public bool ReadCsv()
    {
        string[] lines;
        
        try
        {
            lines = File.ReadAllLines(FileName);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            Console.Error.WriteLine($"Unable to open '{FileName}'.");
            return false;
        }
        
        foreach (var line in lines)
        {
            if (line == lines[0]) continue;

            var info = line.Split(",");

            if (!CreateUser(info[0], info[1], info[2]))
            {
                Console.Error.WriteLine($"Unable to add user '{info[0]}'.");
            }
        }

        return true;
    }

    public bool OutputCsv()
    {
        var lines = new List<string> {"NAME,PASSWORD,ACCOUNT"};

        foreach (var entry in _users)
        {
            var user = entry.Value;
            lines.Add($"{user.Username},{user.Password},{user.AccountType}");
        }
        
        // Write lines to file
        try
        {
            File.WriteAllLines(FileName, lines);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Unable to open file '{FileName}'.  User accounts will be lost!");
            return false;
        }
        return true;
    }

    public User? VerifyUser(string name, string password)
    {
        return _users.ContainsKey(name) ? _users[name] : null;
    }
}