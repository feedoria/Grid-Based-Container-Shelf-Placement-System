using System;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] float interactDistance = 3f;
    [SerializeField] LayerMask interactLayerMask = ~0;

    [Header("Highlight")]
    [SerializeField] Color highlightColor = Color.yellow;

    private IInteractable currentInteractable;
    private ContainerFloor currentContainerFloor;

    private Renderer currentRenderer;
    private Color originalColor;

    void Update()
    {
        HandleHover();

        if (Input.GetKeyDown(KeyCode.E))
            TryInteractPlace();

        if (Input.GetKeyDown(KeyCode.F))
            TryInteractTake();
    }

    // ================= INTERACTION =================

    void TryInteractPlace()
    {
        if (currentInteractable == null) return;

        if (currentInteractable is Shelf shelf)
        {
            if (currentContainerFloor == null) return;

            shelf.Place(currentContainerFloor);
            return;
        }

        currentInteractable.Interact(this);
    }

    void TryInteractTake()
    {
        if (currentInteractable == null) return;

        if (currentInteractable is Shelf shelf)
        {
            if (currentContainerFloor == null) return;

            shelf.Take(currentContainerFloor);
        }
    }

    public ContainerFloor GetCurrentContainerFloor()
    {
        return currentContainerFloor;
    }

    // ================= HOVER =================

    void HandleHover()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayerMask))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                if (interactable != currentInteractable)
                {
                    ClearHighlight();

                    currentInteractable = interactable;
                    currentRenderer = hit.collider.GetComponentInParent<Renderer>();

                    if (currentRenderer != null)
                    {
                        originalColor = currentRenderer.material.color;
                        currentRenderer.material.color = highlightColor;
                    }
                }

                TryResolveShelfContainerFloor(ray, hit);
                return;
            }
        }

        ClearHighlight();
    }

    // ================= SHELF DOUBLE RAYCAST =================

    void TryResolveShelfContainerFloor(Ray ray, RaycastHit firstHit)
    {
        currentContainerFloor = null;

        if (firstHit.collider.GetComponentInParent<Shelf>() == null)
            return;

        float remaining = interactDistance - firstHit.distance;
        if (remaining <= 0f) return;

        Vector3 origin2 = ray.origin + ray.direction * (firstHit.distance + 0.05f);

        RaycastHit[] hits = Physics.RaycastAll(origin2, ray.direction, remaining, interactLayerMask);
        if (hits.Length == 0) return;

        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var h in hits)
        {
            if (h.collider == null) continue;

            ContainerFloor floor = h.collider.GetComponent<ContainerFloor>();
            if (floor == null)
                floor = h.collider.GetComponentInParent<ContainerFloor>();

            if (floor != null)
            {
                currentContainerFloor = floor;
                return;
            }
        }
    }

    // ================= HIGHLIGHT =================

    void ClearHighlight()
    {
        if (currentRenderer != null)
            currentRenderer.material.color = originalColor;

        currentRenderer = null;
        currentInteractable = null;
        currentContainerFloor = null;
    }

    // ================= UTILS =================

    public GatheringContainerController GetHeldContainerPublic()
    {
        var player = Player.Instance;
        if (player == null || player.HeldObject == null)
            return null;

        return player.HeldObject.GetComponentInChildren<GatheringContainerController>(true);
    }
}