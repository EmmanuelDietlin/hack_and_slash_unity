using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityGlow : MonoBehaviour
{
    [SerializeField] float defaultIntensity = 10f;
    [SerializeField] float amplitude = 2f;
    [SerializeField] float speed = 3f;
    Light myLight;
    float intensmax;
    float intensmin;
    int state = 0;
    void Start()
    {
        myLight = GetComponent<Light>();
        intensmax = defaultIntensity + amplitude;
        intensmin = defaultIntensity - amplitude;
    }

    void Update()
    {
        if (state == 0)
        {
            myLight.intensity += Time.deltaTime*speed;
            if (myLight.intensity >= intensmax)
            {
                state = 1;
            }
        }
        if (state == 1)
        {
            myLight.intensity -= Time.deltaTime*speed;
            if (myLight.intensity <= intensmin)
            {
                state = 0;
            }
        }
    }
}
