#r "System.Collections"
#r "Microsoft.WindowsAzure.Storage"

using System;
using System.Linq;
using System.Collections.Specialized;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public class BaseModelQuery
{
    protected CloudTableClient tableClient;

    protected CloudTable table;

    public BaseModelQuery(string tableName)
	{
        //Get connection string to storage
        var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true;");

        // Create the table client.
        tableClient = storageAccount.CreateCloudTableClient();

        // Retrieve a reference to the table.
        table = tableClient.GetTableReference(tableName);

        // Create the table if it doesn't exist.
        table.CreateIfNotExists();
    }
}
