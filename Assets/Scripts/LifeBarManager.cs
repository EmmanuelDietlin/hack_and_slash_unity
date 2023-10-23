using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LifeBarManager : MonoBehaviour
{
    [SerializeField] private GameObject lifeBar;
    [SerializeField] private GameObject lifeBarCanvas;

    private void Start()
    {
        lifeBar.GetComponent<Image>().fillAmount = gameObject.GetComponent<DamageManager>().HPRemainingRatio();
        gameObject.GetComponent<DamageManager>().damageTaken += UpdateLifeBar;
        lifeBarCanvas.SetActive(false);
    }

    private void UpdateLifeBar(object sender, EventArgs e)
    {
        if (! lifeBarCanvas.activeSelf) lifeBarCanvas.SetActive(true);
        lifeBar.GetComponent<Image>().fillAmount = gameObject.GetComponent<DamageManager>().HPRemainingRatio();
        // Debug.Log(lifeBar.GetComponent<Image>().fillAmount);
    }

    private void Update()
    {
        lifeBarCanvas.transform.LookAt(Camera.main.transform);
    }


}
