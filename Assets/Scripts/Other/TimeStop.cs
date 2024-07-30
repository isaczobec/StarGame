using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{

    public static TimeStop Instance { get; private set; } // Singleton instance

    private Coroutine timeStopCoroutine;


    private void Awake()
    {
        Instance = this;
    }


    public void StopTime(float duration)
    {
        if (timeStopCoroutine != null)
        {
            StopCoroutine(timeStopCoroutine);
        }
        StartCoroutine(TimeStopCoroutine(duration));
    }

    private IEnumerator TimeStopCoroutine(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

}
