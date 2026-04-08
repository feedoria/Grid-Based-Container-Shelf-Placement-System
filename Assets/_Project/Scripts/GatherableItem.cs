using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GatherableItem : MonoBehaviour, IGatherable, IInteractable
{
    public ItemData itemData;
    public GatherableInventoryItem InventoryItem { get; private set; }
    GatheringContainerController _container;
    public void Gather()
    {
        // Find the container the player is holding
        _container = FindContainerInHand();
        if (_container == null) return;

        _container.TryGather(this);
    }

    public bool CanBeGathered()
    {
        _container = FindContainerInHand();
        return _container != null && _container.HasFreeSlot();
    }

    
    public GatherableItem PlaceInCrate(Vector3 worldPos, GameObject parent)
    {
        //if (itemData?.heldPrefab == null) return;

        var holder = new GameObject("ItemHolder");
        holder.transform.SetParent(parent.transform);
        holder.transform.position = worldPos;          // world space
        holder.transform.localRotation = Quaternion.identity;
        holder.transform.localScale = Vector3.one;

        var held = Instantiate(itemData.heldPrefab, holder.transform);
        held.transform.localPosition = Vector3.zero;
        held.transform.localRotation = Quaternion.Euler(
            itemData.placedRotation.x, 
            itemData.placedRotation.y, 
            itemData.placedRotation.z);
        held.transform.localScale = itemData.heldObjectScale;
        GatherableItem newItem = held.GetComponent<GatherableItem>();
        _container.AddItemToList(newItem);
        
        Destroy(gameObject);
        return newItem;
    }

    public GatherableItem PlaceInShelf(Vector3 worldPos, GameObject parent, 
        GatheringContainerController container, GatherableInventoryItem inventoryItem)
    {
        //if (itemData?.heldPrefab == null) return;
        
        InventoryItem = inventoryItem; // store for later removal

        var holder = new GameObject("ItemHolder");
        holder.transform.SetParent(parent.transform);
        holder.transform.position = worldPos;
        holder.transform.localRotation = Quaternion.identity;
        holder.transform.localScale = Vector3.one;

        var held = Instantiate(itemData.heldPrefab, holder.transform);
        held.transform.localPosition = Vector3.zero + itemData.shelfPositionOffset;
        held.transform.localRotation = Quaternion.Euler(
            itemData.placedRotationShelf.x,
            itemData.placedRotationShelf.y,
            itemData.placedRotationShelf.z);
        Vector3 desiredWorldScale = itemData.heldObjectScale;
        held.transform.localScale = desiredWorldScale;

        GatherableItem newItem = held.GetComponent<GatherableItem>();
        if (newItem != null) newItem.InventoryItem = inventoryItem;
        
        container.RemoveItemFromList(this);
        Destroy(gameObject);

        return newItem;
    }
    
    public void TakeFromShelfToContainer(GatheringContainerController container)
    {
        container.TryGatherFromShelf(this);
    }

    public void TakeFromShelfToHands()
    {
        //Player.Instance.PickUp(this.gameObject);
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

    public void Interact(PlayerInteractor interactor)
    {
        Debug.Log(gameObject + "Interacting with " + interactor);
        Gather();
    }
}