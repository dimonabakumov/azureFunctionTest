#r "System.Collections"
#r "Microsoft.WindowsAzure.Storage"

#load "BaseModelQuery.csx"
#load "..\..\Configuration\TablesNames.csx"
#load "..\Models\SlackStorageModel.csx"

using System;
using System.Linq;
using System.Collections.Specialized;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public class SlackModelQuery : BaseModelQuery
{
	public SlackModelQuery(NameValueCollection coll) : base(TablesNames.SlackCall)
    {
        // Create a new customer entity.
        var slackCall = new SlackStorageModel
        {
            RowKey = coll.Get("user_id").Trim('\''),
            PartitionKey = DateTimeOffset.Now.Ticks.ToString(),
            Timestamp = DateTimeOffset.Now,
            token = coll.Get("token").Trim('\''),
            user_name = coll.Get("user_name").Trim('\''),
            text = coll.Get("text").Trim('\''),
        };

        // Create the TableOperation object that inserts the customer entity.
        TableOperation insertOperation = TableOperation.Insert(slackCall);

        // Execute the insert operation.
        table.Execute(insertOperation);

        var exQuery = new TableQuery<SlackStorageModel>().Where(
            TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, coll.Get("user_id")));
        var results = table.ExecuteQuery(exQuery).ToList();
    }
}
