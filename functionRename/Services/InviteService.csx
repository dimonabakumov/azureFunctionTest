#load "..\TableStorage\Queries\UserModelQuery.csx"
#load "..\ModelsGenerator\Registration.csx"
#load "..\Requests\Invites.csx"
#load "..\Requests\Bands.csx"
#load "..\Requests\Authorisation.csx"
#load "RevisionService.csx"

using System;

public class InviteService
{
	public string RequestToJoinBand(string sessionId, Actions action = Actions.RequestToJoinBand, bool withAccept = false)
	{
        var me = new UserModelQuery().Get(sessionId, Actions.Me);
	    if (me != null)
	    {
	        var newMember = new Authorisation().GetUser(new Registration().Generate(), sessionId, action);
	        new UserModelQuery().Add(newMember);
	        var invites = new Invites();
	        Guid inviteId;
	        Guid bandId;
	        if (me.BandId.HasValue)
	        {
	            inviteId = invites.PostInvite((Guid) me.BandId, newMember.AccessToken, newMember.RowKey);
	            bandId = (Guid) me.BandId;
	        }
	        else
	        {
	            bandId = new Bands().PostBand(me);
	            new UserModelQuery().Update(me.PartitionKey, me.RowKey, null, null, bandId, null);
	            inviteId = invites.PostInvite(bandId, newMember.AccessToken, newMember.RowKey);
	        }

	        if (withAccept)
	        {
	            invites.AcceptAnInvite(inviteId, me.AccessToken);
	            new UserModelQuery().Update(newMember.PartitionKey, newMember.RowKey, null, null, bandId);
	        }

	        return "Request to join created";
	    }
	    else
            return "You need to authorise the user";
    }

    public string InviteTo(string sessionId, Actions action = Actions.InviteToBand)
    {
        var me = new UserModelQuery().Get(sessionId, Actions.Me);
        if (me != null)
        {
            var owner = new Authorisation().GetUser(new Registration().Generate(), sessionId, action);
            if (action == Actions.InviteToBand)
            {
                var bandId = new Bands().PostBand(owner);
                var inviteId = new Invites().PostInvite(bandId, owner.AccessToken, me.RowKey);
                return "Invitation was sent";
            }
            else if(action == Actions.InviteToSong)
            {
                var newSong = new RevisionService().Create(owner);
                var inviteId = new Invites().PostInvite(newSong.Song.Id, owner.AccessToken, me.RowKey, false);
                return "Invitation was sent";
            }
            else if (action == Actions.JoinedYourBand)
            {
                var invites = new Invites();
                Guid inviteId;
                Guid bandId;
                if (me.BandId.HasValue)
                {
                    inviteId = invites.PostInvite((Guid)me.BandId, me.AccessToken, owner.RowKey);
                    invites.AcceptAnInvite(inviteId, owner.AccessToken);
                }
                else
                {
                    bandId = new Bands().PostBand(me);
                    new UserModelQuery().Update(me.PartitionKey, me.RowKey, null, null, bandId, null);
                    inviteId = invites.PostInvite(bandId, me.AccessToken, owner.RowKey);
                    invites.AcceptAnInvite(inviteId, owner.AccessToken);
                }
                return "Joined your band";
            }
            else
            {
                var invites = new Invites();
                Guid inviteId;
                Guid songId;
                if (me.SongId.HasValue)
                {
                    inviteId = invites.PostInvite((Guid)me.SongId, me.AccessToken, owner.RowKey);
                    invites.AcceptAnInvite(inviteId, owner.AccessToken);
                }
                else
                {
                    songId = new RevisionService().Create(me).Song.Id;
                    new UserModelQuery().Update(me.PartitionKey, me.RowKey, null, null, null, songId);
                    inviteId = invites.PostInvite(songId, me.AccessToken, owner.RowKey, false);
                    invites.AcceptAnInvite(inviteId, owner.AccessToken);
                }
                return "Joined your song";
            }
        }
        else
            return "You need to authorise the user";
    }
}
