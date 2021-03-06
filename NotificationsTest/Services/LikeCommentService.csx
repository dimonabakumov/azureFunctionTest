﻿#load "..\TableStorage\Models\UserStorageModel.csx"
#load "RevisionService.csx"

using System;

public class LikeCommentService
{
    public string Like(string sessionId)
    {
        var revisionId = new RevisionService().CreateIfDoesntExist(sessionId);
        if (revisionId == Guid.Empty)
            return "You need to authorise the user";

        var newLiker = new Authorisation().GetUser(new Registration().Generate(), sessionId, Actions.LikeYourRevision);
        var userQuery = new UserModelQuery();
        userQuery.Add(newLiker);
        var like = new Revisions().Like(revisionId, newLiker.AccessToken);
        if (like == 202)
        {
            userQuery.Update(newLiker.PartitionKey, newLiker.RowKey, revisionId);
            return "Liked";
        }
        else
            return "Something went wrong";
    }

    public string Dislike(string sessionId)
    {
        var myLikers = new UserModelQuery().GetList(sessionId, Actions.LikeYourRevision);
        if (myLikers.Count == 0)
            return "You have no likers";

        var dislike = new Revisions().Dislike(myLikers);
        new UserModelQuery().Delete(myLikers);
        return "Disliked";
    }

    public string LeftComment(string sessionId, bool withMentionMe = false)
    {
        var me = new UserModelQuery().Get(sessionId, Actions.Me);
        if(me != null)
        { 
            var revisionId = new RevisionService().CreateIfDoesntExist(sessionId);
            var newCommenter = new Authorisation().GetUser(new Registration().Generate(), sessionId, Actions.CommentYourRevision);
            var userQuery = new UserModelQuery();
            userQuery.Add(newCommenter);
            var comment = withMentionMe ? new Revisions().LeftComment(revisionId, newCommenter.AccessToken, '@' + me.UserName) : new Revisions().LeftComment(revisionId, newCommenter.AccessToken);
            if (comment != null)
            {
                userQuery.Update(newCommenter.PartitionKey, newCommenter.RowKey, commentId:comment);
                return "Commented";
            }
            else
                return "Something went wrong";
        }
        else
            return "You need to authorise the user";
    }

    public string DeleteComment(string sessionId)
    {
        var myCommenters = new UserModelQuery().GetList(sessionId, Actions.CommentYourRevision);
        if (myCommenters.Count == 0)
            return "Nobody comments your revision";

        var deleteComment = new Revisions().DeleteComment(myCommenters);
        new UserModelQuery().Delete(myCommenters);
        return "Comment deleted";
    }
}
