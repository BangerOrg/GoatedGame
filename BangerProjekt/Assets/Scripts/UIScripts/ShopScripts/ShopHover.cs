using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;
using Image = UnityEngine.UI.Image;

public class ShopHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field:SerializeField] public Transform DetailView{get; set;}
    [field:SerializeField] public Item Item{get; set;} = null;
    [field:SerializeField] public GameObject ItemViewPrefab{get; set;}


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null)
        {
            GetComponent<Image>().color = Color.gray;
            GameObject itemView = Instantiate(ItemViewPrefab, DetailView, false);
            itemView.transform.Find("NameText").GetComponent<TMP_Text>().SetText(Item.name);
            itemView.transform.Find("DescriptionText").GetComponent<TMP_Text>().SetText(Item.description);
            itemView.transform.Find("StatText").GetComponent<TMP_Text>().SetText(BuildStatString());
            itemView.transform.Find("ItemImage").GetComponent<Image>().sprite = Item.icon;
        }
        else
        {
            GameObject newCard = Instantiate(gameObject,DetailView,false);
            newCard.transform.localScale = new Vector2(4,4);
            Destroy(newCard.GetComponent<ShopHover>());
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {  
        if(Item)GetComponent<Image>().color = Color.white;
        if(DetailView.childCount > 0){
            foreach(Transform child in DetailView.transform)
            { 
            Destroy(child.gameObject);
            }
        }
    }
    private string BuildStatString()
    {
        StringBuilder sb = new StringBuilder();
        AddStat(sb, "Damage", Item.damage);
        AddStat(sb, "Fire Rate", Item.fireRate);
        AddStat(sb, "Defense", Item.defense);
        AddStat(sb, "Health Bonus", Item.healthBonus);
        return sb.ToString();
    }
    private void AddStat(StringBuilder sb, string label, float value, string suffix = "")
    {
        if (value != 0)
        {
            string prefix = value > 0 ? "+" : ""; //Add a + sign for positive values, - is implied for negative values
            sb.AppendLine($"{prefix}{value}{suffix} {label}"); //append to the string builder
        }
    }
}
