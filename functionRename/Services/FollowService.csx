#load "..\TableStorage\Queries\UserModelQuery.csx"
#load "..\ModelsGenerator\Registration.csx"
#load "..\Requests\Following.csx"
#load "..\Requests\Authorisation.csx"

using System;

public class FollowService
{
	public string Follow(string sessionId)
	{
        var me = new UserModelQuery().Get(sessionId, Actions.Me);
        var newFollower = new Authorisation().GetUser(new Registration().Generate(), sessionId, Actions.FollowYou);
        var follow = new Following().Follow(Guid.Parse(me.RowKey), newFollower);
        if (follow == 204)
        {
            new UserModelQuery().Add(newFollower);
            return "Followed";
        }
        else
            return "Something went wrong";
    }

    public string Unfollow(string sessionId)
    {
        var me = new UserModelQuery().Get(sessionId, Actions.Me);
        var myFans = new UserModelQuery().GetList(sessionId, Actions.FollowYou);
        var unfollow = new Following().Unfollow(Guid.Parse(me.RowKey), myFans);
        if (unfollow == 204)
        {
            new UserModelQuery().Delete(myFans);
            return "Unfollowed";
        }
        else
            return "Something went wrong";
    }
}
