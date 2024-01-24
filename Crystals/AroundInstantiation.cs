using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class AroundInstantiation : MonoBehaviour
{
    #region Attributes
    [SerializeField] private List<GameObject> sweetCrystalPrefabs;
    [SerializeField] private List<GameObject> acidCrystalPrefabs;
    [SerializeField] private List<GameObject> spicyCrystalPrefabs;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform origin;
    [SerializeField] private float spawnRadius;
    [SerializeField] private int numberOfObjects;
    [SerializeField] private float growDuration;
    [SerializeField] private float srinkDuration;
    [SerializeField] private float wait = 1f;

    [SerializeField] private bool appearFromOrigin = true;
    [SerializeField] private bool instantiateOnStart = true;

    [SerializeField] private Ease movementEase = Ease.OutBounce;
    [SerializeField] private Ease growEase = Ease.OutSine;

    [SerializeField] internal ChocolateTaste currentTimeline = ChocolateTaste.None;

    private List<GameObject> instantiatedObjs = new List<GameObject>();
    private float finalScale;

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        SequenceController.OnCurrentTimelineChange += (newTimeline) => currentTimeline = newTimeline;
    }

    void Start()
    {
        if(instantiateOnStart)
            InstantiateAround();        
    }

    private void OnDestroy()
    {
        SequenceController.OnCurrentTimelineChange -= (newTimeline) => currentTimeline = newTimeline;
    }

    #endregion

    #region Methods

    //Called from the timeline too
    public void InstantiateAround()
    {
        StartCoroutine(InstantiateCrystalsByTime(wait));
    }
    
    //Called from the timeline
    public void DestroyInstantiatedElements()
    {
        foreach(GameObject inst in instantiatedObjs)
        {
            if (inst != null)
            {
                inst.transform.DOScale(Vector3.zero, srinkDuration).SetEase(growEase);
                inst.GetComponent<CrystalModelController>().meshContainer.GetComponent<CrystalBehaviour>().StartDissolveCrystal();
            }
        }
    }

    private IEnumerator InstantiateCrystalsByTime(float delay)
    {
        instantiatedObjs.Clear();

        yield return new WaitForSeconds(delay);

        for (int i = 0; i < numberOfObjects; i++)
        {
            //Delay between crystals
            yield return new WaitForSeconds(Random.Range(1f, 10f));

            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 pos = origin.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRadius;
            Vector3 originPos;

            if (appearFromOrigin)
                originPos = new Vector3(origin.position.x, origin.position.y - 0.3f, origin.position.z);
            else
                originPos = pos;

            GameObject spawnedObject = Instantiate(prefab, originPos, Quaternion.identity);
            spawnedObject.GetComponent<CrystalModelController>().SetCrystalModel(GetRandomCrystal());

            finalScale = spawnedObject.transform.localScale.x;
            spawnedObject.transform.localScale = Vector3.zero;
            spawnedObject.transform.DOMove(pos, growDuration).SetEase(movementEase);
            spawnedObject.transform.DOScale(Vector3.one * finalScale, growDuration).SetEase(growEase);
            instantiatedObjs.Add(spawnedObject);

            CrystalBehaviour crystalBeh = spawnedObject.GetComponentInChildren<CrystalBehaviour>();
            Rigidbody spawnedRb = crystalBeh.GetComponent<Rigidbody>();

            crystalBeh.DoRandomTorqueForce(spawnedRb, 2f, 5f);
        }
    }

    private GameObject GetRandomCrystal()
    {
        switch (currentTimeline) 
        {
            case ChocolateTaste.Sweet:
                return sweetCrystalPrefabs[Random.Range(0, sweetCrystalPrefabs.Count)];
            case ChocolateTaste.Acid:
                return acidCrystalPrefabs[Random.Range(0, acidCrystalPrefabs.Count)];
            case ChocolateTaste.Spicy:
                return spicyCrystalPrefabs[Random.Range(0, spicyCrystalPrefabs.Count)];
            default:
                return sweetCrystalPrefabs[0];
        }
    }

    #endregion
}
