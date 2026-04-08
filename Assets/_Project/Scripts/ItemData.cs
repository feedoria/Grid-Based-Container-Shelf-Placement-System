using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "ItemData", menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public SpawnType  spawnType = SpawnType.CloneFromPrefab;
    public GameObject worldPrefab;    // The scene‑spawned object
    public GameObject heldPrefab;     // What you hold in your hand
    public bool isPickupable = true;
    [Header("Holding")]
    public Vector3 holdItemPos = Vector3.zero;
    public Vector3 twoHandHoldLocalPosOffset;
    public Vector3 shelfPositionOffset;
        
        
    [FormerlySerializedAs("handEulerAngles")] 
    public Vector3 placedRotation = Vector3.zero;
    public Vector3 placedRotationShelf = Vector3.zero;
    public Vector3 heldObjectScale = Vector3.zero;
    [Tooltip("Scale after placement in world.")]
    public Vector3 worldObjectScale = Vector3.zero;
    
    [Header("Gathering Settings")]
    public int slotWidth = 1;
    public int slotHeight = 1;

}