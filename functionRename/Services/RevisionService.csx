#load "..\ModelsGenerator\Revision.csx"
#load "..\Requests\Revisions.csx"
#load "..\TableStorage\Queries\UserModelQuery.csx"
#load "..\TableStorage\Models\UserStorageModel.csx"
#load "..\Requests\Bands.csx"
#load "..\Requests\Authorisation.csx"

using System;

public class RevisionService
{
	public Guid CreateIfDoesntExist(string sessionId, Guid? parentId = null, Guid? bandId = null)
	{
        var me = new UserModelQuery().Get(sessionId, Actions.Me);
	    if (me == null)
	        return Guid.Empty;
        if (me.RevisionId != null)
            return (Guid)me.RevisionId;
        else
            return Create(me).Id;
    }

    public RevisionModel Create(UserStorageModel author, Guid? parentId = null, Guid? bandId = null)
    {
        var generatedRevision = new Revision().Create(Guid.Parse("C7958729-B599-9D4B-14A6-0001754AE924"), true, parentId);
        var postedRevision = new Revisions().Post(generatedRevision, author.AccessToken);
        new UserModelQuery().Update(author.PartitionKey, author.RowKey, postedRevision.Id, null, null, postedRevision.Song.Id);

        return postedRevision;
    }

    public string NewSongInTheBand(string sessionId)
    {
        var invitation = new InviteService().RequestToJoinBand(sessionId, Actions.NewSongInBand, true);
        if (invitation.Equals("You need to authorise the user"))
            return "You need to authorise the user";
        var NewSongAuthor = new UserModelQuery().Get(sessionId, Actions.NewSongInBand);
        var rootRevision = Create(NewSongAuthor);
        var moveSongToTheBand = new Bands().MoveSongtoTheBand(NewSongAuthor.AccessToken, (Guid)NewSongAuthor.BandId, rootRevision.Song.Id);
        return "Created";
    }

    public string NewRevisionInAnySong(string sessionId)
    {
        var NewSongAuthor = new UserModelQuery().Get(sessionId, Actions.NewSongInBand);
        if (NewSongAuthor == null)
        {
            var newSong = NewSongInTheBand(sessionId);
            if(newSong.Equals("You need to authorise the user"))
                return "You need to authorise the user";
        }

        var newRevision = Create(NewSongAuthor, NewSongAuthor.RevisionId);
        return "Created";
    }

    public string NewRevisionBasedOnMy(string sessionId)
    {
        var me = new UserModelQuery().Get(sessionId, Actions.Me);
        if(me == null)
            return "You need to authorise the user";
        var existingSongAuthor = new UserModelQuery().Get(sessionId, Actions.NewSongInBand);
        if (existingSongAuthor != null)
        {
            var rootRevision = Create(me, existingSongAuthor.RevisionId);
            var revisionBasedOnMine = Create(existingSongAuthor, rootRevision.Id);
            return "Created";
        }
        else
        {
            var newSongInBand = NewSongInTheBand(sessionId);
            existingSongAuthor = new UserModelQuery().Get(sessionId, Actions.NewSongInBand);
            var rootRevision = Create(me, existingSongAuthor.RevisionId);
            var revisionBasedOnMine = Create(existingSongAuthor, rootRevision.Id);
            return "Created";
        }
    }

    public string PublishForkedRevision(string sessionId)
    {
        var myRevisionId = CreateIfDoesntExist(sessionId);
        if (myRevisionId == Guid.Empty)
            return "You need to authorise the user";

        var forker = new UserModelQuery().Get(sessionId, Actions.PublishForkedRevision);
        if(forker == null)
            forker = new Authorisation().GetUser(new Registration().Generate(), sessionId, Actions.PublishForkedRevision);
        var forkedRevisionId = new Revisions().ForkSong(myRevisionId, forker.AccessToken);
        var publicForkedRevision = Create(forker, forkedRevisionId);

        if (publicForkedRevision != null)
            return "Forked and published";
        return "Something went wrong";
    }
}
