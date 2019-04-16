using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightScript : MonoBehaviour
{

    Light fireLight;
    float lightInt;
    float lightRange;

    public bool IntensityVariation = false;
    public bool RangeVariation = true;

    public float minIntensity = 1.5f;
    public float maxIntensity = 2f;

    public float minRange = 5f;
    public float maxRange = 15f;

    // Use this for initialization
    void Start()
    {
        fireLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IntensityVariation)
        {
            lightInt = Random.Range(minIntensity, maxIntensity);
            fireLight.intensity = lightInt;
        }
        if (RangeVariation)
        {
            lightRange = Random.Range(minRange, maxRange);
            fireLight.range = Mathf.Lerp(fireLight.range, lightRange, 0.5f);
            //fireLight.range = lightRange;
        }
    }
}
