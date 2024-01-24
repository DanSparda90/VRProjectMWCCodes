using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ObjectToAnchor : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Transform anchor;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float attractionMovementDuration = 4f;

    private Rigidbody rb;
    private Transform crystalSystem;
    private Vector3 targetMove;
    private float distanceStop;
    private bool isCrystalAttracted;
    private bool isAttractingCrystal;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        crystalSystem = transform.parent;
    }

    private void FixedUpdate()
    {
        if (isAttractingCrystal)
        {
            if (Vector3.Distance(crystalSystem.position, targetMove) <= distanceStop)
            {
                crystalSystem.DOPause();
                isAttractingCrystal = false;
            }
        }

        rb.MovePosition(Vector3.MoveTowards(rb.position, anchor.position, speed * Time.fixedDeltaTime));     
    }

    #endregion

    #region Methods
    public void MoveAnchorTo(Vector3 newPos)
    {
        anchor.position = newPos;
    }

    public void MoveCrystalSystemTo(Vector3 newPos, float distanceToStop)
    {
        if (!isCrystalAttracted)
        {                  
            targetMove = newPos;
            distanceStop = distanceToStop;

            crystalSystem.DOMove(newPos, attractionMovementDuration);
            isAttractingCrystal = true;
            isCrystalAttracted = true;
            StartCoroutine(CooldownToAttract(3f));
        }
    }

    private IEnumerator CooldownToAttract(float time)
    {
        yield return new WaitForSeconds(time);
        isCrystalAttracted = false;
    }

    #endregion
}
