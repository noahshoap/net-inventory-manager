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

    public override string Print()
    {
        return string.Format($"{base.Print()}{ExpirationDate,-10}");
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