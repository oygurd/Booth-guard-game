using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "StorageScriptableObjects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int maxStackSize;
    public GameObject itemPrefab;
    public GameObject handItemPrefab;
    
    public string Description;
}
