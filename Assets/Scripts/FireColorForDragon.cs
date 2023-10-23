using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireColorForDragon : MonoBehaviour
{
    private Light myLight;
    private Color myColor;
    private float myRandomness;

    private void Start()
    {
        myLight = GetComponent<Light>();
    }


    private void Update()
    {
        myRandomness = Random.Range(0f, 0.7f);
        myColor.r = 1f;
        myColor.b = 0f;
        myColor.g = myRandomness;
        myColor.a = myRandomness + 0.3f;
        myLight.color = myColor;
        myLight.intensity = myRandomness * 7f + 4f;
    }


}
