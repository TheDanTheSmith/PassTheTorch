using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dan/SpriteAnimation", fileName = "Sprite Animation")]
public class SpriteAnimation : ScriptableObject
{
    public List<Sprite> sprites = new List<Sprite>();
    public int fps = 2;

}
