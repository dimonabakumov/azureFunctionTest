#load "..\ModelsGenerator\Revision.csx"
#load "..\Requests\Revisions.csx"
#load "..\TableStorage\Queries\UserModelQuery.csx"
#load "..\TableStorage\Models\UserStorageModel.csx"
#load "..\Requests\Bands.csx"
#load "..\Requests\Shouts.csx"

using System;

public class ShoutService
{
    public Guid CreateIfDoesntExist(string sessionId)
    {
        var me = new UserModelQuery().Get(sessionId, Actions.Me);
        if (me.PostId != null)
            return (Guid)me.PostId;

        else
            return Create(me);
    }

    public Guid Create(UserStorageModel author)
    {
        var createdShoutId = new Shouts().Post(author.AccessToken);
        new UserModelQuery().Update(author.PartitionKey, author.RowKey, null, null, null, null, createdShoutId);
        return createdShoutId;
    }

    public string Like(string sessionId)
    {
        var postId = CreateIfDoesntExist(sessionId);
        var newLiker = new Authorisation().GetUser(new Registration().Generate(), sessionId, Actions.LikeYourShout);
        var userQuery = new UserModelQuery();
        userQuery.Add(newLiker);
        var like = new Shouts().Like(postId, newLiker.AccessToken);
        if (like == 202)
        {
            userQuery.Update(newLiker.PartitionKey, newLiker.RowKey, null, null, null, null, postId);
            return "Liked";
        }
        else
            return "Something went wrong";
    }

    public string Dislike(string sessionId)
    {
        var myLikers = new UserModelQuery().GetList(sessionId, Actions.LikeYourShout);
        var dislike = new Shouts().Dislike(myLikers);
        new UserModelQuery().Delete(myLikers);
        return "Disliked";
    }

    public string LeftComment(string sessionId, bool withMentionMe = false)
    {
        var me = new UserModelQuery().Get(sessionId, Actions.Me);
        var shoutId = CreateIfDoesntExist(sessionId);
        var newCommenter = new Authorisation().GetUser(new Registration().Generate(), sessionId, Actions.CommentYourShout);
        var userQuery = new UserModelQuery();
        userQuery.Add(newCommenter);
        var comment = withMentionMe ? new Shouts().LeftComment(shoutId, newCommenter.AccessToken, '@' + me.UserName) : new Shouts().LeftComment(shoutId, newCommenter.AccessToken);
        if (comment != null)
        {
            userQuery.Update(newCommenter.PartitionKey, newCommenter.RowKey, commentId: comment);
            return "Commented";
        }
        else
            return "Something went wrong";
    }

    public string DeleteComment(string sessionId)
    {
        var myCommenters = new UserModelQuery().GetList(sessionId, Actions.CommentYourShout);
        var deleteComment = new Shouts().DeleteComment(myCommenters);
        new UserModelQuery().Delete(myCommenters);
        return "Comment deleted";
    }
}
