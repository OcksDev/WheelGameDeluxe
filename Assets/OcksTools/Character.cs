using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Character 
{
    public string Name;
    public Sprite WheelImage;
    public List<RelationshipStatus> DebugMyRelationships = new List<RelationshipStatus>();
    public Dictionary<string,RelationshipStatus> Ships = new Dictionary<string,RelationshipStatus>();
    public Dictionary<string, CanonEvent> Events = new Dictionary<string, CanonEvent>();

    public virtual void Init()
    {
        Name = "Unknown";
    }


    public void CompileThings()
    {
        Ships.Clear();
        foreach(var a in Gamer.Instance.characters)
        {
            if (a.Name == Name) continue;
            var nw = new RelationshipStatus();
            nw.Name = a.Name;
            Ships.Add(a.Name, nw);

            DebugMyRelationships.Add(nw);
        }
        SetInitalRelations();
    }
    public virtual void SetInitalRelations()
    {

    }
}

public class UnicycleWheel : Character
{
    public override void Init()
    {
        Name = "Unicycle Wheel";
        WheelImage = Gamer.Instance.sprites[1];
    }
    public override void SetInitalRelations()
    {
        foreach(var a in Ships)
        {
            if(a.Key == "MC")
            {
                a.Value.LikeScore += 50;
            }
            else
            {
                a.Value.LikeScore -= 50;
            }
        }
    }
}
public class MainCharacter : Character
{
    public override void Init()
    {
        Name = "MC";
        WheelImage = Gamer.Instance.sprites[0];
    }
}




[System.Serializable]
public class CanonEvent
{
    public string Name;
}
[System.Serializable]
public class RelationshipStatus
{
    public string Name;
    public int LikeScore = 0;
}