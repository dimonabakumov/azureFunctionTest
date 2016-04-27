#load "Entity.csx"
#load "TrackModel.csx"
#load "SongModel.csx"

using System;

public class RevisionModel : Entity
{
    public string Title { get; set; }

    public string Description { get; set; }

    public Guid? ParentId { get; set; }

    public List<TrackModel> Tracks { get; set; }

    public bool IsPublic { get; set; }

    public SongModel Song { get; set; }

    public Guid SampleId { get; set; }
}
