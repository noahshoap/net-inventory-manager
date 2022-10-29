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
        Price = Math.Round(double.Parse(price), 2);
        BuyCost = Math.Round(double.Parse(cost), 2);
        Tax = Math.Round(Price * Math.Round(double.Parse(tax), 2), 2);
        Profit = Math.Round(Price - BuyCost, 2);
        TotalPrice = Math.Round(Price + Tax, 2);
        
        if (Price < 0.0 || BuyCost < 0.0 || Tax < 0.0)
            throw new ArgumentException("Can't have negative price / purchase cost / tax.");
    }
    
    public virtual string Print()
    {
        return string.Format($"{ID,-7}{Name,-40}{Category, -17}{Quantity,-10}{Backorder, -10}{BuyCost, -15}{Price, -15}{Tax, -15}{TotalPrice, -15}{Profit, -10}");
    }

    public virtual void SetValue(string key, string value)
    {
        try
        {
            key = key.ToLower();
            switch (key)
            {
                case "name":
                    Name = value;
                    break;
                case "category":
                    Category = value;
                    break;
                case "sub_category":
                    SubCategory = value;
                    break;
                case "quantity":
                    Quantity = ulong.Parse(value);
                    break;
                case "backorder":
                    Backorder = ulong.Parse(value);
                    break;
                case "id":
                    ID = ulong.Parse(value);
                    break;
                case "sale_price":
                    var tmpPrice = double.Parse(value);
                    if (tmpPrice < 0) throw new Exception("Negative price");
                    Price = tmpPrice;
                    break;
                case "buy_cost":
                    var tmpCost = double.Parse(value);
                    if (tmpCost < 0) throw new Exception("Negative cost");
                    BuyCost = tmpCost;
                    break;
                case "tax":
                    var tmpTax = double.Parse(value);
                    if (tmpTax < 0) throw new Exception("Negative tax");
                    Tax = tmpTax;
                    break;
                default:
                    throw new Exception($"Could not find key '{key}' in Item or its subclasses.");
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to set value '{value}' for key '{key}' for Item '{Name}'.");
        }
    }

    public virtual string PrintAsCsv()
    {
        return $"{Name},{ID},{Category},{SubCategory},{Quantity},{Backorder},{Price},{Tax},{TotalPrice},{BuyCost},{Profit},";
    }
}