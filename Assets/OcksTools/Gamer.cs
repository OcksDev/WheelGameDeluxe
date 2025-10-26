using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Gamer : MonoBehaviour
{
    [EnumFlags]
    public DevFlags DeveloperFlags = DevFlags.None;
    public List<Sprite> sprites = new List<Sprite>();
    public List<Character> characters = new List<Character>();
    public Dictionary<string,Character> characterdict = new Dictionary<string, Character>();
    public static Gamer Instance;
    public long GameTime = 0;
    public List<TimedObject> Nerds = new List<TimedObject>();
    public List<CharacterInteractionNerd> CharNerds = new List<CharacterInteractionNerd>();

    public Dictionary<string,Func<bool>> CustomConditions = new Dictionary<string,Func<bool>>();

    private void Awake()
    {
        GlobalEvent.Append("StartingFreedom", StartingFreedom);
        GlobalEvent.Append("UWheelReveal", UWheelReveal);
        GlobalEvent.Append("ShowPaper", ShowPaper);
        GlobalEvent.Append("HidePaper", HidePaper);
        GlobalEvent.Append("TruckPaper", TruckPaper);
        GlobalEvent.Append("PaperCheck", PaperCheck);
        GlobalEvent.Append("UniHeyPaperCheck", UniHeyPaperCheck);
        GlobalEvent.Append("UniHeyPaperStart", UniHeyPaperStart);

        CustomConditions.Add("UniWheelFindsPaper", () =>
        (HasEvented("TruckTalk") || HasEvented("WagonSpoken")) && !HasEvented("PaperFoundReal"));



        ConsoleLol.ConsoleCommandHook.Append(CreateWheelCommands);
        Instance = this;
        characters.Clear();
        characters.Add(new MainCharacter());
        characters.Add(new UnicycleWheel());
        characters.Add(new MonsterTruckWheel());
        characters.Add(new BearingWheel());
        characters.Add(new PizzaWheel());
        characters.Add(new GoldWheel());
        characters.Add(new RustyWheel());
        characters.Add(new CheeseWheel());
        characters.Add(new GearWheel());
        characters.Add(new OfficeChairWheel());
        characters.Add(new PileOfLegoWheelsWheel());
        characters.Add(new WagonWheel());
        characters.Add(new SawbladeWheel());
        foreach (var a in characters)
        {
            a.Init();
        }
        foreach(var a in characters)
        {
            a.CompileThings();
        }
        CloseAllMenus();
        foreach (var a in characters)
        {
            characterdict.Add(a.Name, a);
        }

        //GlobalEvent.Append("UWheelReveal", UWheelReveal);
    }

    public void ResetAll()
    {
        CameraLol.DisableCamera = false;
        DisablePlayerCamera = false;
        InputManager.ResetLockLevel();
        Tags.refs["Paper"].transform.position = Tags.refs["Pos2"].transform.position;
    }

    private void Start()
    {
        if (DeveloperFlags.HasFlag(DevFlags.NoIntro))
        {
            StartGameLol();
        }
        else
        {
            StartCoroutine(WaitForVideo());
        }
    }

    public void SetGameTime(long a)
    {
        GameTime = a;
        UpdateAllNerds();
    }

    public void UpdateAllNerds()
    {
        foreach (var b in Nerds)
        {
            b.UpdateByActions(this);
        }
    }

    public IEnumerator WaitForVideo()
    {
        var e = Tags.refs["Vigeo_Player"].GetComponent<VideoPlayer>();
        yield return new WaitUntil(() => e.isPlaying);
        yield return new WaitForSeconds((float)e.clip.length);
        //yield return new WaitUntil(() => !e.isPlaying && !e.isPaused);
        StartGameLol();
    }
    public static Dictionary<string, CanonEvent> CanonEvents = new Dictionary<string, CanonEvent>();

    public bool HasEvented(string a)
    {
        return CanonEvents.ContainsKey(a);
    }
    
    public  void RemoveEvented(string a)
    {
        if(CanonEvents.ContainsKey(a)) CanonEvents.Remove(a);
        UpdateAllNerds();
    }
    
    public void AddEvented(CanonEvent a)
    {
        CanonEvents.Add(a.Name, a);
        UpdateAllNerds();
    }
    public void StartGameClick()
    {
        if (HasEvented("RealStarted")) return;
        AddEvented(new NonCanonEvent("RealStarted"));
        StartCoroutine(LoadIntoGame());
    }

    public IEnumerator LoadIntoGame()
    {
        var d = Tags.refs["Fader"].GetComponent<Image>();
        var dd = Color.black;
        dd.a = 0;
        d.color = dd;
        yield return StartCoroutine(OXLerp.Linear((x) =>
        {
            var e = Color.black;
            e.a = x;
            d.color = e;
        }
        ));
        StartNewGame();

        yield return StartCoroutine(OXLerp.Linear((x) =>
        {
            var e = Color.black;
            e.a = 1-x;
            d.color = e;
        }
        ));
        if (DeveloperFlags.HasFlag(DevFlags.SkipToRealStart))
        {
            StartingFreedom();
            yield break;
        }
        yield return new WaitForSeconds(0.5f);
        yield return OXLerp.Linear((x) =>
        {
            var dd = RandomFunctions.EaseInAndOut(x);
            CameraLol.Instance.transform.position = Vector3.Lerp(new Vector3(0, 0, -10), new Vector3(0, -0.6f, -10), dd);
        },2);
        yield return new WaitForSeconds(0.5f);
        DialogLol.Instance.StartDialog("Start");
    }


    public static bool DisablePlayerCamera = false;
    public void StartNewGame()
    {
        CloseAllMenus();

        SpawnSystem.Spawn(new SpawnData("Player")
            .Position(Vector3.zero)
            .ParentFromRef("ObjectHolder")
            );

        InputManager.AddLockLevel("Dialog");
        SetGameTime(0);
        DisablePlayerCamera = true;
        CameraLol.DisableCamera = true;
        Camera.main.orthographicSize = 2;

    }


    public bool[] checks = new bool[20];
    public void SetMainMenu(bool a)
    {
        checks[0] = a;
        if (a)
        {
            if (DeveloperFlags.HasFlag(DevFlags.SkipToRealStart))
            {
                StartGameClick();
            }
            else
            {
                StartCoroutine(Mainmenuanim());
            }
        }
        UpdateMenus();
    }

    public void CloseAllMenus()
    {
        for(int i = 0; i < checks.Length; i++)
        {
            checks[i] = false;
        }
        UpdateMenus();
    }

    public void UpdateMenus()
    {
        Tags.refs["MainMenu"].SetActive(checks[0]);
        Tags.refs["MainMenu2"].SetActive(checks[0]);
        Tags.refs["SexMac"].SetActive(!HasEvented("RealStarted"));
    }

    public void StartGameLol()
    {
        Tags.refs["Vigeo_Player"].GetComponent<VideoPlayer>().Stop();
        Tags.refs["Vigeo"].SetActive(false);
        AddEvented(new NonCanonEvent("Started"));

        SetMainMenu(true);
    }
    public IEnumerator Mainmenuanim()
    {
        var mm = Tags.refs["MainMenu2"].GetComponent<MainMenu>();

        Tags.refs["FartShow"].SetActive(false);

        var a = mm.References[0].transform.position;
        var b = mm.References[1].transform.position;
        var c = mm.References[2].transform.position;
        var y1 = mm.References[3].transform.position.y;
        var y2 = mm.References[4].transform.position.y;

        mm.References[0].transform.position = mm.References[3].transform.position;
        mm.References[1].transform.position = mm.References[3].transform.position;
        mm.References[2].transform.position = mm.References[3].transform.position;

        var d = Tags.refs["Fader"].GetComponent<Image>();
        d.color = Color.black;
        yield return StartCoroutine(OXLerp.Linear((x) =>
        {
            var e = Color.black;
            e.a = 1 - x;
            d.color = e;
        }
        ));
        yield return new WaitForSeconds(0.5f);
        SoundSystem.Instance.PlaySound(new OXSound("Wheel", 1f));
        yield return StartCoroutine(OXLerp.Linear((x) =>
        {
            var a1 = Vector3.Lerp(new Vector3(a.x, y1, a.z), new Vector3(a.x, y2, a.z), RandomFunctions.EaseBounce(x));
            mm.References[0].transform.position = a1;
        }
        ));
        SoundSystem.Instance.PlaySound(new OXSound("Game", 1f));
        yield return StartCoroutine(OXLerp.Linear((x) =>
        {
            var a1 = Vector3.Lerp(new Vector3(b.x, y1, b.z), new Vector3(b.x, y2, b.z), RandomFunctions.EaseBounce(x));
            mm.References[1].transform.position = a1;
        }
        ));
        SoundSystem.Instance.PlaySound(new OXSound("Deluxe", 1f).Pitch(0.9f));
        yield return StartCoroutine(OXLerp.Linear((x) =>
        {
            var a1 = Vector3.Lerp(new Vector3(c.x, y1, c.z), new Vector3(c.x, y2, c.z), RandomFunctions.EaseBounce(x));
            mm.References[2].transform.position = a1;
        }
        ));
        SoundSystem.Instance.PlaySound(new OXSound("fart", 0.8f));
        Tags.refs["FartShow"].SetActive(true);

        a = mm.References[5].transform.position;
        b = mm.References[6].transform.position;

        StartCoroutine(OXLerp.Linear((x) =>
        {
            var a1 = Vector3.LerpUnclamped(b, a, RandomFunctions.EaseOvershoot(x, 3, 3f));
            mm.References[7].transform.position = a1;
        }, 0.7f
        ));
    }

    [Flags]
    public enum DevFlags
    {
        None = 0,
        NoIntro = 1 << 1,
        DialogSkipAllowed = 1 << 2,
        SkipToRealStart = 1 << 3,
    }

    public void CreateWheelCommands()
    {
        ConsoleLol.Instance.Add(new OXCommand("time").Action(
            (x) =>
            {
                SetGameTime(long.Parse(x.com[1]));
            }


            ));
    }

    public void StartingFreedom()
    {
        DisablePlayerCamera = false;
        CameraLol.Instance.targetpos = CameraLol.Instance.transform.position;
        CameraLol.Instance.ppos = CameraLol.Instance.transform.position;
        CameraLol.DisableCamera = false;
        Camera.main.orthographicSize = 4f;
        InputManager.RemoveLockLevel("Dialog");
    }

    public void UWheelReveal()
    {
        Gamer.Instance.StartCoroutine(UWheelReveal2());
    }

    public IEnumerator UWheelReveal2()
    {
        yield return OXLerp.Linear((x) =>
        {
            var dd = RandomFunctions.EaseInAndOut(x);
            CameraLol.Instance.transform.position = Vector3.Lerp(new Vector3(0, -0.6f, -10), new Vector3(2.8f, -1.2f, -10), dd);
            Camera.main.orthographicSize = Mathf.Lerp(2, 4, dd);
        }, 1.2f);
    }
    public void ShowPaper()
    {
        Gamer.Instance.StartCoroutine(ShowPaper2());
    }

    public void TruckPaper()
    {
        if (HasEvented("PaperFoundReal") && !HasEvented("FullPaperExplain")) DialogLol.Instance.StartDialog("TruckPaper");
    }
    public void PaperCheck()
    {
        if (!HasEvented("PaperFoundReal")) 
            DialogLol.Instance.SetVariable("Scene", "WagonExplainPaper");
        else
            DialogLol.Instance.SetVariable("Scene", "WagonNoExplainPaper");
    }
    public void UniHeyPaperCheck()
    {
        if (HasEvented("WagonPaperMention")) 
            DialogLol.Instance.SetVariable("Scene", "UniHeyPaper_A"); //already knows
        else
            DialogLol.Instance.SetVariable("Scene", "UniHeyPaper_B"); //doesn't know
    }
    public void HidePaper()
    {
        Gamer.Instance.StartCoroutine(HidePaper2());
    }
    
    public void UniHeyPaperStart()
    {
        DialogLol.Instance.StartDialog("UniHeyPaper");
    }

    public IEnumerator ShowPaper2()
    {
        yield return OXLerp.Linear((x) =>
        {
            var dd = RandomFunctions.EaseIn(x);
            Tags.refs["Paper"].transform.position = Vector3.Lerp(Tags.refs["Pos2"].transform.position, Tags.refs["Pos1"].transform.position, dd);
        }, 0.75f);
    }
    
    public IEnumerator HidePaper2()
    {
        yield return OXLerp.Linear((x) =>
        {
            var dd = RandomFunctions.EaseOut(x);
            Tags.refs["Paper"].transform.position = Vector3.Lerp(Tags.refs["Pos1"].transform.position, Tags.refs["Pos2"].transform.position, dd);
        }, 0.75f);
    }

}
