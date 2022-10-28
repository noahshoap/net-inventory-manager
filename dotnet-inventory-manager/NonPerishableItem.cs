namespace dotnet_inventory_manager;

public class NonPerishableItem : Item
{
    public NonPerishableItem(string name, string category, string subcategory, string quantity, string backorder, string id,
        string price, string cost, string tax)
        : base(name, category, subcategory, quantity, backorder, id, price, cost, tax)
    {
    }

    public override void Print()
    {
        var spacing = "       ";
        Console.WriteLine($"{ID}{spacing}{Name}{spacing}{Category}{spacing}{Quantity}{spacing}{Backorder}{spacing}{BuyCost}{spacing}{Price}{spacing}{Tax * Price}{spacing}{TotalPrice}{spacing}{Profit}{spacing}-1");
    }

    public override string PrintAsCsv()
    {
        return $"{base.PrintAsCsv()}-1";
    }
}