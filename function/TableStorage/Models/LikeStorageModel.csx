#r "Microsoft.WindowsAzure.Storage"

using Microsoft.WindowsAzure.Storage.Table;
using System;

public class LikeStorageModel : TableEntity
{
    public Guid PostId { get; set; }

    public Guid UserId { get; set; }
}
