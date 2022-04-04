using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public List<Character> debugCharactersAtStart = new List<Character>();

    public List<CharacterObject> characters = new List<CharacterObject>();

    public CharacterObject torchHolder;


    public CharacterObject childPrefab;
    public CharacterObject teenPrefab;
    public CharacterObject adultPrefab;
    public CharacterObject adultCarryingPrefab;
    public CharacterObject oldPrefab;

    public Vector3 startPos;

    public bool reachedEnd = false;

    public float torchLevel = 100f;
    public static float safetyRadius = 3.2f;

    float timeBetweenAgeing = 5f;
    float timeLastAged = 0f;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
        debugCharactersAtStart = GameData.characters;
        if (GameData.isStart)
            GameData.StartNewGame();
        StartDay();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SortQueue();
        torchLevel -= Time.fixedDeltaTime;

        if (torchLevel < 0f && !reachedEnd)
            GameOver();

        if (torchHolder.transform.position.x >= GameData.distance)
        {
            TriggerNight();
        }


        if (Time.time - timeLastAged > timeBetweenAgeing)
        {
            RollAgeCharacters();
            timeLastAged = Time.time;
        }
    }

    void SortQueue()
    {
        characters.Sort((x, y) => x.movement.moveSpeed.CompareTo(y.movement.moveSpeed));
        characters.Reverse();
        if (torchHolder != null)
        {
            characters.Remove(torchHolder);
            characters.Insert(0, torchHolder);
        }
    }

    public float GetTargetX(CharacterObject c)
    {
        float torchHolderX = torchHolder.transform.position.x;
        float offset = .6f * characters.IndexOf(c);

        return torchHolderX - offset;
    }

    public bool ShouldMoveForward(CharacterObject c)
    {
        if (c == torchHolder || characters.Count < 2)
            return true;


        bool isAhead = GetTargetX(c) < c.transform.position.x;

        if (!isAhead)
            return true;

        int index = characters.IndexOf(c);
        if (index == 0)
            return true;

        CharacterObject cInFront = characters[characters.IndexOf(c) - 1];

        if (c.transform.position.x - torchHolder.transform.position.x > -0.5f)
            return false;

        if (cInFront.movement.moveSpeed > c.movement.moveSpeed && cInFront != torchHolder)
            return true;

        return false;

    }

    public bool ShouldAllowOvertake(CharacterObject c)
    {
        if (c.movement.isCarryingTorch)
            return false;

        List<CharacterObject> closeObjects = new List<CharacterObject>();

        foreach (CharacterObject co in characters)
        {
            if (Vector3.Distance(co.transform.position, c.transform.position) < 0.5f)
                if (co.movement.moveSpeed > c.movement.moveSpeed)
                    return true;
        }
        return false;
    }

    public void StartDay()
    {
        GameData.isStart = false;
        GameData.dayRecaps.Clear();
        //Spawn Characters
        foreach (Character character in GameData.characters)
        {
            switch (character.age)
            {
                case Character.Ages.Child:
                    SpawnCharacter(character, childPrefab);
                    break;
                case Character.Ages.Teen:
                    SpawnCharacter(character, teenPrefab);
                    break;
                case Character.Ages.Adult:
                    SpawnCharacter(character, adultPrefab);
                    break;
                case Character.Ages.Old:
                    SpawnCharacter(character, oldPrefab);
                    break;
            }
        }


        //Give Torch
        CharacterObject oldest = null;
        foreach (CharacterObject c in characters)
        {
            if (oldest == null)
                oldest = c;
            else if (c.character.age > oldest.character.age)
                oldest = c;
        }

        if (oldest != null)
            SwitchTorchHolder(oldest);


        //Give Babies
        foreach (Character character in GameData.characters)
        {
            Debug.Log(character.name);
            if (character.age == Character.Ages.Baby)
            {
                Debug.Log("Looking for carrier for baby " + character.name);

                CharacterObject carrier = GetAdultToCarry();
                if (carrier == null)
                {
                    //Leave Baby Behind
                }
                else
                {
                    Debug.Log("Giving " + character.name + " to " + carrier.character.name);
                    characters.Remove(carrier);
                    carrier.gameObject.SetActive(false);
                    carrier = carrier.ReplaceWithNewCharacterObject(adultCarryingPrefab);
                    characters.Add(carrier);
                    carrier.character.carrying = character;
                }
            }
        }

        foreach (CharacterObject c in characters)
            c.movement.moveSpeed += Random.Range(-0.05f, 0.05f);


    }

    CharacterObject GetAdultToCarry()
    {
        List<CharacterObject> validToCarryBaby = new List<CharacterObject>();
        foreach (CharacterObject c in characters)
        {
            if ((c.character.age == Character.Ages.Adult || c.character.age == Character.Ages.Teen) && c.character.carrying == null && c != torchHolder)
            {
                validToCarryBaby.Add(c);
            }
        }
        if (validToCarryBaby.Count > 0)
            return validToCarryBaby[0];
        return null;
    }

    public void SwitchTorchHolder(CharacterObject holder)
    {
        Debug.Log("Giving torch to " + holder.character.name);
        if (holder.character.age != Character.Ages.Adult && holder.character.age != Character.Ages.Teen && holder.character.age != Character.Ages.Old)
            return;
        if (holder.character.carrying != null)
            return;

        if (torchHolder != null)
            torchHolder.RemoveTorch();

        holder.GiveTorch();
        torchHolder = holder;
    }

    void SpawnCharacter(Character character, CharacterObject o)
    {
        GameObject go = Instantiate(o.gameObject);
        CharacterObject o2 = go.GetComponent<CharacterObject>();
        o2.character = character;
        go.transform.position = startPos;
        startPos += Vector3.right * 0.5f;
        characters.Add(o2);
        if (character.age == Character.Ages.Teen)
            o2.transform.position += Vector3.down * 0.2f;
    }

    public void CharacterWasKilled(CharacterObject character)
    {
        if (!characters.Contains(character))
            return;
        if (character.character.age == Character.Ages.Child)
            GameData.AddDayRecapMessage(character.character.name + " was left behind while just a child.");
        else if (character.character.carrying != null)
        {
            GameData.characters.Remove(character.character.carrying);
            GameData.AddDayRecapMessage(character.character.name + " froze to death whilst trying to protect Baby " + character.character.carrying.name);
        }
        else if (character.character.age == Character.Ages.Old)
            GameData.AddDayRecapMessage(character.character.name + " grew too old to keep up.");
        else
            GameData.AddDayRecapMessage(character.character.name + " was taken by the cold.");

        GameData.characters.Remove(character.character);
        characters.Remove(character);
    }

    void RollAgeCharacters()
    {
        if (GameData.dayNumber == 0)
            return;

        foreach (CharacterObject c in characters)
        {
            if (c.character.age == Character.Ages.Adult && c.character.carrying == null)
            {
                float ran = Random.Range(0f, 1f);
                bool ageUp = ran < 1 / 25f;

                if (ageUp)
                {
                    c.character.age++;
                    bool isTorchBearer = c == torchHolder;

                    CharacterObject newC = c.ReplaceWithNewCharacterObject(oldPrefab);

                    c.gameObject.SetActive(false);
                    characters.Remove(c);
                    characters.Add(newC);
                    if (isTorchBearer)
                        SwitchTorchHolder(newC);

                    UIManager.instance.FloatText("Aging", newC.transform.position);
                    break;
                }

            }
        }
    }

    void TriggerNight()
    {
        SceneController.LoadNight();
    }

    void GameOver()
    {
        SceneController.LoadGameOver();
    }
}
