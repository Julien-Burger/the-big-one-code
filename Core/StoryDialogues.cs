using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDialogues : MonoBehaviour, ISaveable
{
    [SerializeField] GameObject[] merchants;

    int storyDialoguesIndex;
    float timeSinceSpawn;
    float timeBeforeStartDialogue;
    bool dialogueStarted;

    private IEnumerator Start()
    {
        FindObjectOfType<SavingWrapper>().LoadOneState(GetComponent<SaveableEntity>().GetUniqueIdentifier());

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < merchants.Length; i++)
        {
            if (i != storyDialoguesIndex)
            {
                merchants[i].SetActive(false);
            }
        }

        timeBeforeStartDialogue = UnityEngine.Random.Range(3, 6);
    }

    private void Update()
    {
        if (dialogueStarted) return;

        if ((storyDialoguesIndex == 11 || storyDialoguesIndex == 16 || storyDialoguesIndex == 18 || 
            storyDialoguesIndex == 19) && timeSinceSpawn > timeBeforeStartDialogue)
        {
            StartCoroutine(StartDialogue());
        }

        timeSinceSpawn += Time.deltaTime;
    }

    private IEnumerator StartDialogue()
    {
        dialogueStarted = true;

        yield return merchants[storyDialoguesIndex].GetComponent<LaunchDialogue>().StartDialogue();

        StartCoroutine(IncreaseStoryDialoguesIndex());
    }

    public IEnumerator IncreaseStoryDialoguesIndex()
    {
        dialogueStarted = true;

        if (storyDialoguesIndex < merchants.Length - 1)
        {
            storyDialoguesIndex++;
        }

        CheckAchievement();

        yield return new WaitForSeconds(1f);

        StartCoroutine(FindObjectOfType<GameManager>().LoadCredits());
    }

    private void CheckAchievement()
    {
        if (FindObjectOfType<GameManager>().GetNoobMode()) return;
        if (storyDialoguesIndex != merchants.Length - 1) return;

        if (SteamManager.Initialized)
        {
            SteamUserStats.GetAchievement("ACH_STORY", out bool achievmentUnlock);

            if (!achievmentUnlock)
            {
                FindObjectOfType<Achievements>().SetAchievement("ACH_STORY");

                SteamUserStats.SetAchievement("ACH_STORY");
            }

            SteamUserStats.StoreStats();
        }
    }

    public object CaptureState()
    {
        return storyDialoguesIndex;
    }

    public void RestoreState(object state)
    {
        storyDialoguesIndex = (int)state;
    }
}
