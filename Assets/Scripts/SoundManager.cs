using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource scorePlayer;

    // Start is called before the first frame update
    void Start()
    {
        scorePlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
