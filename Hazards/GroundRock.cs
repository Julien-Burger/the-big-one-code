using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRock : MonoBehaviour
{
    [SerializeField] PolygonCollider2D polygonCollider;
    [SerializeField] float groundRockDamage;
    [SerializeField] AudioSource groundRockAppearAS;

    CatchReferences references;
    GenerateDamage generateDamage;

    private void Awake()
    {
        references = FindObjectOfType<CatchReferences>();
        generateDamage = GetComponent<GenerateDamage>();
    }

    //Use in ground rock animation
    private void PlayGroundRockSFX()
    {
        groundRockAppearAS.Play();
    }

    //Use in ground rock animation
    private void CheckGroundRockCollider()
    {
        if (polygonCollider.IsTouching(references.GetPlayerController().GetBodyCollider()))
        {
            float damage = generateDamage.GetRandomDamage(groundRockDamage, false, false);
            bool isCritical = generateDamage.GetIsCriticalDamage();

            references.GetPlayerHealth().TakeDamage(damage, isCritical, false, false, true, true);
        }
    }
}
