using System;
using UnityEngine;

public class Shelf : MonoBehaviour, IInteractable
{
    ShelfController _shelfController;
    GatheringContainerController _gatheringContainerController;
    void Awake()
    {
        _shelfController = GetComponent<ShelfController>();
    }

    public void Place(ContainerFloor containerFloor)
    {
        if (!CanBePlaced()) return;
        _shelfController.TryPlace(containerFloor);
    }

    public void Take(ContainerFloor containerFloor)
    {
        if (!CanTake(containerFloor)) return;
        _shelfController.TryTake(containerFloor);
    }
    
    public bool CanBePlaced()
    {
        _gatheringContainerController = FindContainerInHand();
        return _gatheringContainerController != null && _shelfController.HasFreeSlot(); 
    }
    public bool CanTake(ContainerFloor containerFloor)
    {
        if (!_shelfController.itemsOnFloor.TryGetValue(containerFloor, out var items)) return false;
        if (items.Count == 0) return false;

        var container = FindContainerInHand();
        if (container != null)
        {
            return container.HasFreeSlot();
        }
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

    public void Interact(PlayerInteractor interactor)
    {
        Debug.Log(gameObject + " Interacting with " + interactor);

        ContainerFloor containerFloor = interactor.GetCurrentContainerFloor();
        if (containerFloor == null)
        {
            Debug.Log("No ContainerFloor found behind shelf.");
            return;
        }

        Place(containerFloor);
    }
}