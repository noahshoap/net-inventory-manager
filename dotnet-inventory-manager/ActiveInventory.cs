namespace dotnet_inventory_manager;

public class ActiveInventory
{
    private readonly SortedDictionary<string, SortedDictionary<string, Item>> _invByCategory;
    private readonly SortedDictionary<string, Item> _invByName;
    private readonly SortedDictionary<ulong, Item> _invById;
     public ActiveInventory()
     {
         _invByCategory = new SortedDictionary<string, SortedDictionary<string, Item>>
         {
             {"Perishable", new SortedDictionary<string, Item>()},
             {"NonPerishable", new SortedDictionary<string, Item>()}
         };

         _invByName = new SortedDictionary<string, Item>();
         _invById = new SortedDictionary<ulong, Item>();
     }

     public int AddItem(Item item)
     {
         if (_invByName.ContainsKey(item.Name))
         {
             Console.Error.WriteLine($"Item '{item.Name}' is already in the inventory.");
             return -1;
         } 
         
         if (_invById.ContainsKey((item.ID)))
         {
             Console.Error.WriteLine($"ID '{item.ID}' is already an ID for another Item.");
             return -1;
         }

         if (item.Category != "Perishable" && item.Category != "NonPerishable")
             throw new FormatException("Bad Item category");
         
         _invByCategory[item.Category].Add(item.Name, item);
         _invByName.Add(item.Name, item);
         _invById.Add(item.ID, item);

         return 0;
     }

     public int RemoveItem(string name)
     {
         if (!_invByName.ContainsKey(name))
         {
             Console.Error.WriteLine($"Couldn't find Item with name '{name}'.");
             return -1;
         }
         
         // Otherwise, get the Item.
         var item = _invByName[name];
         _invByCategory[item.Category].Remove(item.Name);
         _invById.Remove(item.ID);
         _invByName.Remove(name);

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

               _invByName.Remove(name);
               _invByCategory[item.Category].Remove(name);
               
               item.SetValue(field, value);
               _invByName.Add(item.Name, item);
               _invByCategory[item.Category].Add(item.Name, item);
           } else if (field == "id")
           {
               var id = ulong.Parse(value);

               if (SearchById(id) is not null) throw new Exception($"ID '{id}' is already used by another item.");

               _invById.Remove(item.ID);
               item.SetValue(field, value);
               _invById.Add(item.ID, item);
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
         return _invByName.ContainsKey(name) ? _invByName[name] : null;
     }

     public Item? SearchById(ulong id)
     {
         return _invById.ContainsKey(id) ? _invById[id] : null;
     }

     public void PrintItems(string value)
     {
         if (_invById.Count == 0)
         {
             Console.Error.WriteLine("Inventory is currently empty.");
             return;
         }
         if (value.ToLower() == "all")
         {
             foreach (var item in _invById)
             {
                 item.Value.Print();
             }
         } else if (value == "Perishable" || value == "NonPerishable")
         {
             foreach (var item in _invByCategory[value])
             {
                 item.Value.Print();
             }
         }
         else
         {
             var item = SearchByName(value);
             
             if (item is null)
             {
                 Console.Error.WriteLine($"Item '{value} is not in the inventory.");
                 return;
             }
             
             item.Print();
         }
     }

     public void PrintHead()
     {
         Console.WriteLine("");
     }
}