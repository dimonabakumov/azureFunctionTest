#r "Microsoft.WindowsAzure.Storage"

#load "..\..\Configuration\Actions.csx"

using Microsoft.WindowsAzure.Storage.Table;
using System;

public class UserStorageModel : TableEntity
{
    public Guid Id { get; set; }

    public string Password { get; set; }

    public string UserName { get; set; }

    public string AccessToken { get; set; }

    public Guid? OwnerId { get; set; }

    public string Actions { get; set; }

    public DateTimeOffset ExpiryDate { get; set; }

    public string RefreshToken { get; set; }
}
