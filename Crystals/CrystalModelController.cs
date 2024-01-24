using UnityEngine;

public class CrystalModelController : MonoBehaviour
{
    #region Attributes
    [SerializeField] internal Transform meshContainer;
    [SerializeField] private float colliderSizeOffset = 1.5f;
    private BoxCollider[] containerColliders;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        foreach (Transform defaultModel in meshContainer)
            Destroy(defaultModel.gameObject);

        containerColliders = meshContainer.GetComponents<BoxCollider>();
    }

    #endregion

    #region Methods
    public void SetCrystalModel(GameObject crystalModel)
    {
        GameObject crystal = Instantiate(crystalModel, Vector3.zero, Quaternion.identity, meshContainer);
        crystal.transform.localPosition = Vector3.zero;
        MeshRenderer crystalMeshRend = crystal.GetComponent<MeshRenderer>();
        crystalMeshRend.material = new Material(crystalMeshRend.material); //Instance to make unique material for each crystal
        crystalMeshRend.material.SetFloat("_Dissolve", 0);
        meshContainer.GetComponent<CrystalBehaviour>().meshRend = crystalMeshRend;

        SetColliderSize(crystal);
    }

    private void SetColliderSize(GameObject instCrystal)
    {
        foreach (BoxCollider boxCol in containerColliders)
        {
            Bounds newBound;
            newBound = instCrystal.GetComponent<Renderer>().bounds;
            boxCol.bounds.Encapsulate(newBound);
            boxCol.size = newBound.size / colliderSizeOffset;            
        }
    }

    #endregion
}
