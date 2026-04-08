using UnityEngine;

public class ContainerFloor : MonoBehaviour
{
    [SerializeField] public float slotSize = 0.2f; // world units per slot
    public int rows, cols; 
    // Returns the world-space center for an item placed at (gridX, gridY)
    // with dimensions (w, h)
    public Vector3 GetItemCenter(int gridX, int gridY, int w, int h, int totalCols, int totalRows)
    {
        // Total grid size in world units
        float totalWidth = totalCols * slotSize;
        float totalDepth = totalRows * slotSize;

        // Offset so grid is centered on this transform
        float originX = -totalWidth * 0.5f;
        float originZ = -totalDepth * 0.5f;

        // Center of the item within the grid
        float localX = originX + (gridX + w * 0.5f) * slotSize;
        float localZ = originZ + (gridY + h * 0.5f) * slotSize;

        // Y stays 0 — items sit on the floor plane
        Vector3 localCenter = new Vector3(localX, 0f, localZ);
        return transform.TransformPoint(localCenter);
    }
    
    void OnDrawGizmos()
    {
        if (rows == 0 || cols == 0) return;

        float totalWidth = cols * slotSize;
        float totalDepth = rows * slotSize;

        float originX = -totalWidth * 0.5f;
        float originZ = -totalDepth * 0.5f;

        Gizmos.color = new Color(0f, 1f, 0.5f, 0.4f);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                float localX = originX + (c + 0.5f) * slotSize;
                float localZ = originZ + (r + 0.5f) * slotSize;

                Vector3 center = transform.TransformPoint(new Vector3(localX, 0f, localZ));
                Vector3 size = transform.TransformVector(new Vector3(slotSize * 0.9f, 0.01f, slotSize * 0.9f));

                Gizmos.DrawCube(center, size);
            }
        }

        // Grid border
        Gizmos.color = new Color(0f, 1f, 0.5f, 1f);

        for (int r = 0; r <= rows; r++)
        {
            Vector3 start = transform.TransformPoint(new Vector3(originX, 0f, originZ + r * slotSize));
            Vector3 end   = transform.TransformPoint(new Vector3(originX + totalWidth, 0f, originZ + r * slotSize));
            Gizmos.DrawLine(start, end);
        }

        for (int c = 0; c <= cols; c++)
        {
            Vector3 start = transform.TransformPoint(new Vector3(originX + c * slotSize, 0f, originZ));
            Vector3 end   = transform.TransformPoint(new Vector3(originX + c * slotSize, 0f, originZ + totalDepth));
            Gizmos.DrawLine(start, end);
        }
    }
}