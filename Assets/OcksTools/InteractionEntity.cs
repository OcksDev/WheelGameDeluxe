using TMPro;
using UnityEngine;

public class InteractionEntity : MonoBehaviour
{
    public string CoolBehoove = "";
    public string BasicStartDialog = "";
    public float Offset = 1;
    public float InterDistSQ = 3;
    [HideInInspector]
    public TextMeshProUGUI Displaytext;
    public OXEvent OnInteract = new OXEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var gg = Instantiate(SpawnSystem.SpawnableDict["Interact"].Object, Tags.refs["GlobalCanvas"].transform);
        gg.SetActive(false);
        Displaytext = gg.GetComponent<TextMeshProUGUI>();
        OnInteract.Append(Coolbehoove);
    }
    bool was = false;
    // Update is called once per frame
    void Update()
    {
        if(PlayerController.Instance == null) return;
        var xx = RandomFunctions.DistNoSQRT(transform.position, PlayerController.Instance.transform.position);
        if(xx <= InterDistSQ && !was)
        {
            Displaytext.gameObject.SetActive(true);
            Displaytext.text = InputManager.keynames[InputManager.gamekeys["interact"][0]];
            Displaytext.transform.position = transform.position + (Vector3.up * Offset);
            was = true;
        }
        else if(xx >= InterDistSQ && was)
        {
            Displaytext.gameObject.SetActive(false);
            was = false;
        }
        if(was && InputManager.IsKeyDown("interact", "Player"))
        {
            OnInteract.Invoke();
        }
    }
    private void OnDisable()
    {
        Displaytext.gameObject.SetActive(false);
        was = false;
    }
    public void Coolbehoove()
    {
        switch (CoolBehoove)
        {
            case "Paper":
                Gamer.Instance.AddEvented(new CanonEvent("PaperFound"));
                break;
        }
        if(BasicStartDialog != null)
        {
            DialogLol.Instance.StartDialog(BasicStartDialog);
        }
    }

}
