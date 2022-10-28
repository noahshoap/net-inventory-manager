namespace dotnet_inventory_manager;

public class InventoryManager
{
    private bool _commandLine;
    private string _fileName;
    
    public InventoryManager(bool commandLine, string inventoryFile)
    {
        this._commandLine = commandLine;
        this._fileName = inventoryFile;
        // Add Sales later.
    }

    public void ReadCsvFile()
    {
        
    }
}