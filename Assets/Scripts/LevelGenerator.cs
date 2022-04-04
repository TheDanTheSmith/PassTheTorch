using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int levelSize;

    public List<BackgroundObject> backgroundObjects = new List<BackgroundObject>();
    public List<ForegroundObject> foregroundObjects = new List<ForegroundObject>();

    [System.Serializable]
    public class BackgroundObject
    {
        public GameObject go;
        public float repeatEveryX = 30f;
        public float yPos;
        public float zPos;
    }

    [System.Serializable]
    public class ForegroundObject
    {
        public string name;
        public GameObject go;
        public int minAmount = 1;
        public int maxAmount = 5;
        public float yPos;
        public float zPos;
    }

    public GameObject shelter;

    // Start is called before the first frame update
    void Start()
    {
        PopulateLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PopulateLevel()
    {

        foreach (BackgroundObject o in backgroundObjects)
        {
            float currentX = Random.Range(-10f, -5f);
            while (currentX < levelSize)
            {
                SpawnObject(o.go, currentX, o.yPos, o.zPos);
                currentX += o.repeatEveryX;
            }
        }

        List<int> chosenPositions = new List<int>();

        foreach (ForegroundObject o in foregroundObjects)
        {
            int amountToSpawn = Random.Range(o.minAmount, o.maxAmount + 1);

            for (int i = 0; i < amountToSpawn; i++)
            {
                int chosenX = Random.Range(0, (int)levelSize);
                while (chosenPositions.Contains(chosenX))
                    chosenX = Random.Range(0, (int)levelSize);
                SpawnObject(o.go, (float)chosenX, o.yPos, o.zPos);
                chosenPositions.Add(chosenX);
            }
        }

        Instantiate(shelter, new Vector3(-4, 1.8f, 0f), Quaternion.identity);
        Instantiate(shelter, new Vector3(GameData.distance + 2.5f, 1.8f, 0f), Quaternion.identity);

        void SpawnObject(GameObject go, float xPos, float yPos, float zPos)
        {
            Instantiate(go, new Vector3(xPos, yPos, zPos), Quaternion.identity);
        }
    }
}
