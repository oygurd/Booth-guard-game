using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public PlayerControlsSO _controlsSo;

    public bool lookingAtInteractable;

    public LayerMask interactionLayerMask;

    private RaycastHit hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _controlsSo.Initialize();
        _controlsSo.Oninteract += CheckInteraction;
    }

    // Update is called once per frame
    void Update()
    {
        lookingAtInteractable =
            Physics.Raycast(transform.position, transform.forward, out hit, 5, interactionLayerMask);
    }

    public void CheckInteraction()
    {
        if (lookingAtInteractable)
        {
            IInteracctable interaction = hit.collider.GetComponent<IInteracctable>();

            interaction?.Interactable();
        }
    }
}