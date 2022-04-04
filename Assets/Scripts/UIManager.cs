using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterAge;
    public TextMeshProUGUI characterCarrying;

    public TextMeshProUGUI torchLevel;
    public Image torchImage;

    public TextMeshProUGUI distance;

    public CharacterObject currentCharacter;

    public FloatingText floatingTextPrefab;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        HoverCharacter(null);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTorchLevel();
        UpdateDistance();
    }

    void UpdateTorchLevel()
    {
        torchLevel.text = "Torch of Jolintia\n" + Mathf.RoundToInt(Manager.instance.torchLevel).ToString();
        torchImage.fillAmount = Manager.instance.torchLevel / 146f;
    }

    void UpdateDistance()
    {
        distance.text = "Distance To Safety\n<size=150%>" + Mathf.CeilToInt(GameData.distance - Manager.instance.torchHolder.transform.position.x).ToString() + "m";
    }

    public void HoverCharacter(CharacterObject c)
    {
        if (c == null)
        {
            characterName.text = "";
            characterAge.text = "";
            characterCarrying.text = "";
            currentCharacter = null;
            return;
        }

        if (currentCharacter == c)
            return;

        characterName.text = c.character.name;
        characterAge.text = c.character.age.ToString();
        if (c.character.carrying != null)
            characterCarrying.text = "Caring for " + c.character.carrying.name;
        if (c.movement.isCarryingTorch)
            characterCarrying.text = "Torch Bearer";

        currentCharacter = c;
    }

    public void FloatText(string text, Vector3 pos)
    {
        pos += Vector3.up * 0.3f;
        GameObject go = Instantiate(floatingTextPrefab.gameObject, pos, Quaternion.identity);
        FloatingText t = go.GetComponent<FloatingText>();
        t.SetText(text);
    }
}
