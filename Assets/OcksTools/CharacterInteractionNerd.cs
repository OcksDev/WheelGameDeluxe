using System.Collections;
using UnityEngine;

public class CharacterInteractionNerd : MonoBehaviour
{
    TimedObject to;
    InteractionEntity ie;
    void Start()
    {
        to = GetComponent<TimedObject>();
        ie = GetComponent<InteractionEntity>();
        Gamer.Instance.CharNerds.Add(this);
        StartCoroutine(banana());
    }
    public IEnumerator banana()
    {
        yield return new WaitForFixedUpdate();

        GetComponent<SpriteRenderer>().sprite = to.CharacterLink.WheelImage;
        ie.OnInteract.Append(() => DialogLol.Instance.StartDialog(to.CharacterLink.GetPreferredDialog()));
    }
}
