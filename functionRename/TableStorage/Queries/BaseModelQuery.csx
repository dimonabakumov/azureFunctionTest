﻿#r "System.Collections"
#r "Microsoft.WindowsAzure.Storage"
#r "scriptcs_packages\WindowsAzure.Storage.5.0.2\lib\net40\Microsoft.WindowsAzure.Storage.dll"

using System;
using System.Linq;
using System.Collections.Specialized;
using Microsoft.WindowsAzure.Storage;

public class BaseModelQuery
{
    protected CloudTableClient tableClient;

    protected CloudTable table;

    public BaseModelQuery(string tableName)
	{
        //Get connection string to storage
        var storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("Storage"));

        // Create the table client.
        tableClient = storageAccount.CreateCloudTableClient();

        // Retrieve a reference to the table.
        table = tableClient.GetTableReference(tableName);

        // Create the table if it doesn't exist.
        table.CreateIfNotExists();
    }
}
