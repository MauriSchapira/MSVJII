using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cameras;
    private int selectedCameraIndex;

    private void Start()
    {
        selectedCameraIndex = 0;   
    }

    public void ChangeCamera(int newCameraIndex)
    {
        if (newCameraIndex == selectedCameraIndex) return;

        cameras[newCameraIndex].SetActive(true);
        cameras[selectedCameraIndex].SetActive(false);
        selectedCameraIndex = newCameraIndex;
    }




}
