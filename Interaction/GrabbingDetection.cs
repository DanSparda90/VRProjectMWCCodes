using Oculus.Interaction.HandGrab;
using UnityEngine;

public class GrabbingDetection : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        HandGrabInteractor handGrab = GetComponent<HandGrabInteractor>();
        bool editMode = false;

        if (IntroSceneController.instance != null)
            editMode = IntroSceneController.instance.editModeOn;

        if(editMode && handGrab.IsGrabbing && other.gameObject.CompareTag("TasteSphere"))
        {
            //Grabbing            
        }
        else if(editMode && !handGrab.IsGrabbing)
        {
            //Ungrabbing            
        }        
    }   
}
