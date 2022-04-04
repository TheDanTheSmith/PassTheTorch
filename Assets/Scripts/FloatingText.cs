using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public float life = 5f;
    public float speed = 1f;
    public TextMeshPro text;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Floating text");
    }

    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        transform.position += Vector3.up * speed * Time.deltaTime;

        text.color = new Color(text.color.r, text.color.g, text.color.b, life / 1f);

        if (life < 0f)
            Destroy(gameObject);
    }

    public void SetText(string txt)
    {
        text.text = txt;
    }
}
