#load "BaseModelQuery.csx"
#load "..\..\Configuration\TablesNames.csx"
#load "..\..\Configuration\Actions.csx"
#load "..\Models\UserStorageModel.csx"

using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public class UserModelQuery : BaseModelQuery
{
    public UserModelQuery() : base(TablesNames.Like) { }

    public UserStorageModel Get(string userId, Actions action)
    {
        var exQuery = new TableQuery<UserStorageModel>().Where(
                TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("SlackUserId", QueryComparisons.Equal, userId),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Actions", QueryComparisons.Equal, action.ToString())));

        return table.ExecuteQuery(exQuery).FirstOrDefault();
    }

    public void TryGet(string userId, Actions action)
    {
        var results = Get(userId, action);

        if (results != null)
            table.Delete();
    }

    public TableResult Add(UserStorageModel createdUser)
    {
        TableOperation insertOperation = TableOperation.Insert(createdUser);

        TableResult retrievedResult = table.Execute(insertOperation);

        return retrievedResult;
    }
}
