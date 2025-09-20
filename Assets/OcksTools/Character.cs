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
                a.Value.LikeScore -= 25;
            }
        }
    }
}
public class GearWheel : Character
{
    public override void Init()
    {
        Name = "Gear Wheel";
        WheelImage = Gamer.Instance.sprites[2];
    }
}
public class CheeseWheel : Character
{
    public override void Init()
    {
        Name = "Cheese Wheel";
        WheelImage = Gamer.Instance.sprites[3];
    }
}
public class OfficeChairWheel : Character
{
    public override void Init()
    {
        Name = "Office Chair Wheel";
        WheelImage = Gamer.Instance.sprites[5];
    }
}
public class BearingWheel : Character
{
    public override void Init()
    {
        Name = "Bearing Wheel";
        WheelImage = Gamer.Instance.sprites[4];
    }
}
public class GoldWheel : Character
{
    public override void Init()
    {
        Name = "Gold Wheel";
        WheelImage = Gamer.Instance.sprites[6];
    }
}
public class RustyWheel : Character
{
    public override void Init()
    {
        Name = "Rusty Wheel";
        WheelImage = Gamer.Instance.sprites[7];
    }
}
public class SawbladeWheel : Character
{
    public override void Init()
    {
        Name = "Sawblade Wheel";
        WheelImage = Gamer.Instance.sprites[8];
    }
}
public class PileOfLegoWheelsWheel : Character
{
    public override void Init()
    {
        Name = "Pile Of Lego Wheels Wheel";
        WheelImage = Gamer.Instance.sprites[9];
    }
}
public class MonsterTruckWheel : Character
{
    public override void Init()
    {
        Name = "Monster Truck Wheel";
        WheelImage = Gamer.Instance.sprites[10];
    }
}
public class WoodWheel : Character
{
    public override void Init()
    {
        Name = "Wood Wheel";
        WheelImage = Gamer.Instance.sprites[11];
    }
}
public class CatWheel : Character
{
    public override void Init()
    {
        Name = "Cat Wheel";
        WheelImage = Gamer.Instance.sprites[12];
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
    public bool SaveMe = true;
    public CanonEvent(string a)
    {
        Name = a;
    }

}
public class NonCanonEvent : CanonEvent
{
    public NonCanonEvent(string aa) : base(aa)
    {
        Name = aa;
        SaveMe = false;
    }
}
[System.Serializable]
public class RelationshipStatus
{
    public string Name;
    public int LikeScore = 0;
}