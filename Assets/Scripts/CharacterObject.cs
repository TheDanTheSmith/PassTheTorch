using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Character character;
    public CharacterMovement movement;

    public CharacterObject childPrefab;
    public CharacterObject teenPrefab;
    public CharacterObject adultPrefab;
    public CharacterObject oldPrefab;

    public Material deathMaterial;

    public bool isDead = false;


    void Update()
    {
        float dist = Vector3.Distance(movement.torch.transform.position, transform.position);

        if (dist > Manager.safetyRadius)
            Kill();
    }


    public CharacterObject ReplaceWithNewCharacterObject(CharacterObject o)
    {
        GameObject go = Instantiate(o.gameObject);
        CharacterObject o2 = go.GetComponent<CharacterObject>();
        o2.character = character;
        go.transform.position = transform.position;

        if (movement.isCarryingTorch)
            o2.movement.isCarryingTorch = true;
        return o2;
    }

    public void GiveTorch()
    {
        movement.torch = GameObject.Find("Torch");
        movement.torch.transform.position = transform.position;
        movement.isCarryingTorch = true;
    }

    public void RemoveTorch()
    {
        movement.isCarryingTorch = false;
    }

    public void Kill()
    {
        if (isDead)
            return;

        Manager.instance.CharacterWasKilled(this);

        movement.enabled = false;
        movement.animator.SetActive(false);
        SpriteRenderer r = movement.animator.GetComponent<SpriteRenderer>();
        r.material = deathMaterial;
        r.color = Color.white;
        UIManager.instance.FloatText("X", transform.position);
        this.enabled = false;
        isDead = true;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.instance.HoverCharacter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HoverCharacter(null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Manager.instance.SwitchTorchHolder(this);
    }
}
