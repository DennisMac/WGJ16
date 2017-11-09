using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public enum CamaraShooting { fps, spotLight};
    public GameObject fpsCam;
    public GameObject spotLightCam;
    public CamaraShooting cameraShooting = CamaraShooting.spotLight;
    public Toggle[] toggles;


    public void TurnOnSpotLight()
    {
        cameraShooting = CamaraShooting.spotLight;
        SwitchCamera();
    }
    public void TurnOnFPS()
    {
        cameraShooting = CamaraShooting.fps;
        SwitchCamera();
    }
    private void SwitchCamera()
    {
        fpsCam.SetActive(false);
        spotLightCam.SetActive(false);

        switch (cameraShooting)
        {
            case CamaraShooting.fps:
                fpsCam.SetActive(true);
                break;
            case CamaraShooting.spotLight:
                spotLightCam.SetActive(true);
                break;
        }
    }

    public void ClearSandBox()
    {
        ExampleClass.Instance.ResetHeights();
    }

    public void Rake()
    {
        ExampleClass.Instance.actionMode = ExampleClass.ActionMode.rake;
    }
    public void Circles()
    {
        ExampleClass.Instance.actionMode = ExampleClass.ActionMode.cirle;
    }
    public void Curves()
    {
        ExampleClass.Instance.actionMode = ExampleClass.ActionMode.curve;
    }

    public void Flatten()
    {
        ExampleClass.Instance.actionMode = ExampleClass.ActionMode.flatten;
    }

    public void SetAdditive(bool value)
    {
        ExampleClass.Instance.SetAdditive(value);
    }
}
