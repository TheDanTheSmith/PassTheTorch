using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        GameData.StartNewGame();
        GameData.isStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
            SceneController.LoadNight();

        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
    }
}
