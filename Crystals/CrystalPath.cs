using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPath : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Rigidbody anchorRb;
    private BoxCollider boxCollider;
    [Space]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] internal bool pausePathing = false;
    [SerializeField] private Vector3 nextTarget;
    private float smoothSpeed = 0.125f;
    public float delayToStart = 3f;
    #endregion

    #region UnityCallbacks
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        anchorRb.gameObject.transform.position = transform.position;

        nextTarget = transform.position;
    }

    private void OnEnable()
    {
        if (boxCollider != null)
            GetRandomPointInsideCollider();
    }
    private void FixedUpdate()
    {
        if (pausePathing) return;
        
        MoveTowardsTarget();
        CheckIfReachedTarget();
    }
    #endregion

    #region Methods
    private void MoveTowardsTarget()
    {
        // anchorRb.velocity = (nextTarget - anchorRb.position).normalized * moveSpeed;
        Vector3 targetVelocity = (nextTarget - anchorRb.position).normalized * moveSpeed;
        anchorRb.velocity = Vector3.Lerp(anchorRb.velocity, targetVelocity, smoothSpeed * Time.deltaTime);
    }

    private void CheckIfReachedTarget()
    {
        if (Vector3.Distance(anchorRb.position, nextTarget) < 0.1f)
            nextTarget = GetRandomPointInsideCollider();
    }

    public Vector3 GetRandomPointInsideCollider()
    {
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );

        return boxCollider.transform.TransformPoint(point);
    }

 
    #endregion
}
