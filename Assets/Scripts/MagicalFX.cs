using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalFX : MonoBehaviour
{
    private Light myLight;
    private Renderer myRenderer;
    private Color myColor;
    private TrailRenderer myTrail;
    [SerializeField] private float speedoflight = 1f;

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
        if (myColor.r >= 1)
        {
            if (myColor.b >= 0.2)
            {
                myColor.b -= Time.deltaTime * speedoflight;
            }
            else
            {
                myColor.g += Time.deltaTime * speedoflight;
            }            
        }
        if (myColor.g >= 1)
        {
            if (myColor.r >= 0.2)
            {
                myColor.r -= Time.deltaTime * speedoflight;
            }
            else
            {
                myColor.b += Time.deltaTime * speedoflight;
            }
        }
        if (myColor.b >= 1)
        {
            if (myColor.g >= 0.2)
            {
                myColor.g -= Time.deltaTime * speedoflight;
            }
            else
            {
                myColor.r += Time.deltaTime * speedoflight;
            }
        }
        myLight.color = myColor;
        myRenderer.material.color = myColor;
        myTrail.startColor = myColor;
        myTrail.endColor = myColor;
    }

}
