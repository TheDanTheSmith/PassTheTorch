using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string name;
    public enum Ages { Baby, Child, Teen, Adult, Old};
    public Ages age;

    public Character carrying;
}
