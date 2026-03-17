using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    private GameObject shopPanel;

    public static event Action<List<Item>> sendItemToShelf;
    public static event Action<List<Card>> sendCardsToTable;


    private void OnEnable()
    {
        LayerManager.newLayer += NewLayer;
    }
    private void OnDisable()
    {
        LayerManager.newLayer -= NewLayer;
    }
    void Awake()
    {
        shopPanel = GameObject.FindWithTag("Shop");
    }

    public void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
        if (shopPanel.activeSelf)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }
    }

    public void NewLayer()
    {
        
        Layer layer = LayerManager.CurrentLayer;
        Debug.Log(layer.name);
         if (layer == null)
         {
             Debug.LogError("CurrentLayer is null in ShopManager");
             return;
         }
         if (layer.PossibleItems.Count == 0 || layer.PossibleCards.Count == 0)
         {
             Debug.LogError("No possible items or cards in the current layer");
             return;
         }
        List<Item> itemsToSend = new List<Item>();
        for(int i = 0; i < 4; i++)
        {
            itemsToSend.Add(layer.PossibleItems[Random.Range(0, layer.PossibleItems.Count)]);
        }
        Debug.Log("Trying to place items");
        sendItemToShelf?.Invoke(itemsToSend);

        List<Card> cardsToSend = new List<Card>();
        for(int i = 0; i < 5; i++)
        {
            cardsToSend.Add(layer.PossibleCards[Random.Range(0, layer.PossibleCards.Count)]);
        }
        Debug.Log("Trying to place cards");
        sendCardsToTable?.Invoke(cardsToSend); 
        ToggleShop();
    }
}
