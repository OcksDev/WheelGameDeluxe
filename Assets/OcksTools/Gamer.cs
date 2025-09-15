using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamer : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public List<Character> characters = new List<Character>();
    public static Gamer Instance;
    private void Awake()
    {
        Instance = this;
        characters.Clear();
        characters.Add(new MainCharacter());
        characters.Add(new UnicycleWheel());
        foreach(var a in characters)
        {
            a.Init();
        }
        foreach(var a in characters)
        {
            a.CompileThings();
        }
    }
}
