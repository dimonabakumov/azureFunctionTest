#load "Entity.csx"

using System;

public class MediaCaption : Entity
{
    public string Caption { get; set; }

    public bool AutoPost { get; set; }

    public Clip Clip { get; set; }
}

public class Clip
{
    public Clip(float startPosition, int duration)
    {
        StartPosition = startPosition;
        Duration = duration;
    }
    public float StartPosition { get; set; }

    public int Duration { get; set; }
}