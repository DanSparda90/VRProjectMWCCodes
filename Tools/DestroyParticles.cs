using System.Collections;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    #region Attributes
    [SerializeField] private float delay = 7f;

    #endregion

    #region Unity Callbacks
    //Destroy itself
    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    #endregion
}