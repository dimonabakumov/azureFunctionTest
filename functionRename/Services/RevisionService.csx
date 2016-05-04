#load "..\ModelsGenerator\Revision.csx"
#load "..\Requests\Revisions.csx"
#load "..\TableStorage\Queries\PostModelQuery.csx"
#load "..\TableStorage\Models\UserStorageModel.csx"

using System;

public class RevisionService
{
	public PostStorageModel CreateIfNotExists(string sessionId)
	{
        var me = new UserModelQuery().Get(sessionId, Actions.Me);
        var postQuery = new PostModelQuery();

        var existedRevision = postQuery.Get(me.Id);

        if (existedRevision != null)
            return existedRevision;

        else
        {
            var generatedRevision = new Revision().Create(Guid.Parse("C7958729-B599-9D4B-14A6-0001754AE924"), true);
            var postStorageModel = new Revisions().Post(generatedRevision, me);
            postQuery.Add(postStorageModel);

            return postStorageModel;
        }
    }
}
