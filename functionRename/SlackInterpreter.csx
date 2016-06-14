#load "Services\CreateMe.csx"
#load "Services\LikeCommentService.csx"
#load "Services\FollowService.csx"
#load "Services\InviteService.csx"
#load "Services\ShoutService.csx"
#load "TableStorage\Queries\SlackModelQuery.csx"

#load "Requests\Shouts.csx"

using System;
using System.Web;

public class SlackInterpreter
{
    public object ExtractCommand(string request)
    {
        var coll = System.Web.HttpUtility.ParseQueryString(request);
        var noted = new SlackModelQuery(coll);

        try
        {
            var text = coll.Get("text").Trim('\'').Split(' ');
            var sessionId = coll.Get("user_id").Trim('\'');
            return ExecCommand(int.Parse(text[1]), sessionId);
        }
        catch (Exception ex)
        {
            return new {exeprion = ex};
        }
    }

    public object ExecCommand(int digitCommand, string sessionId)
    {
        switch (digitCommand)
        {
            // Help

            case 0:
                //return new { text = new ShoutService().Create(sessionId) };
                return new { text = "Help is here...\r\n" + "Second line \r\n " };
                break;

            // Reg/Auth user

            case 1:
                var reg = new CreateMe().Auth(sessionId);
                return new { text = "Registered! Username : " + reg.UserName + " Password : " + reg.Password };
                break;

            // Follow me

            case 2:
                return new { text = new FollowService().Follow(sessionId) };
                break;

            // Unfollow me

            case 3:
                return new { text = new FollowService().Unfollow(sessionId) };
                break;

            // Like my revision

            case 4:
                return new { text = new LikeCommentService().Like(sessionId) };
                break;

            // Dislike my revision

            case 5:
                return new { text = new LikeCommentService().Dislike(sessionId) };
                break;

            // Comment my revision

            case 6:
                return new { text = new LikeCommentService().LeftComment(sessionId) };
                break;

            // Delete comment from revision

            case 7:
                return new { text = new LikeCommentService().DeleteComment(sessionId) };
                break;

            // Comment my revision with mintion me

            case 8:
                return new { text = new LikeCommentService().LeftComment(sessionId, true) };
                break;

            // New song in my band

            case 9:
                return new { text = new RevisionService().NewSongInTheBand(sessionId) };
                break;

            // New revision in any song

            case 10:
                return new { text = new RevisionService().NewRevisionInAnySong(sessionId) };
                break;

            // New revision based on my revision

            case 11:
                return new { text = new RevisionService().NewRevisionBasedOnMy(sessionId) };
                break;

            // User sends request to join my band

            case 12:
                return new { text = new InviteService().RequestToJoinBand(sessionId, withAccept: true) };
                break;

           // User invites me in their band

            case 13:
                return new { text = new InviteService().InviteTo(sessionId) };
                break;

            // Like my shout

            case 14:
                return new { text = new ShoutService().Like(sessionId) };
                break;

            // Dislike my shout

            case 15:
                return new { text = new ShoutService().Dislike(sessionId) };
                break;

            // Comment my shout

            case 16:
                return new { text = new ShoutService().LeftComment(sessionId) };
                break;

            // Delete comment from shout

            case 17:
                return new { text = new ShoutService().DeleteComment(sessionId) };
                break;

            // Comment my shout with mintion me

            case 18:
                return new { text = new ShoutService().LeftComment(sessionId, true) };
                break;

            // User invites me to collaborate

            case 19:
                return new { text = new InviteService().InviteTo(sessionId, Actions.InviteToSong) };
                break;

            // User joined my band

            case 20:
                return new { text = new InviteService().InviteTo(sessionId, Actions.JoinedYourBand) };
                break;

            // User joined my song

            case 21:
                return new { text = new InviteService().InviteTo(sessionId, Actions.JoinedYourSong) };
                break;

            // User fork my revision, create new and publish it

            case 22:
                return new { text = new RevisionService().PublishForkedRevision(sessionId) };
                break;

            // Unknown command

            default:
                return new {text = "Unknown command"};
                break;
        }
    }
}
