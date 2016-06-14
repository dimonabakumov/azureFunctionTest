using System;

public class ForkSongModel : Entity
{
    public Guid RevisionId { get; set; }

    public string Name { get; set; }

    public ForkSongModel(Guid revisionId)
    {
        RevisionId = revisionId;
    }
}