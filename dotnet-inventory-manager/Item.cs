namespace dotnet_inventory_manager;

public abstract class Item
{
    public string Name { get; private set; }
    public string Category { get; private set; }
    public string SubCategory { get; private set; }
    public ulong Quantity { get; private set; }
    public ulong Backorder { get; private set; }
    public ulong ID { get; private set; }
    public double Price { get; private set; }
    public double BuyCost { get; private set; }
    public double Tax { get; private set; }
    public double TotalPrice { get; private set; }
    public double Profit { get; private set; }
    
    public Item(string name, string category, string subcategory, string quantity, string backorder, string id,
        string price, string cost, string tax)
    {
        Name = name;
        Category = category;
        SubCategory = subcategory;
        Quantity = ulong.Parse(quantity);
        Backorder = ulong.Parse(backorder);
        ID = ulong.Parse(id);
        Price = double.Parse(price);
        BuyCost = double.Parse(cost);
        Tax = double.Parse(tax);
        Profit = Price - BuyCost;
        TotalPrice = Price * (Tax * Price);
        
        if (Price < 0.0 || BuyCost < 0.0 || Tax < 0.0)
            throw new ArgumentException("Can't have negative price / purchase cost / tax.");
    }

    public abstract void Print();

    public abstract void PrintAsCsv();
}