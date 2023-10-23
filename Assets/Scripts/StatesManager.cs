using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesManager : MonoBehaviour
{
    public static event Action invisible;
    public static event Action visible;
    public static event Action paralizePlayer;
    public static event Action unparalizePlayer;

    public static void IsInvisible()
    {
        invisible?.Invoke();
    }

    public static void IsVisible()
    {
        visible?.Invoke();
    }

    public static void ParalizePlayer()
    {
        paralizePlayer?.Invoke();
    }

    public static void UnparalizePlayer()
    {
        unparalizePlayer?.Invoke();
    }
}
