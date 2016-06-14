using System;

public enum Actions : byte
{
    Me = 0,
    FollowYou = 1,
    UnfollowYou = 2,
    LikeYourRevision = 3,
    CommentYourRevision = 4,
    RequestToJoinBand = 5,
    InviteToBand = 6,
    NewSongInBand = 7,
    NewRevisionBasedOnMy = 8,
    LikeYourShout = 9,
    CommentYourShout = 10,
    InviteToSong = 11,
    JoinedYourBand = 12,
    JoinedYourSong = 13,
    PublishForkedRevision = 14,
}