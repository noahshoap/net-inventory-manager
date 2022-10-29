namespace dotnet_inventory_manager;

public class NonPerishableItem : Item
{
    public NonPerishableItem(string name, string category, string subcategory, string quantity, string backorder, string id,
        string price, string cost, string tax)
        : base(name, category, subcategory, quantity, backorder, id, price, cost, tax)
    {
    }

    public override string Print()
    {
        return string.Format($"{base.Print()}{"-1",-10}");
    }

    public override string PrintAsCsv()
    {
        return $"{base.PrintAsCsv()}-1";
    }
}