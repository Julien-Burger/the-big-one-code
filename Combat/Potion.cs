using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Potion : MonoBehaviour, IUpdateStats
{
    [SerializeField] ChargeBar potionChargeBar;
    [SerializeField] Image cantUseImg;
    [SerializeField] bool cantUsePotion;
    [SerializeField] AudioSource potionAS;
    [SerializeField] AudioClip[] potionClips;

    CatchReferences references;
    PlayerInputSystem inputActions;
    Animator cantUseAnimator;

    PotionsType potionType;

    const float healPercentage = 0.4f;
    const float timeToHeal = 1f;

    float numberOfPotions;
    float timeSinceStartPotion;

    bool usePotion;
    bool isFadeIn;
    bool isFadeOut = true;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Home" || SceneManager.GetActiveScene().name == "End Scene") return;

        references = FindObjectOfType<CatchReferences>();
        cantUseAnimator = cantUseImg.GetComponent<Animator>();

        inputActions = new();
        inputActions.Player.UsePotion.performed += ctx => OnUsePotion();
        inputActions.Player.UsePotion.canceled += ctx => OnStopUsePotion();

        inputActions.Player.Enable();

        cantUseImg.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void OnDestroy()
    {
        if (SceneManager.GetActiveScene().name == "Home" || SceneManager.GetActiveScene().name == "End Scene") return;

        inputActions.Player.Disable();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Home" || SceneManager.GetActiveScene().name == "End Scene") return;
        if (references.GetPauseMenu().GetInPauseMenu()) return;

        CantUseDisplay();

        if (references.GetPlayerController().GetIsInActivity())
        {
            usePotion = false;

            timeSinceStartPotion = 0f;

            potionChargeBar.PotionUpdateChargeBar(timeSinceStartPotion, timeToHeal);

            return;
        }

        if (usePotion)
        {
            UsePotion();
        }
        else
        {
            timeSinceStartPotion = 0f;
        }

        potionChargeBar.PotionUpdateChargeBar(timeSinceStartPotion, timeToHeal);
    }

    private void CantUseDisplay()
    {
        if (references.GetPlayerController().GetIsInActivity() && numberOfPotions > 0 && !isFadeIn)
        {
            if (cantUseAnimator.enabled == false)
            {
                cantUseAnimator.enabled = true;
            }

            isFadeIn = true;
            isFadeOut = false;

            cantUseAnimator.ResetTrigger("fadeOut");
            cantUseAnimator.SetTrigger("fadeIn");
        }
        else if (!isFadeOut && !references.GetPlayerController().GetIsInActivity())
        {
            if (cantUseAnimator.enabled == false)
            {
                cantUseAnimator.enabled = true;
            }

            isFadeOut = true;
            isFadeIn = false;

            cantUseAnimator.ResetTrigger("fadeIn");
            cantUseAnimator.SetTrigger("fadeOut");
        }
    }

    private void OnUsePotion()
    {
        if (cantUsePotion) return;
        if (references.GetPlayerController().GetIsInActivity()) return;
        if (references.GetPauseMenu().GetInPauseMenu()) return;

        usePotion = true;
    }

    private void OnStopUsePotion()
    {
        usePotion = false;
    }

    private void UsePotion()
    {
        if (numberOfPotions > 0)
        {
            timeSinceStartPotion += Time.deltaTime;

            if (timeSinceStartPotion > timeToHeal)
            {
                PlaySFX();

                numberOfPotions--;

                references.GetPlayerHealth().Heal(references.GetPlayerStatistics().GetBaseHealth() * healPercentage);;

                references.GetPotionsDisplay().UpdatePotion(potionType, numberOfPotions);

                references.GetCurse().RemoveCurse(40);

                timeSinceStartPotion = 0f;
            }
        }
    }

    private void PlaySFX()
    {
        int index = UnityEngine.Random.Range(0, potionClips.Length);

        potionAS.PlayOneShot(potionClips[index]);
    }

    public void UpdateStats()
    {
        if (SceneManager.GetActiveScene().name == "Home" || SceneManager.GetActiveScene().name == "End Scene") return;

        numberOfPotions = references.GetPlayerStatistics().GetPotionNumber();

        if (numberOfPotions == 1)
        {
            potionType = PotionsType.First;
        }
        else if (numberOfPotions == 2)
        {
            potionType = PotionsType.Second;
        }
        else
        {
            potionType = PotionsType.Third;
        }

        if (references.GetPotionsDisplay())
        {
            references.GetPotionsDisplay().UpdatePotion(potionType, numberOfPotions);
        }
    }

    public void CanUsePotion()
    {
        cantUsePotion = false;
    }

    public void AddPotion()
    {
        numberOfPotions++;

        references.GetPotionsDisplay().UpdatePotion(potionType, numberOfPotions);
    }

    public bool CanAddPotion()
    {
        if (potionType == PotionsType.First && numberOfPotions < 1)
        {
            return true;
        }
        else if (potionType == PotionsType.Second && numberOfPotions < 2)
        {
            return true;
        }
        else if (potionType == PotionsType.Third && numberOfPotions < 3)
        {
            return true;
        }

        return false;
    }
}
