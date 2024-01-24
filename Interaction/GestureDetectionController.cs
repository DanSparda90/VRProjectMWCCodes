using UnityEngine;

public class GestureDetectionController : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Transform rightHandTrans, leftHandTrans;
    [SerializeField] private GameObject correctGestureEffect;
    [SerializeField] private GameObject correctGestureEffectInExperience;

    private int secuenceCorrectCount = 0;

    #endregion

    #region Methods
    public void ControlGestureSecuence()
    {
        secuenceCorrectCount++;
        if(secuenceCorrectCount== 3)
            IntroSceneController.instance.SwitchEditMode();
    }

    public void SecuenceGestureDone(int numGesture)
    {
        switch (numGesture)
        {
            case 1:
                if (secuenceCorrectCount == 0)
                    ControlGestureSecuence();
                else
                    secuenceCorrectCount = 0;
                break;
            case 2:
                if (secuenceCorrectCount == 1)
                    ControlGestureSecuence();
                else
                    secuenceCorrectCount = 0;
                break;
            case 3:
                if (secuenceCorrectCount == 2)
                    ControlGestureSecuence();
                else
                    secuenceCorrectCount = 0;
                break;
        }        

        if(secuenceCorrectCount > 0)
        {
            Transform effectPos;
            if(secuenceCorrectCount == 2)
                effectPos = leftHandTrans;
            else
                effectPos = rightHandTrans;

            Vector3 fxPos = new Vector3(effectPos.position.x, effectPos.position.y, effectPos.position.z);
            Instantiate(correctGestureEffect, fxPos, Quaternion.identity);            
        }
    }

    public void ResetCounter()
    {
        secuenceCorrectCount = 0;
    }

    #endregion
}