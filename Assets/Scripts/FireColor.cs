using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireColor : MonoBehaviour
{
    private Light myLight;
    private Renderer myRenderer;
    private Color myColor;
    private TrailRenderer myTrail;
    private float myRandomness;

    private void Start()
    {
        myLight = GetComponent<Light>();
        myRenderer = GetComponent<Renderer>();
        myColor = myRenderer.material.color;
        myTrail = GetComponent<TrailRenderer>();
        myRenderer.material.color = Color.red;
    }
    

    private void Update()
    {
        myRandomness = Random.Range(0f, 0.7f);
        myColor.r = 1f;
        myColor.b = 0f;
        myColor.g = myRandomness;
        myColor.a = myRandomness + 0.3f;
        myLight.color = myColor;
        myLight.intensity = myRandomness * 5f + 1f;
        myRenderer.material.color = myColor;
        myTrail.startColor = myColor;
        myTrail.endColor = myColor;
    }
    

}
