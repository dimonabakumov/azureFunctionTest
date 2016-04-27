#load "Entity.csx"

using System;

public class RegionModel : Entity
{
    public Guid TrackId { get; set; }

    public float StartPosition { get; set; }

    public float EndPosition { get; set; }

    public float SampleOffset { get; set; }

    public float SampleStartPosition { get; set; }

    public Guid SampleId { get; set; }
}
