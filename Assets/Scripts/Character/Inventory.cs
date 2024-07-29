using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    // 아이템 목록
    public List<Item> items = new List<Item>();
    public List<GameObject> _itemIcon = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject item = transform.Find("InfoCanvas").Find("Item").GetChild(i).gameObject;
            _itemIcon.Add(item);
        }
        AddItem(ItemType.Potion, 2);
        AddItem(ItemType.Scroll, 1);
        AddItem(ItemType.Coin, 1);
        AddItem(ItemType.Candy, 0);
        AddItem(ItemType.Herb, 0);
    }
    void Update()
    {
        PrintInventory();
    }
    // 아이템 추가
    public void AddItem(ItemType itemType, int quantity)
    {
        Item existingItem = items.Find(item => item.itemType == itemType);
        if (existingItem != null)
        {
            existingItem.quantity += quantity;
        }
        else
        {
            items.Add(new Item(itemType, quantity));
        }
    }

    // 아이템 소모
    public bool ConsumeItem(ItemType itemType, int quantity)
    {
        Item existingItem = items.Find(item => item.itemType == itemType);
        if (existingItem != null && existingItem.quantity >= quantity)
        {
            existingItem.quantity -= quantity;
            if (existingItem.quantity == 0)
            {
                //items.Remove(existingItem);
            }
            return true;
        }
        return false; // 실패: 아이템 부족
    }

    // 인벤토리 출력
    public void PrintInventory()
    {
        foreach (var item in items)
        {
            foreach (var icon in _itemIcon)
            {
                if (icon.GetComponent<ItemController>()._itemType == item.itemType)
                {
                    icon.GetComponent<ItemController>().SetItem(item);
                    break;
                }
            }
        }
    }
}