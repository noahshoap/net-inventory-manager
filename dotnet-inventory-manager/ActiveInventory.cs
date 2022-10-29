namespace dotnet_inventory_manager;

public class ActiveInventory
{
    public SortedDictionary<string, SortedDictionary<string, Item>> InvByCategory { get; }
    public SortedDictionary<string, Item> InvByName { get; }
    public SortedDictionary<ulong, Item> InvById { get; }

    public ActiveInventory()
     {
         InvByCategory = new SortedDictionary<string, SortedDictionary<string, Item>>
         {
             {"Perishable", new SortedDictionary<string, Item>()},
             {"NonPerishable", new SortedDictionary<string, Item>()}
         };

         InvByName = new SortedDictionary<string, Item>();
         InvById = new SortedDictionary<ulong, Item>();
     }

     public int AddItem(Item item)
     {
         if (InvByName.ContainsKey(item.Name))
         {
             Console.Error.WriteLine($"Item '{item.Name}' is already in the inventory.");
             return -1;
         } 
         
         if (InvById.ContainsKey((item.ID)))
         {
             Console.Error.WriteLine($"ID '{item.ID}' is already an ID for another Item.");
             return -1;
         }

         if (item.Category != "Perishable" && item.Category != "NonPerishable")
             throw new FormatException("Bad Item category");
         
         InvByCategory[item.Category].Add(item.Name, item);
         InvByName.Add(item.Name, item);
         InvById.Add(item.ID, item);

         return 0;
     }

     public int RemoveItem(string name)
     {
         if (!InvByName.ContainsKey(name))
         {
             Console.Error.WriteLine($"Couldn't find Item with name '{name}'.");
             return -1;
         }
         
         // Otherwise, get the Item.
         var item = InvByName[name];
         InvByCategory[item.Category].Remove(item.Name);
         InvById.Remove(item.ID);
         InvByName.Remove(name);

         return 0;
     }

     public int UpdateItem(string name, string field, string value)
     {
       try
       {
           field = field.ToLower();
           
           var item = SearchByName(name) ?? throw new Exception($"Item {name} is not in the inventory.");

           if (field == "name")
           {
               if (SearchByName(value) is not null)
                   throw new Exception($"Name {value} is already used by another Item.");

               InvByName.Remove(name);
               InvByCategory[item.Category].Remove(name);
               
               item.SetValue(field, value);
               InvByName.Add(item.Name, item);
               InvByCategory[item.Category].Add(item.Name, item);
           } else if (field == "id")
           {
               var id = ulong.Parse(value);

               if (SearchById(id) is not null) throw new Exception($"ID '{id}' is already used by another item.");

               InvById.Remove(item.ID);
               item.SetValue(field, value);
               InvById.Add(item.ID, item);
           } else if (field == "category")
           {
               throw new Exception("You can only set category when creating an Item.");
           }
           else
           {
               item.SetValue(field, value);
           }
       }
       catch (Exception e)
       {
           Console.Error.WriteLine(e.Message);
           return -1;
       }
         
         return 0;
     }
     
     public Item? SearchByName(string name)
     {
         return InvByName.ContainsKey(name) ? InvByName[name] : null;
     }

     public Item? SearchById(ulong id)
     {
         return InvById.ContainsKey(id) ? InvById[id] : null;
     }

     public void PrintItems(string value)
     {
         if (InvById.Count == 0)
         {
             Console.Error.WriteLine("Inventory is currently empty.");
             return;
         }
         if (value.ToLower() == "all")
         {
             foreach (var item in InvById)
             {
                 Console.WriteLine(item.Value.Print());
             }
         } else if (value.ToLower() == "perishable" || value.ToLower() == "nonperishable")
         {
             var category = (value == "perishable") ? "Perishable" : "NonPerishable";
             foreach (var item in InvByCategory[category])
             {
                 Console.WriteLine(item.Value.Print());
             }
         }
         else
         {
             var item = SearchByName(value);
             
             if (item is null)
             {
                 Console.Error.WriteLine($"Item '{value}' is not in the inventory.");
                 return;
             }

             Console.WriteLine(item.Print());
         }
     }

     public void PrintHead()
     {
         Console.WriteLine("");
     }
}