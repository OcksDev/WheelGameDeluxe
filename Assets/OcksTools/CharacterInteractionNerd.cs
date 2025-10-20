using System.Collections;
using UnityEngine;

public class CharacterInteractionNerd : MonoBehaviour
{
    TimedObject to;
    public float offset = 1f;
    void Start()
    {
        to = GetComponent<TimedObject>();
        Gamer.Instance.CharNerds.Add(this);
        StartCoroutine(banana());
    }
    public IEnumerator banana()
    {
        yield return new WaitForFixedUpdate();

        GetComponent<SpriteRenderer>().sprite = to.CharacterLink.WheelImage;
    }
    public string GetPreferredDialog()
    {
        throw new System.Exception("Bad");
    }
}
