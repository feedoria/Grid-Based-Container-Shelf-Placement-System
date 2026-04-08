using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShelfController : MonoBehaviour
{
    [SerializeField] int rows, cols; 
    Dictionary<ContainerFloor, GatherableInventoryGrid> _grids = new();
    public Dictionary<ContainerFloor, List<GatherableItem>> itemsOnFloor = new();

    public void TryPlace(ContainerFloor containerFloor)
    {
        Debug.Log("TryPlace floor: " + containerFloor.name + " pos: " + containerFloor.transform.position);
        if (containerFloor == null)
        {
            Debug.LogWarning("TryPlace container floor is null!");
            return;
        }
        var grid = GetOrCreateGrid(containerFloor);
        
        
        var container = FindContainerInHand();
        //GatherableItem newItem = container?.GetItemsInContainerList()?.LastOrDefault();
        var allItems = container?.GetItemsInContainerList();
        GatherableItem newItem = FindBestFitItem(allItems, grid);
        if (allItems == null || allItems.Count == 0)
        {
           Debug.Log("The crate is empty.");
            return;
        }
        if (newItem == null)
        {
            Debug.Log("Can't fit this shelf.");
            return;
        }
        int w = newItem.itemData.slotWidth;
        int h = newItem.itemData.slotHeight;

        var inventoryItem = new GatherableInventoryItem(w, h);

        
        if (!grid.TryAutoPlaceItem(inventoryItem))
        {
            Debug.Log("The shelf is full.");
            return;
        }

        Vector3 spawnPos = containerFloor.GetItemCenter(
            inventoryItem.X, inventoryItem.Y, w, h,
            grid.Columns, grid.Rows
        );

        var placedItem = newItem.PlaceInShelf(spawnPos, this.gameObject, container, inventoryItem);
        if (placedItem != null)
            GetOrCreateItemList(containerFloor).Add(placedItem);
        
    }
    
    public void TryTake(ContainerFloor containerFloor)
    {
        if (containerFloor == null) return;

        var items = GetOrCreateItemList(containerFloor);
        if (items.Count == 0) return;

        var grid = GetOrCreateGrid(containerFloor);
        var item = items.Last();
        items.RemoveAt(items.Count - 1);

        // Free the grid slot
        grid.RemoveItem(item?.InventoryItem);

        var container = FindContainerInHand();
        if (container != null)
        {
            item.TakeFromShelfToContainer(container);
        }
        else
        {
            item.TakeFromShelfToHands();
        }
    }
    GatherableItem FindBestFitItem(List<GatherableItem> items, GatherableInventoryGrid grid)
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            var item = items[i];
            if (item == null || !item) continue;

            int w = item.itemData.slotWidth;
            int h = item.itemData.slotHeight;
            var testItem = new GatherableInventoryItem(w, h);

            if (grid.CanAutoPlace(testItem))
                return item;
        }

        return null;
    }
    
    GatherableItem FindBestFitItemFromLargeToSmall(List<GatherableItem> items, GatherableInventoryGrid grid)
    {
        GatherableItem bestFit = null;
        int bestArea = 0;

        foreach (var item in items)
        {
            if (item == null || !item) continue;
            int w = item.itemData.slotWidth;
            int h = item.itemData.slotHeight;
            var testItem = new GatherableInventoryItem(w, h);

            if (grid.CanAutoPlace(testItem))
            {
                int area = w * h;
                if (area > bestArea)
                {
                    bestArea = area;
                    bestFit = item;
                }
            }
        }

        return bestFit;
    }
    
    List<GatherableItem> GetOrCreateItemList(ContainerFloor floor)
    {
        if (!itemsOnFloor.TryGetValue(floor, out var list))
        {
            list = new List<GatherableItem>();
            itemsOnFloor[floor] = list;
        }
        return list;
    }

    
    GatherableInventoryGrid GetOrCreateGrid(ContainerFloor floor)
    {
        if (!_grids.TryGetValue(floor, out var grid))
        {
            grid = new GatherableInventoryGrid(floor.rows, floor.cols, floor);
            _grids[floor] = grid;
        }
        return grid;
    }

    public bool HasFreeSlot()
    {
        return true;
    }
    
    static GatheringContainerController FindContainerInHand()
    {
        var player = Player.Instance;
        if (player == null) return null;

        if (player.HeldObject != null)
        {
            var c = player.HeldObject.GetComponentInChildren<GatheringContainerController>(true);
            if (c != null) return c;
        }

        if (player.OneHandSlot != null)
        {
            var c = player.OneHandSlot.GetComponentInChildren<GatheringContainerController>(true);
            if (c != null) return c;
        }

        return null;
    }
}