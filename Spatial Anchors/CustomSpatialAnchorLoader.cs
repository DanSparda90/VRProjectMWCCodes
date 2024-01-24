// (c) Meta Platforms, Inc. and affiliates. Confidential and proprietary.

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Demonstrates loading existing spatial anchors from storage.
/// </summary>
/// <remarks>
/// Loading existing anchors involves two asynchronous methods:
/// 1. Call <see cref="OVRSpatialAnchor.LoadUnboundAnchors"/>
/// 2. For each unbound anchor you wish to localize, invoke <see cref="OVRSpatialAnchor.UnboundAnchor.Localize"/>.
/// 3. Once localized, your callback will receive an <see cref="OVRSpatialAnchor.UnboundAnchor"/>. Instantiate an
/// <see cref="OVRSpatialAnchor"/> component and bind it to the `UnboundAnchor` by calling
/// <see cref="OVRSpatialAnchor.UnboundAnchor.BindTo"/>.
/// </remarks>
public class CustomSpatialAnchorLoader : MonoBehaviour
{
    [SerializeField]
    OVRSpatialAnchor _anchorPrefab;

    Action<OVRSpatialAnchor.UnboundAnchor, bool> _onLoadAnchor;
    Dictionary<Guid, int> savedTastes= new Dictionary<Guid, int>();

    public int LoadAnchorsByUuid()
    {
        // Get number of saved anchor uuids
        if (!PlayerPrefs.HasKey(Anchor.NumUuidsPlayerPref))
        {
            PlayerPrefs.SetInt(Anchor.NumUuidsPlayerPref, 0);
        }

        var playerUuidCount = PlayerPrefs.GetInt("numUuids");
        Log($"Attempting to load {playerUuidCount} saved anchors.");
        if (playerUuidCount == 0)
            return playerUuidCount;

        var uuids = new Guid[playerUuidCount];
        for (int i = 0; i < playerUuidCount; ++i)
        {
            var uuidKey = "uuid" + i;
            var currentUuid = PlayerPrefs.GetString(uuidKey);
            var currentTaste = PlayerPrefs.GetInt(uuidKey + "_Taste");
            //Debug.Log($">> uuid = {uuidKey} - currentuuid = {currentUuid} - currentTaste = {currentTaste}");
            Log("QueryAnchorByUuid: " + currentUuid);

            uuids[i] = new Guid(currentUuid);
            savedTastes.Add(uuids[i], currentTaste);
        }

        Load(new OVRSpatialAnchor.LoadOptions
        {
            MaxAnchorCount = 100,
            Timeout = 0,
            StorageLocation = OVRSpace.StorageLocation.Local,
            Uuids = uuids,
        });

        return playerUuidCount;
    }

    private void Awake()
    {
        _onLoadAnchor = OnLocalized;
    }

    private void Load(OVRSpatialAnchor.LoadOptions options) => OVRSpatialAnchor.LoadUnboundAnchors(options, anchors =>
    {
        if (anchors == null)
        {
            Log("Query failed.");
            return;
        }
        int index = 0;
        foreach (var anchor in anchors)
        {
            if (anchor.Localized)
            {
                _onLoadAnchor(anchor, true);
            }
            else if (!anchor.Localizing)
            {
                anchor.Localize(_onLoadAnchor);
            }
            index++;
        }

    });

    private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success)
    {
        if (!success)
        {
            Log($"{unboundAnchor} Localization failed!");
            return;
        }

        var pose = unboundAnchor.Pose;
        var spatialAnchor = Instantiate(_anchorPrefab, pose.position, pose.rotation);
        
        unboundAnchor.BindTo(spatialAnchor);
        CustomAnchor customAnchor = spatialAnchor.GetComponent<CustomAnchor>();
        customAnchor.taste = (ChocolateTaste)savedTastes[unboundAnchor.Uuid];
        GameObject associatedObject = null;

        associatedObject = IntroSceneController.instance.GetTasteSphere(customAnchor.taste);
        
        //customAnchor.SetConstraint(associatedObject.transform, false);
        customAnchor.SetTasteSphereToThisAnchor();
        SpatialAnchorManager.instance.currentCustomAnchors.Add(customAnchor);
        

        //if (spatialAnchor.TryGetComponent<Anchor>(out var anchor))
        //{
        //    // We just loaded it, so we know it exists in persistent storage.
        //    anchor.ShowSaveIcon = true;
        //}
    }

    private static void Log(string message) => Debug.Log($"[SpatialAnchorsUnity]: {message}");
}
