using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class shopInventory : MonoBehaviour
{
    public bool getItems;
    
    public int maxItemsInShopInventory;

    public GameObject uiHolder;
    public bool uiIsOpen;

    public GameObject shopInventoryParent;

    public List<ItemSO> itemsPool = new List<ItemSO>();
    public List<ItemSO> RandomItems = new List<ItemSO>();

    public List<shopSlot> allSlots = new List<shopSlot>();

    private Image iconImage;
    private TextMeshProUGUI amountText;

    private ItemSO displayedItem;
    private int itemAmount;

    private void Awake()
    {
        allSlots.AddRange(shopInventoryParent.GetComponentsInChildren<shopSlot>());

        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (!getItems)
        {
            getItems = true;
            GetRandomItems();
            getItems = true;
        }
    }

    public void OpenUI()
    {
        uiIsOpen = !uiIsOpen;
        uiHolder.SetActive(uiIsOpen);
    }

    public void GetRandomItems()
    {
        for (int i = 0; i < maxItemsInShopInventory; i++)
        {
            ItemSO item = itemsPool[Random.Range(0, itemsPool.Count)];
            RandomItems.Add(item);
            AddItem(item, item.amountInShop);
            Debug.Log("The item " + item.itemName + " has been added to the shop");

            /*allSlots[i].displayedItem = item;
            allSlots[i].displayedItem.icon = item.icon;
            allSlots[i].displayedItem.amountInShop =  item.amountInShop;*/
            allSlots[i].SetItem(item, item.amountInShop);

            if (i == maxItemsInShopInventory - 1)
                break;
        }
    }

    public void AddItem(ItemSO itemToAdd, int amount)
    {
        int remaining = amount;

        foreach (shopSlot slot in allSlots)
        {
            if (slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.maxStackSize;

                if (currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remaining);

                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    remaining -= amountToAdd;

                    if (remaining <= 0)
                    {
                        return;
                    }
                }
            }
        }

        /*foreach (shopSlot slot in allSlots)
        {
            if (!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remaining);
                slot.SetItem(itemToAdd, amountToPlace);
                remaining -= amountToPlace;
                if (remaining <= 0)
                {
                    return;
                }
            }

            if (remaining > 0)
            {
                Debug.Log("Inventory Is Full, Could Not Add" + remaining + "of" + itemToAdd.itemName);
            }
        }*/
    }

    public void SetItem(ItemSO item, int amount)
    {
        displayedItem = item;
        itemAmount = amount;

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (iconImage == null)
        {
            iconImage = transform.GetChild(0).GetComponent<Image>();
            amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        if (displayedItem != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = displayedItem.icon;
            amountText.text = itemAmount.ToString();
        }
        else
        {
            iconImage.enabled = false;
            amountText.text = "";
        }
    }

    public void Payment(float price)
    {
    }
}