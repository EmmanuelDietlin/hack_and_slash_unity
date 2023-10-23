using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyLightening : MonoBehaviour
{
    private new Light light;
    private UnityEngine.Color initialColor;
    private UnityEngine.Color highlightColor = UnityEngine.Color.white;

    [SerializeField] private float period = 7f;
    [SerializeField] private float duration = 0.5f;
    private float periodDouble;
    private float timerPeriod = 0f;
    private float timerDuration = 0f;
    private float timerDouble = 0f;

    [SerializeField] private float angleSpeed = 1f;
    private float rotationSpeed;

    private AudioSource audio;
    [SerializeField] private AudioClip thunder;
    private int thunderState = 0;

    private void Awake()
    {
        light = GetComponent<Light>();
        initialColor = light.color;
        periodDouble = period+period+2;
        rotationSpeed = angleSpeed;
        audio = GetComponent<AudioSource>();
        audio.volume = PlayerPrefs.GetFloat("EffectVolume");
    }

    private void Update()
    {
        transform.Rotate(0,Time.deltaTime * rotationSpeed, 0,Space.World);
        timerPeriod += Time.deltaTime;
        timerDouble += Time.deltaTime;
        if (timerPeriod >= period)
        {
            rotationSpeed = 0f;
            light.color = highlightColor;
            timerDuration += Time.deltaTime;
            if (audio.time>1.5 || !audio.isPlaying)
            {
                audio.Play();
            }
            if (timerDuration >= duration)
            {
                light.color = initialColor;
                timerPeriod = 0f;
                timerDuration = 0f;
                rotationSpeed = angleSpeed;
            }
        }
        if (timerDouble >= periodDouble)
        {
            rotationSpeed = 0f;
            light.color = highlightColor;
            timerDuration += Time.deltaTime;
            if (audio.time >= 1.9 && thunderState==1)
            {
                audio.PlayOneShot(thunder);
                thunderState = 0;
            }
            if (timerDuration >= duration)
            {
                light.color = initialColor;
                timerPeriod = 0f;
                timerDouble = 0f;
                timerDuration = 0f;
                rotationSpeed = angleSpeed;
                thunderState = 1;
            }
        }

    }
}
