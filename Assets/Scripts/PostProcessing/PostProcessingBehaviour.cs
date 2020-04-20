using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class PostProcessingBehaviour : MonoBehaviour
{

    PostProcessVolume volume;
    public PlayerController pController;

    private void Start()
    {
        volume = GetComponent<PostProcessVolume>();
    }

    private void Update()
    {
        if (volume != null)
        {
            ColorGrading colorGrading;
            if (volume.profile.TryGetSettings(out colorGrading))
            {
                colorGrading.temperature.value = Mathf.Clamp(pController.GetTemperatureValue() - 50, -100, 0);
            }
        }
    }
}
