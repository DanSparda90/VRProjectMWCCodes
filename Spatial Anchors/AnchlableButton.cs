using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class AnchlableButton : MonoBehaviour
{
    private bool haveAnchor;
    private PokeInteractable pokeInteract;
    private InteractableUnityEventWrapper eventsWrapper;
    private HandGrabInteractable handInteractable;

    private void Awake()
    {
        pokeInteract = GetComponentInChildren<PokeInteractable>();
        eventsWrapper = GetComponentInChildren<InteractableUnityEventWrapper>();
        handInteractable= GetComponent<HandGrabInteractable>();
    }

    public bool GetHaveAnchor()
    {
        return haveAnchor;
    }

    public void SetHaveAnchor(bool newHaveAnchorState)
    {
        haveAnchor = newHaveAnchorState;
    }

    public void SetButtonEditState(bool isEditModeOn)
    {
        pokeInteract.enabled = !isEditModeOn;
        eventsWrapper.enabled = !isEditModeOn;
        if(handInteractable!= null ) 
            handInteractable.enabled = isEditModeOn;
    }

    public void ShowVisual(bool state)
    {
        GetComponentInChildren<DisablePokeButton>(true).EnableInmediate(state);
    }
}