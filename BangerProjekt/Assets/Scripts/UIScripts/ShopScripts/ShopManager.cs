using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Image = UnityEngine.UI.Image;
using Unity.VisualScripting;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [field:SerializeField] private GameObject shopPanel;
    [field:SerializeField] private List<GameObject> itemSpots;
    [field:SerializeField] private Transform table;
    [field:SerializeField] private Transform detailView;
    [field:SerializeField] private GameObject cardPrefab;
    [field:SerializeField] private GameObject itemViewPrefab;
    



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
        PlaceItemsOnShelf(itemsToSend);
        Debug.Log("Trying to place items");

        List<Card> cardsToSend = new List<Card>();
        for(int i = 0; i < 5; i++)
        {
            cardsToSend.Add(layer.PossibleCards[Random.Range(0, layer.PossibleCards.Count)]);
        }
        Debug.Log("Trying to place cards");
        PlaceCardsOnTable(cardsToSend);
    }

        void PlaceCardsOnTable(List<Card> cards)
        {
        Debug.Log("PlacingCards");
        int cardNumber = 0;
            foreach(Card card in cards)
            {
            GameObject GUIcard = Instantiate(cardPrefab, table, false);
            GUIcard.name = "Card_" + ++cardNumber;
            GUIcard.transform.Find("CardBackgroundImage").GetComponent<Image>().sprite = LayerManager.CurrentLayer.CardBackround[(int)card.CardRarity + 1]; //+1 because 0 is the backside
            GUIcard.transform.Find("CardEffectImage").GetComponent<Image>().sprite = card.CardImage;
            GUIcard.transform.Find("CardName").GetComponent<TMP_Text>().SetText(card.Name);
            GUIcard.transform.Find("CardDescription").GetComponent<TMP_Text>().SetText(card.Description);
            GUIcard.transform.AddComponent<ShopHover>();
            GUIcard.GetComponent<ShopHover>().DetailView = detailView;
            GUIcard.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        void PlaceItemsOnShelf(List<Item> items)
    {
        if(items.Count < 4)
        {
            Debug.LogError("Not Enough Items were sent to be set in the Shop (Must be 4 or more)");
            return;
        }

        if (itemSpots.Count < 4)
        {
            Debug.LogError("Not Enough Item Spots in the Shop (Must be 4 or more)");
            return;
        }

        for(int i = 0; i < 4; i++)
        {
            itemSpots[i].GetComponent<Image>().sprite = items[i].icon;
            ShopHover hover = itemSpots[i].GetComponent<ShopHover>();
             if (hover == null)
             {
                 Debug.LogError("Item spot " + itemSpots[i].name + " does not have a ShopHover component");
                 continue;
             }
            hover.Item = items[i];
            hover.DetailView = detailView;
            hover.ItemViewPrefab = itemViewPrefab;
        }
    }
}
