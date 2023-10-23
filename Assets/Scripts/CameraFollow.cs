using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    //Faire en sorte que la caméra ne sorte pas des limites du terrain
    //(blocage si transform.position = player.transform.position + offset fait que la caméra dépasse la bordure du terrain)
    private void LateUpdate()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (!player) return;
        transform.position = player.transform.position;
    }
}
