using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class SceneController
{

    public static void LoadDay()
    {
        SceneManager.LoadScene("Day");
    }

    public static void LoadNight()
    {
        SceneManager.LoadScene("Night");
    }

    public static void LoadGameOver()
    {
        GameData.gameOver = true;
        SceneManager.LoadScene("Night");
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
