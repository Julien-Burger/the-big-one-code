using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMusics : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] inGameMusics;

    private void Start()
    {
        StartCoroutine(TakeMusic());
    }

    private IEnumerator TakeMusic()
    {
        while (true)
        {
            int index = Random.Range(0, inGameMusics.Length);

            audioSource.PlayOneShot(inGameMusics[index]);

            yield return new WaitForSeconds(inGameMusics[index].length);
        }
    }
}
