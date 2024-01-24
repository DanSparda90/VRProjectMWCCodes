using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CrystalBehaviour : MonoBehaviour
{
    #region Attributes
    [Header("Physics")]
    [Range(0f,5f)][SerializeField] private float collisionForce = 1f;
    [SerializeField] private float delayToStart = 1f;

    [Header("Rotations")]
    [SerializeField] private float forcedRotationRate = 2f;
    [SerializeField] private float constantRotationForce = 0.2f;

    [Header("Dissolve Parameters")]
    [SerializeField] private float waitDurationToStartDissolve = 0.8f;
    [SerializeField] private float dissolveDuration = 3f;
       
    internal Rigidbody rb;
    internal Collider col;
    internal MeshRenderer meshRend;

    [Header("Particles")]
    [SerializeField] GameObject crystalParticlesPrefabSweet;
    [SerializeField] GameObject crystalParticlesPrefabAcid;
    [SerializeField] GameObject crystalParticlesPrefabSpicy;
    [SerializeField] ChocolateTaste currentTimeline = ChocolateTaste.None;

    private bool particlesInCooldown = false;
    #endregion

    #region UnityCallbacks
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        currentTimeline = SequenceController.Instance.currentTimeline;
    }

    private void OnEnable()
    {
        if(col != null)
            col.enabled = true;

        particlesInCooldown = false;
        StartCoroutine(AddRotationForceByTime(forcedRotationRate));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hand") && !particlesInCooldown) 
        {
            if (!SequenceController.Instance.CrystalCollision()) return;

            InstantiateParticles(collision);
            //Cooldown collision prevention
            StartCoroutine(CollisionCooldown(1.5f));

            SequenceController.Instance.PlayHandInteraction();

            StartDissolveCrystal();
        }
        else if (collision.gameObject.CompareTag("Crystal") && !particlesInCooldown)
        {
            if (!SequenceController.Instance.CrystalCollision()) return;

            //Collision direction + addforce 
            Vector3 collisionDirection = collision.transform.position - transform.position;
            rb.velocity = Vector3.zero;
            rb.AddForce(collisionDirection * -collisionForce, ForceMode.Impulse);

            InstantiateParticles(collision);
            //Cooldown collision prevention
            StartCoroutine(CollisionCooldown(1.5f));

            SequenceController.Instance.PlayCrystalInteraction();
        }
    }

    #endregion

    #region Methods
    public void StartDissolveCrystal()
    {
        StartCoroutine(DissolveCrystal());
    }

    private void InstantiateParticles(Collision collision)
    {
        //Collision direction 
        Vector3 collisionDirection = collision.transform.position - transform.position;
        //Particles instantiation rotation calc
        Quaternion particlesRotation = Quaternion.LookRotation(transform.up, collisionDirection);

        //Instantiate particles
        switch (currentTimeline) 
        {
            case ChocolateTaste.Sweet:
                Instantiate(crystalParticlesPrefabSweet, collision.collider.ClosestPoint(gameObject.transform.position), particlesRotation);
                break;
            case ChocolateTaste.Acid:
                Instantiate(crystalParticlesPrefabAcid, collision.collider.ClosestPoint(gameObject.transform.position), particlesRotation);
                break;
            case ChocolateTaste.Spicy:
                Instantiate(crystalParticlesPrefabSpicy, collision.collider.ClosestPoint(gameObject.transform.position), particlesRotation);
                break;
            default:
                break;
        }
    }

    private IEnumerator CollisionCooldown(float cdTime)
    {
        particlesInCooldown = true;
        yield return new WaitForSeconds(cdTime);
        particlesInCooldown = false;
    }

    internal void DoRandomTorqueForce(Rigidbody rb, float minForce, float maxForce)
    {
        Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
        float randomMagnitude = UnityEngine.Random.Range(minForce, maxForce);

        rb.AddTorque(randomDirection * randomMagnitude);
    }
    internal void DoRandomTorqueForce(Rigidbody rb, float constantForce)
    {
        Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
        rb.AddTorque(randomDirection * constantForce);
    }

    private IEnumerator AddRotationForceByTime(float time)
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(time);
            DoRandomTorqueForce(rb, constantRotationForce);
        }
    }

    private IEnumerator DissolveCrystal()
    {
        float startValue = meshRend.sharedMaterial.GetFloat("_Dissolve");
        
        yield return new WaitForSeconds(waitDurationToStartDissolve);
        
        //meshRend.sharedMaterial.SetInteger("_Boolean_Emission", 1);
        //meshRend.sharedMaterial.SetInteger("_Boolean_Alpha", 1);

        DOTween.To(
            () => meshRend.sharedMaterial.GetFloat("_Dissolve"),
            x => meshRend.sharedMaterial.SetFloat("_Dissolve", x),
            1,
            dissolveDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete( () => Destroy(transform.parent.gameObject) 
        );
    }

    #endregion
}