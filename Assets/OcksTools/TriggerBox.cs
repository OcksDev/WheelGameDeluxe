using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    public string Event;
    public bool RemoveInstead = false;
    private TimedObject bananas;
    void Awake()
    {
        bananas = GetComponent<TimedObject>();
        var dingle = new TimedAction();
        if (RemoveInstead)
        {
            dingle.ActionTime = TimedAction.Times.BeforeEvent;
        }
        else
        {
            dingle.ActionTime = TimedAction.Times.AfterEvent;
        }
        dingle.TargetEvent = Event;
        dingle.Enabled = false;
        bananas.bb.Add(dingle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var gm = collision.gameObject;
        if (gm.tag != "Player") return;
        if (!RemoveInstead)
        {
            Gamer.Instance.AddEvented(new CanonEvent(Event));
        }
        else
        {
            Gamer.Instance.RemoveEvented(Event);
        }
    }

}
