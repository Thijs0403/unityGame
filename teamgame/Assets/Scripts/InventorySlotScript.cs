using UnityEngine;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text amountText;

    public void SetSlot(string itemName, int amount)
    {
        Debug.Log("Setting slot:" + itemName + " x" + amount);
        itemNameText.text = itemName;
        amountText.text = amount.ToString();
    }

    public void ClearSlot()
    {
        itemNameText.text = "";
        amountText.text = "";
    }
}
