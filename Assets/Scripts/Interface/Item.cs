using System;

[Serializable]
public class Item
{
    public ItemType itemType;
    public int quantity;

    public Item(ItemType itemType, int quantity)
    {
        this.itemType = itemType;
        this.quantity = quantity;
    }
    public void AddItem()
    {
        quantity++;
    }
    public void ConsumeItem()
    {
        if (quantity > 0)
            quantity--;
    }
}