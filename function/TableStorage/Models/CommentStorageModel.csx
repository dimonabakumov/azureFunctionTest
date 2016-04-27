#r "Microsoft.WindowsAzure.Storage"

using Microsoft.WindowsAzure.Storage.Table;
using System;

public class CommentStorageModel : TableEntity
{
    public long CommentId { get; set; }

    public Guid PostId { get; set; }

    public Guid CreatorId { get; set; }
}
