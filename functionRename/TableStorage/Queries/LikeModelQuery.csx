#load "BaseModelQuery.csx"
#load "..\..\Configuration\TablesNames.csx"
#load "..\..\Configuration\Actions.csx"
#load "..\Models\UserStorageModel.csx"

using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public class LikeModelQuery : BaseModelQuery
{
    public LikeModelQuery() : base(TablesNames.Like) { }

    public void Add(LikeStorageModel like)
    {
        TableOperation insertOperation = TableOperation.Insert(like);

        table.Execute(insertOperation);
    }
}
