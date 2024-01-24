using System.Collections;
using UnityEngine;

public class DisablePokeButton : MonoBehaviour
{
    #region Attributes
    [SerializeField] private GameObject visualButton;
    [SerializeField] private float timeToDisableAll = 2f;
    [SerializeField] private bool destroyButton = true;

    #endregion

    #region Methods
    //Called from the button
    public void Disable()
    {
        visualButton.SetActive(false);
        StartCoroutine(DisableElementByTime(timeToDisableAll));    
    }

    public void EnableInmediate(bool state)
    {
        gameObject.SetActive(state);
        visualButton.SetActive(state);
    }

    private IEnumerator DisableElementByTime(float time)
    {
        yield return new WaitForSeconds(time);
        if(destroyButton)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    #endregion
}
