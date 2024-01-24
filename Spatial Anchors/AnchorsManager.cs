using System.Collections.Generic;
using UnityEngine;

public class AnchorsManager : MonoBehaviour
{
    #region Attributes
    public CustomAnchor anchorPrefab;
    internal List<CustomAnchor> currentCustomAnchors = new List<CustomAnchor>();
    #endregion

    #region Unity Callbacks

    void Start()
    {
        int numLoadedAnchors = LoadAnchors();

        if (numLoadedAnchors == 0)
        {
            GameObject[] spheres = IntroSceneController.instance.GetTasteSpheres();
            GameObject startBtn = IntroSceneController.instance.GetStartButton();

            foreach (GameObject tasteSphere in spheres)
                InstantiateNewAnchor(tasteSphere);
            
            InstantiateNewAnchor(startBtn.transform);
        }
        else
        {
            if(numLoadedAnchors < 4)
            {
                GameObject[] spheres = IntroSceneController.instance.GetTasteSpheres();
                GameObject startBtn = IntroSceneController.instance.GetStartButton();

                foreach (GameObject tasteSphere in spheres)
                {
                    if (!tasteSphere.GetComponent<TasteSelector>().haveAnchor)
                        InstantiateNewAnchor(tasteSphere);
                }

                if (!startBtn.GetComponent<AnchlableButton>().GetHaveAnchor())
                    InstantiateNewAnchor(startBtn.transform);
                    
            }
        }
    }

    #endregion

    #region Methods
    internal void InstantiateNewAnchor(GameObject tasteSphere)
    {
        CustomAnchor newAnchor = Instantiate(anchorPrefab);
        newAnchor.SetPosition(tasteSphere.transform);
        newAnchor.SetConstraint(tasteSphere.transform, true);
        ChocolateTaste taste = tasteSphere.GetComponent<TasteSelector>().chocolateTaste;
        newAnchor.taste = taste;
        newAnchor.SetId(taste);
        currentCustomAnchors.Add(newAnchor);
        tasteSphere.GetComponent<TasteSelector>().haveAnchor = true;
        IntroSceneController.instance.SetAnchorNameUI(taste);
    }

    internal void InstantiateNewAnchor(Transform objectToAnchor)
    {
        CustomAnchor newAnchor = Instantiate(anchorPrefab);
        newAnchor.SetPosition(objectToAnchor.transform);
        newAnchor.SetConstraint(objectToAnchor.transform, true);
        ChocolateTaste taste = ChocolateTaste.None;
        newAnchor.taste = taste;
        newAnchor.SetId(taste);
        currentCustomAnchors.Add(newAnchor);
        objectToAnchor.GetComponent<AnchlableButton>().SetHaveAnchor(true);
        IntroSceneController.instance.SetAnchorNameUI(taste);
    }

    public void SaveAnchor(ChocolateTaste anchorTaste)
    {
        foreach(CustomAnchor currentAnchor in currentCustomAnchors)
        {
            if (currentAnchor.taste == anchorTaste)
            {
                currentAnchor.SaveAnchor();
                IntroSceneController.instance.SetAnchorSavedUI(anchorTaste);
            }
        }
    }

    public int LoadAnchors()
    {
        int numAnchorsLoaded = 0;

        if(TryCreateAnchor(ChocolateTaste.Sweet))
            numAnchorsLoaded++;
        if (TryCreateAnchor(ChocolateTaste.Acid))
            numAnchorsLoaded++;
        if (TryCreateAnchor(ChocolateTaste.Spicy))
            numAnchorsLoaded++;
        if (TryCreateAnchor(ChocolateTaste.None))
            numAnchorsLoaded++;

        return numAnchorsLoaded;
    }

    private bool TryCreateAnchor(ChocolateTaste anchorTaste)
    {
        string id = "";
        Vector3 pos;

        if (anchorTaste == ChocolateTaste.Sweet)
            id = "sweetAnchor";
        if (anchorTaste == ChocolateTaste.Acid)
            id = "acidAnchor";
        if (anchorTaste == ChocolateTaste.Spicy)
            id = "spicyAnchor";
        if (anchorTaste == ChocolateTaste.None)
            id = "startAnchor";

        if (PlayerPrefs.HasKey(id + "_posX"))
        {
            pos = new Vector3(
                PlayerPrefs.GetFloat(id + "_posX"),
                PlayerPrefs.GetFloat(id + "_posY"),
                PlayerPrefs.GetFloat(id + "_posZ")
            );

            GameObject tasteSphere;
            ChocolateTaste taste;
            if (anchorTaste != ChocolateTaste.None) {
                tasteSphere = IntroSceneController.instance.GetTasteSphere(anchorTaste);
                taste = tasteSphere.GetComponent<TasteSelector>().chocolateTaste;
                tasteSphere.GetComponent<TasteSelector>().haveAnchor = true;
            } else {
                tasteSphere = IntroSceneController.instance.GetStartButton();
                taste = ChocolateTaste.None;
                tasteSphere.GetComponent<AnchlableButton>().SetHaveAnchor(true);
            }
           
            CustomAnchor anchor = Instantiate(anchorPrefab);
            anchor.SetPosition(pos);
            anchor.SetConstraint(tasteSphere.transform, false); 
            anchor.taste = taste;
            anchor.SetId(taste);
            anchor.SetTasteSphereToThisAnchor();
            currentCustomAnchors.Add(anchor);
            
            IntroSceneController.instance.SetAnchorNameUI(taste);
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
}
