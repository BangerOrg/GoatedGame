using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    [field: SerializeField] public Item Item { get; set; }
    [field: SerializeField] public GameObject PopUp { get; set; }
    private SpriteRenderer SR;
    private TMP_Text text;

    private void Awake()
    {
        SR = this.GetComponentInChildren<SpriteRenderer>(true);
        text = this.GetComponentInChildren<TMP_Text>(true);
    }
    public void setup(Item item)
    {
        Item = item;
        SR.sprite = item.Icon;
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            text.SetText($"press '{other.GetComponent<PlayerInput>().actions.FindAction("")}' ");
            PopUp.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PopUp.SetActive(false);
        }
    }

}
