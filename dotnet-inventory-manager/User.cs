namespace dotnet_inventory_manager;

public class User
{
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string AccountType { get; private set; }
    public int Permission { get; private set; }
    public User(string username, string password, string accountType)
    {
        Username = username;
        Password = password;
        AccountType = accountType;

        Permission = accountType switch
        {
            "owner" => 5,
            "manager" => 3,
            "employee" => 1,
            _ => throw new Exception("Bad account type")
        };
    }
}