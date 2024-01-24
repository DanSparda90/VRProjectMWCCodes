using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
    #region Attributes
    private Camera mainPlayerCam;

    #endregion

    #region Unity Callbacks
    private void Start()
    {
        if (IntroSceneController.instance == null)
            mainPlayerCam = Camera.main;
        else
            mainPlayerCam = IntroSceneController.instance.GetPlayerCam();
    }

    void Update()
    {
        transform.LookAt(mainPlayerCam.transform);
        transform.rotation = Quaternion.LookRotation(transform.position - mainPlayerCam.transform.position);
    }

    #endregion
}
