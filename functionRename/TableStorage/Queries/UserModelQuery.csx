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

    public UserStorageModel Get(string sessionId, Actions action)
    {
        var exQuery = UserQuery(sessionId, action);
        return table.ExecuteQuery(exQuery).FirstOrDefault();
    }

    public List<UserStorageModel> GetList(string sessionId, Actions action)
    {
        var exQuery = UserQuery(sessionId, action);
        return table.ExecuteQuery(exQuery).ToList();
    }

    public void TryGet(string sessionId, Actions action)
    {
        var results = Get(sessionId, action);

        if (results != null)
            table.Delete();
    }

    public void Add(UserStorageModel createdUser)
    {
        TableOperation insertOperation = TableOperation.Insert(createdUser);

        table.Execute(insertOperation);
    }

    public void Delete(List<UserStorageModel> users)
    {
        foreach (var user in users)
        {
            TableOperation deleteOperation = TableOperation.Delete(user);
            table.Execute(deleteOperation);
        }
    }

    protected TableQuery<UserStorageModel> UserQuery(string sessionId, Actions action)
    {
        return new TableQuery<UserStorageModel>().Where(
            TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, sessionId),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Actions", QueryComparisons.Equal, action.ToString())));
    }
}
