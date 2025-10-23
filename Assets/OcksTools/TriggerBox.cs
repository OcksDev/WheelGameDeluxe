using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    public string Event;
    private TimedObject bananas;
    void Awake()
    {
        bananas = GetComponent<TimedObject>();
        var dingle = new TimedAction();
        dingle.ActionTime = TimedAction.Times.AfterEvent;
        dingle.TargetEvent = Event;
        dingle.Enabled = false;
        bananas.bb.Add(dingle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Gamer.Instance.AddEvented(new CanonEvent(Event));
    }

}
