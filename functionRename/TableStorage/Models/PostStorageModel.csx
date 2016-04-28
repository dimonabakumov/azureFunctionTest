#r "Microsoft.WindowsAzure.Storage"

using Microsoft.WindowsAzure.Storage.Table;
using System;

public class PostStorageModel : TableEntity
{
    public Guid Id { get; set; }

    public Guid CreatorId { get; set; }
}
