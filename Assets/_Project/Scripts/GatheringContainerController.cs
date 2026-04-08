using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GatheringContainerController : MonoBehaviour
{
    [SerializeField] List<GatherableItem> itemsInContainer = new();
    GatherableInventoryGrid grid0;
    GatherableInventoryGrid grid1;

    ContainerFloor[] _floors;
    
    Dictionary<GatherableItem, GatherableInventoryItem> _inventoryMap = new();
    
    void Awake()
    {
        _floors = GetComponentsInChildren<ContainerFloor>();
        // floors[0] = Floor0, floors[1] = Floor1 (by hierarchy order)

        grid0 = new GatherableInventoryGrid(3, 4, _floors[0]);
        grid1 = new GatherableInventoryGrid(3, 4, _floors[1]);
    }
    
    

    public void TryGather(GatherableItem item)
    {
        int w = item.itemData.slotWidth;
        int h = item.itemData.slotHeight;

        var inventoryItem = new GatherableInventoryItem(w, h);

        ContainerFloor targetFloor;
        GatherableInventoryGrid targetGrid;

        if (grid0.TryAutoPlaceItem(inventoryItem))
        {
            targetFloor = _floors[0];
            targetGrid = grid0;
        }
        else if (grid1.TryAutoPlaceItem(inventoryItem))
        {
            targetFloor = _floors[1];
            targetGrid = grid1;
        }
        else
        {
            // UI should be displayed here 
            Debug.Log("Your crate is full.");
            return;
        }

        // inventoryItem.X/Y are set by TryAutoPlaceItem
        Vector3 spawnPos = targetFloor.GetItemCenter(
            inventoryItem.X, inventoryItem.Y, w, h,
            targetGrid.Columns, targetGrid.Rows   // expose these as public properties
        );
        var newItem = item.PlaceInCrate(spawnPos, this.gameObject);
        if (newItem != null)
            _inventoryMap[newItem] = inventoryItem;
    }
    
    public void TryGatherFromShelf(GatherableItem item)
    {
        if (item == null || !item) return; // destroyed object check
        
        int w = item.itemData.slotWidth;
        int h = item.itemData.slotHeight;

        var inventoryItem = new GatherableInventoryItem(w, h);

        ContainerFloor targetFloor;
        GatherableInventoryGrid targetGrid;

        if (grid0.TryAutoPlaceItem(inventoryItem))
        {
            targetFloor = _floors[0];
            targetGrid = grid0;
        }
        else if (grid1.TryAutoPlaceItem(inventoryItem))
        {
            targetFloor = _floors[1];
            targetGrid = grid1;
        }
        else
        {
            // UI should be displayed here 
            Debug.Log("Your crate is full.");
            return;
        }

        Vector3 spawnPos = targetFloor.GetItemCenter(
            inventoryItem.X, inventoryItem.Y, w, h,
            targetGrid.Columns, targetGrid.Rows
        );

        var holder = item.transform.parent;
        holder.SetParent(this.transform, worldPositionStays: true);
        holder.localScale = Vector3.one;
        holder.localRotation = Quaternion.identity;
        holder.position = spawnPos;

        _inventoryMap[item] = inventoryItem;
        AddItemToList(item);
    }


    public bool HasFreeSlot(int w = 1, int h = 1)
    {
        var testItem = new GatherableInventoryItem(w, h);
        return grid0.CanAutoPlace(testItem) || grid1.CanAutoPlace(testItem);
    }

    public List<GatherableItem> GetItemsInContainerList()
    {
        return itemsInContainer;
    }

    public void RemoveItemFromList(GatherableItem item)
    {
        if (!itemsInContainer.Contains(item)) return;
        itemsInContainer.Remove(item);

        if (_inventoryMap.TryGetValue(item, out var inventoryItem))
        {
            if (!grid0.RemoveItem(inventoryItem))
                grid1.RemoveItem(inventoryItem);
            _inventoryMap.Remove(item);
        }
    }

    public void AddItemToList(GatherableItem item)
    {
        if (!itemsInContainer.Contains(item))
            itemsInContainer.Add(item);
    }
}