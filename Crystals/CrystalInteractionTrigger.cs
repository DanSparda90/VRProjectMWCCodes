using System.Collections;
using UnityEngine;

public class CrystalInteractionTrigger : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Transform attractionPoint;
    [SerializeField] private float distanceToAttract = 0.5f;
    [SerializeField] private GameObject dependentObj;
    private bool particlesInCooldown = false;

    #endregion

    #region Unity Callbacks
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal") && dependentObj.activeSelf)
        {
            other.GetComponent<ObjectToAnchor>().MoveCrystalSystemTo(attractionPoint.position, distanceToAttract);
            CrystalBehaviour crystalBeh = other.GetComponentInChildren<CrystalBehaviour>();
            Rigidbody crystalRb = crystalBeh.GetComponent<Rigidbody>();
            crystalBeh.DoRandomTorqueForce(crystalRb, 0.2f, 0.5f);

            if (!particlesInCooldown) 
            {
                //Void pulse sequence
                SequenceController.Instance.PlayVoidCollisionEffect();
                SequenceController.Instance.PlayTrailInteraction();
                //Cooldown collision prevention
                StartCoroutine(CollisionCooldown(5.5f));
            }
        }
    }

    #endregion

    #region Methods
    private IEnumerator CollisionCooldown(float cdTime)
    {
        particlesInCooldown = true;
        yield return new WaitForSeconds(cdTime);
        particlesInCooldown = false;
    }

    #endregion
}
