using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public PlayerControlsSO PlayerControlsSo;
    public GameObject uiHolder;
    public bool uiIsOpen;

    public GameObject hotbarObject;
    public GameObject inventorySlotParent;

    public Image dragIcon;

    public float pickupRange;
    private item lookAtItem;
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer lookedAtRenderer = null;

    private int equippedHotbarIndex = 0;
    public float equippedOpacity = 0.9f;
    public float normalOpacity = 0.5f;
    public Transform hand;
    private GameObject currentHeldItem;

    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> hotbarSlots = new List<Slot>();
    private List<Slot> allslots = new List<Slot>();

    private Slot draggedSlot = null;
    private bool isDragging = false;

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>());
        hotbarSlots.AddRange(hotbarObject.GetComponentsInChildren<Slot>());

        allslots.AddRange(inventorySlots);
        allslots.AddRange(hotbarSlots);

        PlayerControlsSo.Initialize();
        PlayerControlsSo.Oninventory += OpenInventory;
        PlayerControlsSo.Oninteract += Pickup;
        PlayerControlsSo.onEscape += UseEscapeToCloseInventory;
    }


    private void Update()
    {
        DetectedLookedAtItem();
        Pickup();

        StartDrag();
        UpdateDragItemPosition();
        EndDrag();
        
        HandleHotbarSelection();
        HandleDropEquippedItem();
        UpdateHotbarOpacity();
    }

    public void OpenInventory()
    {
        Debug.Log("OpenInventory called");
        uiIsOpen = !uiIsOpen;
        uiHolder.SetActive(uiIsOpen);
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = uiIsOpen;
        FpsCamController.instance.updatingRotation = !uiIsOpen;
    }

    public void UseEscapeToCloseInventory()
    {
        if (uiIsOpen)
        {
            OpenInventory();
        }
    }

    public void AddItem(ItemSO itemToAdd, int amount)
    {
        int remaining = amount;

        foreach (Slot slot in allslots)
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

        foreach (Slot slot in allslots)
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
        }
    }

    private void StartDrag()
    {
        if (PlayerControlsSo.LeftClickAction.IsPressed())
        {
            Slot hovered = GetHoveredSlot();
            if (hovered != null && hovered.HasItem())
            {
                draggedSlot = hovered;
                isDragging = true;

                //show drag item
                dragIcon.sprite = hovered.GetItem().icon;
                dragIcon.color = new Color(1, 1, 1, 0.5f);
                dragIcon.enabled = true;
            }
        }
    }

    private void EndDrag()
    {
        if (isDragging && PlayerControlsSo.LeftClickAction.WasReleasedThisFrame())
        {
            Slot hovered = GetHoveredSlot();
            if (hovered != null)
            {
                HandleDrop(draggedSlot, hovered);

                dragIcon.enabled = false;
                draggedSlot = null;
                isDragging = false;
            }
        }
    }

    private Slot GetHoveredSlot()
    {
        foreach (Slot s in allslots)
        {
            if (s.hovering)
            {
                return s;
            }
        }

        return null;
    }

    private void HandleDrop(Slot from, Slot to)
    {
        if (from == to) return;

        //stacking
        if (to.HasItem() && to.GetItem() == from.GetItem())
        {
            int max = to.GetItem().maxStackSize;
            int space = max - to.GetAmount();

            if (space > 0)
            {
                int move = Mathf.Min(space, from.GetAmount());

                to.SetItem(to.GetItem(), to.GetAmount() + move);
                from.SetItem(from.GetItem(), from.GetAmount() - move);

                if (from.GetAmount() <= 0)
                {
                    from.ClearSlot();
                }

                return;
            }
        }

        //different item
        if (to.HasItem())
        {
            ItemSO tempItem = to.GetItem();
            int tempAmount = to.GetAmount();

            to.SetItem(from.GetItem(), from.GetAmount());
            from.SetItem(tempItem, tempAmount);
            return;
        }

        //empty slot
        to.SetItem(from.GetItem(), from.GetAmount());
        from.ClearSlot();
    }

    private void UpdateDragItemPosition()
    {
        if (isDragging)
        {
            dragIcon.transform.position = Input.mousePosition;
        }
    }

    private void Pickup()
    {
        if (lookedAtRenderer != null &&
            PlayerControlsSo.InteractAction.WasPressedThisFrame()) 
        {
            item item = lookedAtRenderer.GetComponent<item>();
            if (item != null)
            {
                AddItem(item.Item, item.amount);
                Destroy(item.gameObject);
                EquipHeldItem();
            }
        }
    }

    private void DetectedLookedAtItem()
    {
        if (lookedAtRenderer != null)
        {
            lookedAtRenderer.material = originalMaterial;
            lookedAtRenderer = null;
            originalMaterial = null;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            item item = hit.collider.GetComponent<item>();
            if (item != null)
            {
                Renderer rend = item.GetComponent<Renderer>();
                if (rend != null)
                {
                    originalMaterial = rend.material;
                    rend.material = highlightMaterial;
                    lookedAtRenderer = rend;
                }
            }
        }
    }

    private void UpdateHotbarOpacity()
    {
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            Image icon = hotbarSlots[i].GetComponent<Image>();

            if (icon != null)
            {
                icon.color = (i == equippedHotbarIndex) ? new Color(1,1,1,equippedOpacity) : new Color(1, 1, 1, normalOpacity);
            }
        }
    }

    private void HandleHotbarSelection()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                equippedHotbarIndex = i;
                UpdateHotbarOpacity();
                EquipHeldItem();
            }
        }
    }

    private void HandleDropEquippedItem()
    {
        if (!Input.GetKeyDown(KeyCode.Q)) return;

        Slot equippedSlot = hotbarSlots[equippedHotbarIndex];
        
        if (!equippedSlot.HasItem()) return;

        ItemSO itemSO = equippedSlot.GetItem();
        GameObject prefab = itemSO.itemPrefab;
        
        if (prefab == null) return;
        
        GameObject dropped = Instantiate(prefab, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);

        item item = dropped.GetComponent<item>();
        item.Item =  itemSO;
        item.amount = equippedSlot.GetAmount();
        
        equippedSlot.ClearSlot();
        
        EquipHeldItem();
    }

    private void EquipHeldItem()
    {
        if (currentHeldItem != null) Destroy(currentHeldItem);
        
        Slot equippedSlot = hotbarSlots[equippedHotbarIndex];
        if (!equippedSlot.HasItem()) return;
        ItemSO item = equippedSlot.GetItem();
        if (item.handItemPrefab == null) return;
        
        currentHeldItem = Instantiate(item.handItemPrefab, hand);
        currentHeldItem.transform.localPosition = Vector3.zero;
        currentHeldItem.transform.localRotation = Quaternion.identity;
    }
}