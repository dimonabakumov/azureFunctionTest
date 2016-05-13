#load "..\ModelsGenerator\Revision.csx"
#load "..\Requests\Revisions.csx"
#load "..\TableStorage\Queries\PostModelQuery.csx"
#load "..\TableStorage\Models\UserStorageModel.csx"
#load "..\TableStorage\Models\LikeStorageModel.csx"

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

    public string Like(string sessionId)
    {
        var revision = CreateIfNotExists(sessionId);
        var newLiker = new Authorisation().GetUser(new Registration().Generate(), sessionId, Actions.LikeYourRevision);
        new UserModelQuery().Add(newLiker);
        var like = new Revisions().Like(revision, newLiker);
        if (like == 202)
        {
            var newLike = new LikeStorageModel()
            {
                PostId = revision.Id,
                UserId = newLiker.Id
            };
            new LikeModelQuery().Add(newLike);
            return "Liked";
        }
        else
            return "Something went wrong";
    }

    public string Dislike(string sessionId)
    {
        var me = new UserModelQuery().Get(sessionId, Actions.Me);
        var myLikers = new UserModelQuery().GetList(sessionId, Actions.LikeYourRevision);
        return "Disliked";
    }
}
