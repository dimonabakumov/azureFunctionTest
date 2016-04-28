#load "BaseModelQuery.csx"
#load "..\..\Configuration\TablesNames.csx"
#load "..\..\Configuration\Actions.csx"
#load "..\Models\UserStorageModel.csx"

using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public class PostModelQuery : BaseModelQuery
{
    public PostModelQuery() : base(TablesNames.Post) { }

    public PostStorageModel Get(Guid creatorId)
    {
        var exQuery = new TableQuery<PostStorageModel>().Where(
            TableQuery.GenerateFilterCondition("CreatorId", QueryComparisons.Equal, creatorId.ToString()));

        return table.ExecuteQuery(exQuery).FirstOrDefault();
    }

    public void Add(PostStorageModel createdPost)
    {
        TableOperation insertOperation = TableOperation.Insert(createdPost);

        table.Execute(insertOperation);
    }
}
