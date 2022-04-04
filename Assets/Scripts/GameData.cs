using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static List<Character> characters = new List<Character>();

    public static int dayNumber = 0;

    public static float distance = 20f;

    public static bool gameOver = false;

    public static int monthsSurvived = 0;

    public static bool isStart = true;

    public static List<string> dayRecaps = new List<string>();
    public static List<string> nightRecaps = new List<string>();


    public static void StartNewGame()
    {
        dayNumber = 0;
        distance = 20f;
        gameOver = false;
        monthsSurvived = 0;
        dayRecaps.Clear();
        nightRecaps.Clear();
        isStart = true;
        characters.Clear();

        Character c1 = new Character();
        c1.name = "John";
        c1.age = Character.Ages.Adult;
        characters.Add(c1);

        Character c2 = new Character();
        c2.name = "Mary";
        c2.age = Character.Ages.Adult;
        characters.Add(c2);

        Character c3 = new Character();
        c3.name = "Max";
        c3.age = Character.Ages.Baby;
        characters.Add(c3);

        Character c4 = new Character();
        c4.name = "Charlotte";
        c4.age = Character.Ages.Teen;
        characters.Add(c4);


        SortCharacters();
    }

    public static void DoNight()
    {
        nightRecaps.Clear();
        distance += 3f;
        dayNumber++;
        foreach (Character c in characters)
        {
            c.carrying = null;
            if (c.age == Character.Ages.Baby || c.age == Character.Ages.Child || c.age == Character.Ages.Teen)
            {
                AgeCharacter(c);
            }
        }

        List<Character> adults = characters.FindAll(x => x.age == Character.Ages.Adult);
        if (adults.Count >= 2)
            NewCharacter();
    }

    public static void AgeCharacter(Character c)
    {
        c.age++;

        if (c.age == Character.Ages.Child)
            AddNightMessage(c.name + " learned to walk.");
        else if (c.age == Character.Ages.Teen)
            AddNightMessage(c.name + " grew stronger.");
        else if (c.age == Character.Ages.Adult)
            AddNightMessage(c.name + " came of age.");
    }

    public static void NewCharacter()
    {
        Character baby = new Character();
        baby.age = Character.Ages.Baby;
        baby.name = NameGenerator.GetName();
        AddNightMessage("Baby " + baby.name + " was born.");
        characters.Add(baby);
    }

    public static void AddNightMessage(string msg)
    {
        nightRecaps.Add(msg);
    }

    public static void AddDayRecapMessage(string msg)
    {
        dayRecaps.Add(msg);
    }

    static void SortCharacters()
    {
        characters.Sort((x, y) => x.age.CompareTo(y.age));
    }
}
