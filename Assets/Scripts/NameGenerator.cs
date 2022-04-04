using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class NameGenerator
{

    static TextAsset file;

    static List<string> names = new List<string>();

    static bool init = false;

    static void Init()
    {
        file = Resources.Load<TextAsset>("Names") as TextAsset;

        string s = file.text;

        string[] s2 = s.Split(" - ");
        names.AddRange(s2);

        init = true;
    }
    public static string GetName()
    {
        if (!init)
            Init();

        return names[Random.Range(0, names.Count)];
    }
}
