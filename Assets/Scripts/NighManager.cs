using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NighManager : MonoBehaviour
{
    public List<string> dayRecapMessages = new List<string>();

    public List<string> nightRecapMessages = new List<string>();

    public List<string> messageQueue = new List<string>();

    public bool displayingMessage = false;

    public TextMeshProUGUI messageLabel;

    // Start is called before the first frame update
    void Start()
    {
        if (GameData.gameOver)
        {
            PrepareGameOver();
            NextMessage();
            return;
        }

        if (GameData.isStart)
        {
            PrepareOpening();
        }




        else
        {
            GameData.DoNight();
            PrepMessages();
        }

        NextMessage();
    }

    void PrepMessages()
    {
        dayRecapMessages = GameData.dayRecaps;
        nightRecapMessages = GameData.nightRecaps;

        messageQueue.Add("The journey was not easy...");
        messageQueue.AddRange(dayRecapMessages);
        messageQueue.Add("Now, we can rest until the threat draws near once again...");
        messageQueue.Add("[Take Shelter]");
        int ran = Random.Range(6, 18);
        GameData.monthsSurvived += ran;
        messageQueue.Add(ran.ToString() + " months passes quietly...");
        messageQueue.AddRange(nightRecapMessages);
        foreach (Character c in GameData.characters)
        {
            if (c.age == Character.Ages.Baby)
                BabyCarryCheck(c);
        }

        messageQueue.Add("Time to step out and into the cold...");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && Time.timeSinceLevelLoad > 1f)
        {
            if (displayingMessage)
                NextMessage();
        }


    }

    void MessagesFinished()
    {
        if (GameData.gameOver)
        {
            SceneController.LoadMenu();
            return;
        }


        messageLabel.text = "";
        displayingMessage = false;
        SceneController.LoadDay();
    }

    public void NextMessage()
    {
        if (messageQueue.Count < 1)
        {
            MessagesFinished();
            return;
        }

        ShowMessage(messageQueue[0]);
        messageQueue.RemoveAt(0);
    }

    void ShowMessage(string msg)
    {
        messageLabel.text = msg;
        displayingMessage = true;
    }

    void BabyCarryCheck(Character baby)
    {
        //Find Vailid Baby Carriers

        List<Character> validCharacters = new List<Character>();
        foreach (Character c in GameData.characters)
        {
            if ((c.age == Character.Ages.Adult || c.age == Character.Ages.Teen))
            {
                validCharacters.Add(c);
            }
        }

        //Find Valid Torch Carriers
        List<Character> validTorchCharacters = new List<Character>();
        foreach (Character c in GameData.characters)
        {
            if ((c.age == Character.Ages.Adult || c.age == Character.Ages.Teen || c.age == Character.Ages.Old))
            {
                validTorchCharacters.Add(c);
            }
        }

        //Pick Torch Character
        Character torchCarrier = null;
        if (validCharacters.Find(c => c.age == Character.Ages.Old) != null)
            torchCarrier = validCharacters.Find(c => c.age == Character.Ages.Old);
        else
            torchCarrier = validTorchCharacters[0];

        if (validCharacters.Contains(torchCarrier))
            validCharacters.Remove(torchCarrier);

        if (validCharacters.Count == 0)
        {
            GameData.characters.Remove(baby);
            messageQueue.Add("With nobody strong enough to carry Baby " + baby.name + ", they must be abandoned.");
        }
        else
            messageQueue.Add("Baby " + baby.name + " must be carried.");
    }


    void PrepareOpening()
    {
        messageQueue.Add("Almost three hundred years ago, humanity lost its battle against The Winter...");
        messageQueue.Add("It came without warning, alive and with purpose, and claimed the lives of all those who could not stay warm...");
        messageQueue.Add("Now, it searches for the last few that remain...");
        messageQueue.Add("The few that claim possession of the single greatest weapon against The Winter...");
        messageQueue.Add("The Torch of Joelintia");
        messageQueue.Add("Its flame cannot burn for long while outside, but all those within its warmth cannot be taken.");
        messageQueue.Add("Their numbers are small now and The Torch, passed down from generation to generation, burns dimmer each day.");
        messageQueue.Add("Each journey threatens the lives of the weakest. The young and the frail.");
        messageQueue.Add("Their defeat to the cold is inevitable, but for the sake of their children and their children's children... they must try.");
        messageQueue.Add("It is time to find new shelter");
    }

    void PrepareGameOver()
    {
        messageQueue.Add("The Torch of Joelintia's flame burned out for the final time.");
        messageQueue.Add("After " + GameData.monthsSurvived.ToString() + " months, humanity's inevitable fall happened in silence.");
        messageQueue.Add("Game Over.");
    }
}
