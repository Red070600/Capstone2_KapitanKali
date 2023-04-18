using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JRLB
{
    public class ConsumableItemUI : MonoBehaviour
    {
        public Image itemIcon;
        public Text currentAmountText;
        public Text maxAmountText;

        public PlayerInventory playerInventory; // reference to the PlayerInventory script

        private void Awake()
        {
            // Find the PlayerInventory script in the scene and assign it to the reference
            playerInventory = FindObjectOfType<PlayerInventory>();
        }

        private void Update()
        {
            // Retrieve the current consumable from the PlayerInventory and update the UI elements
            currentAmountText.text = "Current: " + playerInventory.currentConsumable.currentItemAmount.ToString();
            maxAmountText.text = "Max: " + playerInventory.currentConsumable.maxItemAmount.ToString();
            itemIcon.gameObject.SetActive(true);
            itemIcon.sprite = playerInventory.currentConsumable.itemIcon;
        }
    }
}
