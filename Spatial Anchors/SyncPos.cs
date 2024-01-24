using UnityEngine;

/// <summary>
/// Syncronize the position of the spheres to the anchors position if have any
/// </summary>
public class SyncPos : MonoBehaviour
{
    TasteSelector _sphereTeaste;
    TasteSelector sphereTeaste
    {
        get
        {
            if (_sphereTeaste == null)
                _sphereTeaste = GetComponent<TasteSelector>();

            return _sphereTeaste;
        }
    }

    void Update()
    {
        if (sphereTeaste.associatedAnchor != null)
            transform.position = sphereTeaste.associatedAnchor.transform.position;
    }
}