#load "BaseModelQuery.csx"
#load "..\..\Configuration\Actions.csx"
#load "..\..\Configuration\TablesNames.csx"
#load "..\Models\UserStorageModel.csx"
#load "..\..\Requests\Authorisation.csx"

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

    public bool TryGet(string sessionId, Actions action, string username, string password)
    {
        var exQuery = QueryByAllFields(sessionId, action, username, password);
        var results = table.ExecuteQuery(exQuery).FirstOrDefault();
        if (results != null)
        {
            var newToken = new Authorisation().RefreshToken(username, password);
            if (newToken == null)
                return false;

            UpdateToken(results.PartitionKey, results.RowKey, newToken);
            return true;
        }
        else
        {
            var usersOfSession = table.ExecuteQuery(UserQuery(sessionId)).ToList();
            Delete(usersOfSession);
            return false;
        }
    }

    public void Add(UserStorageModel createdUser)
    {
        var insertOperation = TableOperation.Insert(createdUser);
        table.Execute(insertOperation);
    }

    public void Delete(List<UserStorageModel> users)
    {
        foreach (var user in users)
        {
            var deleteOperation = TableOperation.Delete(user);
            table.Execute(deleteOperation);
        }
    }

    public void Update(string partitionKey, string rowKey, Guid? revisionId = null, long? commentId = null, Guid? bandId = null, Guid? songId = null, Guid? postId = null)
    {
        var retrieveOperation = TableOperation.Retrieve<UserStorageModel>(partitionKey, rowKey);
        var retrievedResult = table.Execute(retrieveOperation);
        var updateEntity = (UserStorageModel)retrievedResult.Result;

        if (updateEntity != null)
        {
            if(revisionId.HasValue)
                updateEntity.RevisionId = revisionId;

            if (commentId.HasValue)
                updateEntity.CommentId = commentId;

            if (bandId.HasValue)
                updateEntity.BandId = bandId;

            if (songId.HasValue)
                updateEntity.SongId = songId;

            if (postId.HasValue)
                updateEntity.PostId = postId;

            var insertOrReplaceOperation = TableOperation.InsertOrReplace(updateEntity);
            table.Execute(insertOrReplaceOperation);
        }
    }

    public void UpdateToken(string partitionKey, string rowKey, string AccessToken)
    {
        var retrieveOperation = TableOperation.Retrieve<UserStorageModel>(partitionKey, rowKey);
        var retrievedResult = table.Execute(retrieveOperation);
        var updateEntity = (UserStorageModel)retrievedResult.Result;

        if (updateEntity != null)
        {
            updateEntity.AccessToken = AccessToken;

            var insertOrReplaceOperation = TableOperation.InsertOrReplace(updateEntity);
            table.Execute(insertOrReplaceOperation);
        }
    }

    protected TableQuery<UserStorageModel> UserQuery(string sessionId, Actions action)
    {
        return new TableQuery<UserStorageModel>().Where(
            TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, sessionId),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Actions", QueryComparisons.Equal, action.ToString())));
    }

    protected TableQuery<UserStorageModel> UserQuery(string sessionId)
    {
        return new TableQuery<UserStorageModel>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, sessionId));
    }

    protected TableQuery<UserStorageModel> QueryByAllFields(string sessionId, Actions action, string username, string password)
    {
        var firstFilter = TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, sessionId),
                TableOperators.And,TableQuery.GenerateFilterCondition("Actions", QueryComparisons.Equal, action.ToString()));

        var secondFilter = TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("UserName", QueryComparisons.Equal, username),
                TableOperators.And, TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, password));

        return new TableQuery<UserStorageModel>().Where(
            TableQuery.CombineFilters(
                firstFilter,
                TableOperators.And,
                secondFilter));
    }
}
