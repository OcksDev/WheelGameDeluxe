using System;
using System.Collections.Generic;
using UnityEngine;

public class TimedObject : MonoBehaviour
{
    public string Character = "";
    [HideInInspector]
    public Character CharacterLink;    
    public TimedActionBased DefaultState;
    public List<TimedAction> bb = new List<TimedAction>();
    public string CurrentDesiredDialog = "";
    private void Start()
    {
        if(Character != "")
        {
            CharacterLink = Gamer.Instance.characterdict[Character];
        }
        if(DefaultState.Position == default)
        {
            DefaultState.Position = transform.position;
        }
        DoAction(DefaultState);
        Gamer.Instance.Nerds.Add(this);
    }

    public void UpdateByActions(Gamer g)
    {
        DoAction(DefaultState);
        foreach (var a in bb)
        {
            switch(a.ActionTime)
            {
                case TimedAction.Times.Repeating:
                    if (g.GameTime == a.StartTime) DoAction(a);
                    if(a.RepeatTime > 0 && g.GameTime > a.StartTime)
                    {
                        if((g.GameTime-a.StartTime)%a.RepeatTime == 0)
                        {
                            DoAction(a);
                        }
                    }
                    break;
                case TimedAction.Times.Before:
                    if (g.GameTime < a.StartTime) DoAction(a);
                    break;
                case TimedAction.Times.AtAndAfter:
                    if (g.GameTime >= a.StartTime) DoAction(a);
                    break;
                case TimedAction.Times.BeforeEvent:
                    if (!g.HasEvented(a.TargetEvent)) DoAction(a);
                    break;
                case TimedAction.Times.AfterEvent:
                    if (g.HasEvented(a.TargetEvent)) DoAction(a);
                    break;
                case TimedAction.Times.BeforeCharacterEvent:
                    if (!CharacterLink.HasEvented(a.TargetEvent)) DoAction(a);
                    break;
                case TimedAction.Times.AfterCharacterEvent:
                    if (CharacterLink.HasEvented(a.TargetEvent)) DoAction(a);
                    break;
            }
        }
    }
    public void DoAction(TimedActionBased a)
    {
        if (!a.IgnoreAction)
        {
            if (a.Position != default) transform.position = a.Position;
            gameObject.SetActive(a.Enabled);
        }
    }

}
[Serializable]
public class TimedAction : TimedActionBased
{
    public long StartTime;
    public long RepeatTime=-1;
    public string TargetEvent;
    public Times ActionTime = Times.Repeating;
    public enum Times
    {
        Repeating,
        Before,
        AtAndAfter,
        BeforeEvent,
        AfterEvent,
        BeforeCharacterEvent,
        AfterCharacterEvent,
    }
}
[Serializable]
public class TimedActionBased
{
    public bool IgnoreAction = false;
    public Vector3 Position = default;
    public bool Enabled = true;
}