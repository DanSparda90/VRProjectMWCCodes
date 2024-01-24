using UnityEngine;

public class CustomAnchor : MonoBehaviour
{
    public ChocolateTaste taste;
    [SerializeField] private GameObject editMenu;

    internal bool followTransform;
    internal Transform transformToFollow;

    private string id;
    private GameObject visualObject;

    private void Awake()
    {
        visualObject = transform.GetChild(0).gameObject;
        editMenu.GetComponentInChildren<Canvas>().worldCamera = IntroSceneController.instance.GetPlayerCam();
    }

    private void Update()
    {
        if (IntroSceneController.instance.currentGameZone == GameZone.TasteSpheres) 
        { 
            if (IntroSceneController.instance.editModeOn && !visualObject.activeSelf)
                Show(true);

            if (!IntroSceneController.instance.editModeOn && visualObject.activeSelf)
            {
                Show(false);
                ShowEditMenu(false);
            }
        }
    }

    public void SetId(ChocolateTaste sphereTaste)
    {
        if (sphereTaste == ChocolateTaste.Sweet)
            id = "sweetAnchor";
        if (sphereTaste == ChocolateTaste.Acid)
            id = "acidAnchor";
        if (sphereTaste == ChocolateTaste.Spicy)
            id = "spicyAnchor";
        if (sphereTaste == ChocolateTaste.None)
            id = "startAnchor";
    }

    public void SetPosition(Transform pointPos)
    {
        transform.position = pointPos.position;
        transform.rotation = pointPos.rotation;
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetConstraint(Transform attachTransform, bool isActive)
    {
        transformToFollow = attachTransform;
        followTransform = isActive;
    }

    public void SaveAnchor()
    {
        string labelPrefX = id + "_posX";
        string labelPrefY = id + "_posY";
        string labelPrefZ = id + "_posZ";
        
        PlayerPrefs.SetFloat(labelPrefX, transform.position.x);
        PlayerPrefs.SetFloat(labelPrefY, transform.position.y);
        PlayerPrefs.SetFloat(labelPrefZ, transform.position.z);
    }

    public void SetTasteSphereToThisAnchor()
    {
        GameObject tasteSphere = IntroSceneController.instance.GetTasteSphere(taste);
        tasteSphere.transform.position = transform.position;
        tasteSphere.GetComponent<TasteSelector>().associatedAnchor = GetComponent<Anchor>();
    }

    public void Show(bool isVisible)
    {
        transform.GetChild(0).gameObject.SetActive(isVisible);
    }

    public void ShowEditMenu()
    {
        editMenu.SetActive(!editMenu.activeSelf);
    }

    public void ShowEditMenu(bool state)
    {
        editMenu.SetActive(state);
    }
}
