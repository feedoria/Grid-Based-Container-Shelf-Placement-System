using UnityEngine;

public class PickupableItem : MonoBehaviour, IInteractable
{
    Player _player;
    private Vector3 _heldObjInitialScale;
    public ItemData itemData;

    private void Start()
    {
        _player = Player.Instance;
    }

    public void Interact(PlayerInteractor interactor)
    {
        GameObject picked = PickUp(itemData);

        if (picked == null)
        {
            Debug.Log("Pickup failed.");
            return;
        }

        Destroy(gameObject);
    }

    public GameObject PickUp(ItemData data)
    {
        if (data == null || data.heldPrefab == null) return null;
        if (_player.OneHandSlot == null) return null;

        var obj = Instantiate(data.heldPrefab, _player.OneHandSlot.transform);
        if (!obj) return null;
        
        _heldObjInitialScale = obj.transform.localScale;

        obj.transform.localPosition = data.holdItemPos;
        obj.transform.localRotation = Quaternion.Euler(data.placedRotation);
        obj.transform.localScale    = data.heldObjectScale;

        // heldObjectRb = obj.GetComponent<Rigidbody>();
        // if (heldObjectRb != null) heldObjectRb.isKinematic = true;

        return obj;
    }
}