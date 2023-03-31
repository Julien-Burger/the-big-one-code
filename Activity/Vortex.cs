using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController[] skins;
    [SerializeField] Animator animator;
    [SerializeField] Activity activity;
    [SerializeField] AudioSource vortexDisappearAS;

    bool closed;

    private void Start()
    {
        int index = Random.Range(0, skins.Length);

        animator.runtimeAnimatorController = skins[index];
    }

    private void Update()
    {
        if (activity.GetIsActivityCompleted() && !closed)
        {
            closed = true;

            vortexDisappearAS.Play();

            animator.SetTrigger("close");
        }
    }
}
