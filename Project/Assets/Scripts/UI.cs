using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject canvasFillImageObject;
    [SerializeField] private Text strokesCount;
    [SerializeField] private Text golfClubEquippedName;
    [SerializeField] private Image golfClubEquippedImage;
    [SerializeField] private GameObject golfClub;
    [SerializeField] private GameObject arrow;

    

    public void ChangeFillImageFill(float newFill)
    {
        fillImage.fillAmount = newFill;
    }

    public void EnableDisableFillImage (bool newState)
    {
        canvasFillImageObject.SetActive(newState);
       
    }

    public void ChangeArrowAngle(float angle)
    {
       arrow.transform.rotation = Quaternion.Euler(0, 0, -angle);
    }

    public void EnableDisableArrow (bool isEnabled)
    {
        arrow.SetActive(isEnabled);
    }
    public void EnableDisableGolfClub(bool newState)
    {
        golfClub.SetActive(newState);
    }
    public void UpdateStrokes (int newStrokes)
    {
        strokesCount.text = "Strokes count: " + newStrokes.ToString();
    }

    public void UpdateGolfClub (Sprite newGolfClubImage, string newGolfClubName)
    {
        golfClubEquippedName.text = newGolfClubName;
        golfClubEquippedImage.sprite = newGolfClubImage;
    }



}
