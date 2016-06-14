#r "Microsoft.WindowsAzure.Storage"

#load "..\..\Configuration\Actions.csx"

using Microsoft.WindowsAzure.Storage.Table;
using System;

public class UserStorageModel : TableEntity
{
    public string Password { get; set; }

    public string UserName { get; set; }

    public string AccessToken { get; set; }

    public string Actions { get; set; }

    public DateTimeOffset ExpiryDate { get; set; }

    public string RefreshToken { get; set; }

    public Guid? BandId { get; set; }

    public Guid? RevisionId { get; set; }

    public long? CommentId { get; set; }

    public Guid? InviteId { get; set; }

    public Guid? SongId { get; set; }

    public Guid? PostId { get; set; }
}
