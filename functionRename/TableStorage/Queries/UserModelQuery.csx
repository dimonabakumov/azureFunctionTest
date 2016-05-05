#load "BaseModelQuery.csx"
#load "..\..\Configuration\Actions.csx"
#load "..\..\Configuration\TablesNames.csx"
#load "..\Models\UserStorageModel.csx"

using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public class UserModelQuery : BaseModelQuery
{
    public UserModelQuery() : base(TablesNames.User) { }

    public UserStorageModel Get(string userId, Actions action)
    {
        var exQuery = UserQuery(userId, action);
        return table.ExecuteQuery(exQuery).FirstOrDefault();
    }

    public List<UserStorageModel> GetList(string userId, Actions action)
    {
        var exQuery = UserQuery(userId, action);
        return table.ExecuteQuery(exQuery).ToList();
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

    protected TableQuery<UserStorageModel> UserQuery(string userId, Actions action)
    {
        return new TableQuery<UserStorageModel>().Where(
            TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, userId),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Actions", QueryComparisons.Equal, action.ToString())));
    }
}
