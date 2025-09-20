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
    public static Gamer Instance;
    private void Awake()
    {
        Instance = this;
        characters.Clear();
        characters.Add(new MainCharacter());
        characters.Add(new UnicycleWheel());
        characters.Add(new MonsterTruckWheel());
        characters.Add(new BearingWheel());
        characters.Add(new CatWheel());
        characters.Add(new GoldWheel());
        characters.Add(new RustyWheel());
        characters.Add(new CheeseWheel());
        characters.Add(new GearWheel());
        characters.Add(new OfficeChairWheel());
        characters.Add(new PileOfLegoWheelsWheel());
        characters.Add(new WoodWheel());
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
        UpdateMenus();
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
    
    public void AddEvented(CanonEvent a)
    {
        CanonEvents.Add(a.Name, a);
    }
    public bool[] checks = new bool[20];
    public void SetMainMenu(bool a)
    {
        checks[0] = a;
        if (a)
        {
            StartCoroutine(Mainmenuanim());
        }
        UpdateMenus();
    }

    public void CloseAllMenus()
    {
        for(int i = 0; i < checks.Length; i++)
        {
            checks[i] = false;
        }
    }

    public void UpdateMenus()
    {
        Tags.refs["MainMenu"].SetActive(checks[0]);
        Tags.refs["MainMenu2"].SetActive(checks[0]);
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

        SoundSystem.Instance.PlaySound(new OXSound("Wheel", 0.5f));

        yield return StartCoroutine(OXLerp.Linear((x) =>
        {
            var a1 = Vector3.Lerp(new Vector3(a.x, y1, a.z), new Vector3(a.x, y2, a.z), RandomFunctions.EaseBounce(x));
            mm.References[0].transform.position = a1;
        }
        ));
        yield return StartCoroutine(OXLerp.Linear((x) =>
        {
            var a1 = Vector3.Lerp(new Vector3(b.x, y1, b.z), new Vector3(b.x, y2, b.z), RandomFunctions.EaseBounce(x));
            mm.References[1].transform.position = a1;
        }
        ));
        yield return StartCoroutine(OXLerp.Linear((x) =>
        {
            var a1 = Vector3.Lerp(new Vector3(c.x, y1, c.z), new Vector3(c.x, y2, c.z), RandomFunctions.EaseBounce(x));
            mm.References[2].transform.position = a1;
        }
        ));
    }

    [Flags]
    public enum DevFlags
    {
        None = 0,
        NoIntro = 1 << 1,
        Test1 = 1 << 2,
    }

}
