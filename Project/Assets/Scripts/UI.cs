using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject canvasFillImageObject;
    [SerializeField] private Text strokesCount;



    

    public void ChangeFillImageFill(float newFill)
    {
        fillImage.fillAmount = newFill;
    }

    public void EnableDisableFillImage (bool newState)
    {
        canvasFillImageObject.SetActive(newState);
    }

    public void UpdateStrokes (int newStrokes)
    {
        strokesCount.text = "Strokes count: " + newStrokes.ToString();
    }




}
