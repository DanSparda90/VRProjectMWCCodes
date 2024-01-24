using System;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAnchorManager : MonoBehaviour
{
    #region Attributes
    public static SpatialAnchorManager instance;
    public CustomSpatialAnchorLoader customAnchorLoader;

    [SerializeField] private Anchor _anchorPrefab;
    public Anchor AnchorPrefab => _anchorPrefab;
    [SerializeField] internal List<CustomAnchor> currentCustomAnchors = new List<CustomAnchor>();

    private GameObject currentGrabbingObj;

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        customAnchorLoader = GetComponent<CustomSpatialAnchorLoader>();
    }

    private void Start()
    {
        LoadAnchors();
    }

    #endregion

    #region Methods
    public void PlaceAnchor(GameObject tasteSphere)
    {
        Anchor anchor = Instantiate(_anchorPrefab, tasteSphere.transform.position, tasteSphere.transform.rotation);
        CustomAnchor newAnchor = anchor.GetComponent<CustomAnchor>();
        TasteSelector sphereTasteCtrl = tasteSphere.GetComponent<TasteSelector>();

        ChocolateTaste taste = sphereTasteCtrl.chocolateTaste;
        newAnchor.taste = taste;
        currentCustomAnchors.Add(newAnchor);
        sphereTasteCtrl.haveAnchor = true;
        sphereTasteCtrl.associatedAnchor = anchor;
    }
    public void PlaceAnchor()
    {
        PlaceAnchor(currentGrabbingObj);
    }

    public int LoadAnchors()
    {
       return customAnchorLoader.LoadAnchorsByUuid();
    }

    public void RemoveAnchor()
    {
        TasteSelector sphereGrabbed = currentGrabbingObj.GetComponent<TasteSelector>();
        if (sphereGrabbed.associatedAnchor != null)
        {
            CustomAnchor grabbedSphereCustomAnchor = sphereGrabbed.associatedAnchor.GetComponent<CustomAnchor>();
            Anchor grabbedSphereAnchor = grabbedSphereCustomAnchor.GetComponent<Anchor>();

            grabbedSphereAnchor.OnEraseButtonPressed();
            currentCustomAnchors.Remove(grabbedSphereCustomAnchor);
            Destroy(sphereGrabbed.associatedAnchor.gameObject);
            sphereGrabbed.associatedAnchor = null;
        }
    }

    public void ResetAnchors()
    {
        PlayerPrefs.DeleteAll();
        IntroSceneController.instance.ResetScene();
    }

    public void SetCurrentGrabbingObject(GameObject currentGrab)
    {
        currentGrabbingObj = currentGrab;
    }

    #endregion
}