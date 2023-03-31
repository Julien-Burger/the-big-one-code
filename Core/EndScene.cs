using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    [SerializeField] bool loadCredit;
    [SerializeField] GameManager gameManager;
    [SerializeField] Fader fader;
    [SerializeField] AudioSource endSceneSFX;

    private IEnumerator Start()
    {
        yield return fader.FadeIn(1f);
    }

    private void Update()
    {
        if (loadCredit)
        {
            loadCredit = false;

            StartCoroutine(DisableSFX());

            StartCoroutine(gameManager.LoadCredits());
        }
    }

    private IEnumerator DisableSFX()
    {
        while (endSceneSFX.volume > 0)
        {
            endSceneSFX.volume -= Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
