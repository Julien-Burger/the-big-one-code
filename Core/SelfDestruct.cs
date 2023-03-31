using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [Tooltip("Set Time To Wait at -1 to use the animation / audio clip duration instead")]
    [SerializeField] float timeToWait;
    [SerializeField] AnimationClip clip;
    [SerializeField] AudioClip audioClip;

    float timer;

    private void Start()
    {
        if (timeToWait == -1)
        {
            if (clip)
            {
                timeToWait = clip.length;
            }
            else if (audioClip)
            {
                timeToWait = audioClip.length;
            }
        }
    }

    private void Update()
    {
        if (timeToWait < timer)
        {
            Destroy(gameObject);
        }

        timer += Time.deltaTime;
    }

    public void SetTimeToWait(float timeToWait)
    {
        this.timeToWait = timeToWait;
    }
}
