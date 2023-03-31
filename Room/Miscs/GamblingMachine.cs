using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingMachine : MonoBehaviour
{
    [System.Serializable]
    private class GamblingStatsUp
    {
        public GamblingStatsIndex statIndex;
        public int statChance;
        public float[] value;
        public int epicValuePercentage;
    }

    [SerializeField] GameObject pressKey;
    [SerializeField] GamblingStatsUp[] statsUp;
    [SerializeField] AnimationClip gamblingAnimation;
    [SerializeField] SpriteRenderer highlightSpriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] TextInfoSpawner textInfoSpawner;
    [SerializeField] AudioClip[] winAndLost;
    [SerializeField] AudioSource audioSource;

    CatchReferences references;
    PlayerInputSystem inputActions;

    int numberOfGambling;
    bool isAlreadyGambling;

    private void Awake()
    {
        references = FindObjectOfType<CatchReferences>();

        inputActions = new();
        inputActions.PressKey.GamblingMachine.performed += ctx => OnGambling();

        inputActions.PressKey.Enable();

        pressKey.SetActive(false);

        numberOfGambling = Random.Range(3, 7);
    }

    private void OnDestroy()
    {
        inputActions.PressKey.Disable();
    }

    private void Update()
    {
        if (!isAlreadyGambling && !pressKey.activeInHierarchy)
        {
            highlightSpriteRenderer.enabled = true;
        }
    }

    private void OnGambling()
    {
        if (!pressKey.activeInHierarchy) return;
        if (numberOfGambling == 0) return;

        references.GetGameManager().IncreaseNumberOfGambling();

        audioSource.volume = 0.3f;
        audioSource.Play();

        numberOfGambling--;
        isAlreadyGambling = true;

        pressKey.SetActive(false);
        highlightSpriteRenderer.enabled = false;

        GamblingStatsIndex statIndex = GamblingStatsIndex.None;

        while (true)
        {
            int chanceToWin = Random.Range(0, 100);

            foreach (var stat in statsUp)
            {
                if (stat.statChance >= chanceToWin)
                {
                    statIndex = stat.statIndex;
                }
            }

            if (statIndex == GamblingStatsIndex.Heal && references.GetPlayerStatistics().GetBaseHealth() == references.GetPlayerHealth().GetCurrentHealth()) continue;
            else if (statIndex == GamblingStatsIndex.UpgradeWeapon && references.GetFighter().GetIsWeaponUpgraded()) continue;
            else if (statIndex == GamblingStatsIndex.RemoveCurse && (!references.GetPlayerController().GetCursed() || references.GetCurse().GetCurrentCurse() == 0)) continue;
            else if (statIndex == GamblingStatsIndex.RemoveSimulation && !references.GetSimulationsPlaceHolder().CanRemoveSimulationLevel()) continue;

            break;
        }

        if (statIndex == GamblingStatsIndex.None)
        {
            StartCoroutine(Lose());
        }
        else
        {
            foreach (var stat in statsUp)
            {
                if (statIndex == stat.statIndex)
                {
                    int chanceForEpic = Random.Range(0, 100);

                    if (chanceForEpic <= stat.epicValuePercentage)
                    {
                        StartCoroutine(GainBonus(statIndex, stat.value[1], true));
                    }
                    else
                    {
                        StartCoroutine(GainBonus(statIndex, stat.value[0], false));
                    }
                }
            }
        }

        CheckAchievement();
    }

    private void CheckAchievement()
    {
        if (references.GetGameManager().GetNoobMode()) return;

        if (SteamManager.Initialized)
        {
            int numberOfGambling = references.GetGameManager().GetNumberOfGambling();

            if (numberOfGambling == 100)
            {
                SteamUserStats.GetAchievement("ACH_GAMBLING_100", out bool achievementUnlock);
                
                if (achievementUnlock)
                {
                    references.GetAchievements().SetAchievement("ACH_GAMBLING_100");
                    SteamUserStats.SetAchievement("ACH_GAMBLING_100");
                }
            }
            
            SteamUserStats.StoreStats();
        }
    }

    private IEnumerator GainBonus(GamblingStatsIndex statIndex, float value, bool isEpic)
    {
        yield return pressKey.GetComponent<PressKey>().SwitchKeySprites();

        animator.SetTrigger("win");

        yield return new WaitForSeconds(2.6f);

        audioSource.Stop();
        audioSource.volume = 0.6f;
        audioSource.PlayOneShot(winAndLost[0]);

        ColorUtility.TryParseHtmlString("#D65108", out Color epicColor);
        ColorUtility.TryParseHtmlString("#3152A6", out Color normalColor);

        if (statIndex == GamblingStatsIndex.Heal)
        {
            float healAmount = references.GetPlayerStatistics().GetBaseHealth() - references.GetPlayerHealth().GetCurrentHealth();

            if (healAmount < value)
            {
                value = healAmount;
            }

            references.GetPlayerHealth().Heal(value);

            if (isEpic)
            {
                textInfoSpawner.SpawnNPCBonus("Healing", epicColor);
            }
            else
            {
                textInfoSpawner.SpawnNPCBonus("Healing", normalColor);
            }
        }
        else if (statIndex == GamblingStatsIndex.DamageUp)
        {
            references.GetPlayerStatistics().AddBonusDamage(value);

            if (isEpic)
            {
                textInfoSpawner.SpawnNPCBonus("Damage +" + value, epicColor);
            }
            else
            {
                textInfoSpawner.SpawnNPCBonus("Damage +" + value, normalColor);
            }
        }
        else if (statIndex == GamblingStatsIndex.HealthUp)
        {
            references.GetPlayerStatistics().AddBonusHealth(value);

            if (isEpic)
            {
                textInfoSpawner.SpawnNPCBonus("Health +" + value, epicColor);
            }
            else
            {
                textInfoSpawner.SpawnNPCBonus("Health +" + value, normalColor);
            }
        }
        else if (statIndex == GamblingStatsIndex.ArmorUp)
        {
            references.GetPlayerStatistics().AddArmorPNJBonus(value);
            references.GetPlayerHealth().RegainArmor(value);

            if (isEpic)
            {
                textInfoSpawner.SpawnNPCBonus("Armor +" + value, epicColor);
            }
            else
            {
                textInfoSpawner.SpawnNPCBonus("Armor +" + value, normalColor);
            }
        }
        else if (statIndex == GamblingStatsIndex.UpgradeWeapon)
        {
            references.GetFighter().UpgradeWeapon(true);

            textInfoSpawner.SpawnNPCBonus(references.GetFighter().GetWeaponType().ToString() + "+", epicColor);
        }
        else if (statIndex == GamblingStatsIndex.RemoveCurse)
        {
            references.GetCurse().RemoveCurse(value);

            if (isEpic)
            {
                textInfoSpawner.SpawnNPCBonus("Curse", epicColor);
            }
            else
            {
                textInfoSpawner.SpawnNPCBonus("Curse", normalColor);
            }
        }
        else if (statIndex == GamblingStatsIndex.GainGlitch)
        {
            references.GetGlitchDisplay().AddGlitch((int)value);

            if (isEpic)
            {
                textInfoSpawner.SpawnNPCBonus("Glitches", epicColor);
            }
            else
            {
                textInfoSpawner.SpawnNPCBonus("Glitches", normalColor);
            }
        }
        else if (statIndex == GamblingStatsIndex.GainExperience)
        {
            references.GetExperience().GainExperience((int)value);

            if (isEpic)
            {
                textInfoSpawner.SpawnNPCBonus("Experience", epicColor);
            }
            else
            {
                textInfoSpawner.SpawnNPCBonus("Experience", normalColor);
            }
        }
        else if (statIndex == GamblingStatsIndex.GainRandomPassive)
        {
            references.GetPassiveObject().GainRandomPassive(false);

            textInfoSpawner.SpawnNPCBonus("Random Passive", normalColor);
        }
        else if (statIndex == GamblingStatsIndex.GainRandomTrinket)
        {
            references.GetTrinket().GainRandomTrinket(false);

            textInfoSpawner.SpawnNPCBonus("Random Trinket", normalColor);
        }
        else if (statIndex == GamblingStatsIndex.GainSecondChance)
        {
            references.GetPlayerHealth().AddSecondChance(value);

            if (isEpic)
            {
                textInfoSpawner.SpawnNPCBonus("Second Chance", epicColor);
            }
            else
            {
                textInfoSpawner.SpawnNPCBonus("Second Chance", normalColor);
            }
        }
        else if (statIndex == GamblingStatsIndex.RemoveSimulation)
        {
            references.GetSimulationsPlaceHolder().RemoveSimulationLevel();

            textInfoSpawner.SpawnNPCBonus("Remove Simulation", normalColor);
        }

        yield return new WaitForSeconds(gamblingAnimation.length - 2.06f);

        if (numberOfGambling != 0)
        {
            isAlreadyGambling = false;
        }
    }

    private IEnumerator Lose()
    {
        yield return pressKey.GetComponent<PressKey>().SwitchKeySprites();

        animator.SetTrigger("lose");

        yield return new WaitForSeconds(gamblingAnimation.length - 1f);

        audioSource.Stop();
        audioSource.volume = 0.6f;
        audioSource.PlayOneShot(winAndLost[1]);

        if (numberOfGambling != 0)
        {
            isAlreadyGambling = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pressKey.activeInHierarchy && !isAlreadyGambling)
        {
            pressKey.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (pressKey.activeInHierarchy && !isAlreadyGambling)
        {
            pressKey.SetActive(false);
        }
    }
}
