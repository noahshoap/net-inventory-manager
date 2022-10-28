namespace dotnet_inventory_manager;

public class PerishableItem : Item
{
    public string ExpirationDate { get; private set; }
    
    public PerishableItem(string name, string category, string subcategory, string quantity, string backorder, string id,
        string price, string cost, string tax, string expDate)
    : base(name, category, subcategory, quantity, backorder, id, price, cost, tax)
    {
        ExpirationDate = expDate;
    }

    public override void Print()
    {
        var spacing = "       ";
        Console.WriteLine($"{ID}{spacing}{Name}{spacing}{Category}{spacing}{Quantity}{spacing}{Backorder}{spacing}{BuyCost}{spacing}{Price}{spacing}{Tax * Price}{spacing}{TotalPrice}{spacing}{Profit}{spacing}{ExpirationDate}");
    }

    public override void SetValue(string key, string value)
    {
        key = key.ToLower();
        switch (key)
        {
            case "expiration_date":
                ExpirationDate = value;
                break;
            default:
                base.SetValue(key, value);
                break;
        }
    }

    public override string PrintAsCsv()
    {
        return $"{base.PrintAsCsv()}{ExpirationDate}";
    }
}