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

    protected TableQuery<UserStorageModel> UserQuery(string sessionId, Actions action)
    {
        return new TableQuery<UserStorageModel>().Where(
            TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, sessionId),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Actions", QueryComparisons.Equal, action.ToString())));
    }
}
