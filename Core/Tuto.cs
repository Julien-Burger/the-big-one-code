using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tuto : MonoBehaviour
{
    [SerializeField] DisplayDialogue IALaunchDialogue;

    [Header("Right And Left")]
    [SerializeField] CanvasGroup keyboardRightAndLeft;
    [SerializeField] CanvasGroup controllerRightAndLeft;

    [Header("Jump")]
    [SerializeField] CanvasGroup keyboardJump;
    [SerializeField] CanvasGroup controllerJump;
    [SerializeField] Image jumpImg;
    [SerializeField] Sprite[] jumpSprites;

    [Header("Roll")]
    [SerializeField] CanvasGroup keyboardRoll;
    [SerializeField] CanvasGroup controllerRoll;
    [SerializeField] Image rollImg;
    [SerializeField] Sprite[] rollSprites;
    [SerializeField] CanvasGroup healthBar;

    [Header("Attack")]
    [SerializeField] CanvasGroup damage;
    [SerializeField] CanvasGroup keyboardAttack;
    [SerializeField] CanvasGroup controllerAttack;
    [SerializeField] Image attackImg;
    [SerializeField] Sprite[] attackSprites;

    [Header("Potion")]
    [SerializeField] CanvasGroup potion;
    [SerializeField] CanvasGroup keyboardPotion;
    [SerializeField] CanvasGroup controllerPotion;
    [SerializeField] Image potionImg;
    [SerializeField] Sprite[] potionSprites;

    [Header("Weapon")]
    [SerializeField] CanvasGroup weapon;
    [SerializeField] CanvasGroup keyboardWeapon;
    [SerializeField] CanvasGroup controllerWeapon;
    [SerializeField] Image weaponImg;
    [SerializeField] Sprite[] weaponSprites;

    [Header("Stats Up Board")]
    [SerializeField] CanvasGroup keyboardStatsUpBoard;
    [SerializeField] CanvasGroup controllerStatsUpBoard;
    [SerializeField] Image statsUpBoardImg;
    [SerializeField] Sprite[] statsUpBoardSprites;

    [Header("Trinket")]
    [SerializeField] CanvasGroup trinket;
    [SerializeField] CanvasGroup trinket2;
    [SerializeField] CanvasGroup keyboardTrinket;
    [SerializeField] CanvasGroup controllerTrinket;
    [SerializeField] Image trinketImg;
    [SerializeField] Sprite[] trinketSprites;

    CatchReferences references;
    Room[] rooms;
    Coroutine lastCoroutine;
    Coroutine secondLastCoroutine;
    EnemyHealth zombie;

    bool disableRightAndLeftCanvas;
    bool disableJumpCanvas;
    bool disableRollCanvas;
    bool disableSpikes;
    bool disableAttackCanvas;
    bool disablePotion;
    bool disableWeapon;
    bool disableStatsUpBoard;
    bool disableTrinket;
    bool killZombie;
    bool findRooms;

    private void Awake()
    {
        references = FindObjectOfType<CatchReferences>();

        keyboardRightAndLeft.alpha = 0;
        controllerRightAndLeft.alpha = 0;

        keyboardJump.alpha = 0;
        controllerJump.alpha = 0;

        keyboardRoll.alpha = 0;
        controllerRoll.alpha = 0;

        healthBar.alpha = 0;

        damage.alpha = 0;
        keyboardAttack.alpha = 0;
        controllerAttack.alpha = 0;

        potion.alpha = 0;
        keyboardPotion.alpha = 0;
        controllerPotion.alpha = 0;

        weapon.alpha = 0;
        keyboardWeapon.alpha = 0;
        controllerWeapon.alpha = 0;

        keyboardStatsUpBoard.alpha = 0;
        controllerStatsUpBoard.alpha = 0;

        trinket.alpha = 0;
        trinket2.alpha = 0;
        keyboardTrinket.alpha = 0;
        controllerTrinket.alpha = 0;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.01f);

        references.GetPlayerController().disableControl = true;
        references.GetPlayerController().StopPlayer(true);
        references.GetPlayerController().FlipSprite(false);

        yield return new WaitForSeconds(2f);

        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("Where am I ?", null, TMPro.FontStyles.Bold);
        yield return new WaitForSeconds(1f);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("Hello ?", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("Hi C-001.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("Welcome to your new life.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("Please, follow the instructions \nand go to the next room to continue.", null, TMPro.FontStyles.Bold);

        references.GetPlayerController().disableControl = false;

        lastCoroutine = StartCoroutine(FadeOut(keyboardRightAndLeft));
        secondLastCoroutine = StartCoroutine(FadeOut(controllerRightAndLeft));

        rooms[0].OpenRoom(); //First Room

        yield return new WaitUntil(() => rooms[1].gameObject.activeInHierarchy == true);

        lastCoroutine = StartCoroutine(FadeOut(keyboardJump));
        secondLastCoroutine = StartCoroutine(FadeOut(controllerJump));

        references.GetPlayerController().EnableJump();

        yield return new WaitUntil(() => rooms[2].gameObject.activeInHierarchy == true);

        references.GetPlayerController().disableControl = true;
        references.GetPlayerController().StopPlayer(true);
        references.GetPlayerController().FlipSprite(false);

        yield return new WaitForSeconds(1f);

        yield return IALaunchDialogue.LaunchDialogue("You can roll to avoid damage.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("Try to run on these spikes.", null, TMPro.FontStyles.Bold);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("Really ?!", null, TMPro.FontStyles.Bold);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("Where the hell am I ?!", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("DO IT !", null, TMPro.FontStyles.Bold);

        references.GetPlayerController().disableControl = false;

        lastCoroutine = StartCoroutine(FadeOut(keyboardRoll));
        secondLastCoroutine = StartCoroutine(FadeOut(controllerRoll));
        StartCoroutine(FadeOut(healthBar));

        references.GetPlayerController().EnableRoll();

        yield return new WaitUntil(() => rooms[3].gameObject.activeInHierarchy == true);

        disableSpikes = true;

        references.GetPlayerController().disableControl = true;
        references.GetPlayerController().SetIsInActivity(true);
        references.GetPlayerController().StopPlayer(true);
        references.GetPlayerController().FlipSprite(false);

        yield return new WaitForSeconds(0.4f);

        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("What is that ?!", null, TMPro.FontStyles.Bold);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("Is he dead ?", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("SHUT UP !", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("In this simulation you will meet many monsters like that.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("To kill him, you have to focus on him.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("After that, do the key series above the enemy.", null, TMPro.FontStyles.Bold);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("I don't want to die !", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("This one is harmless.", null, TMPro.FontStyles.Bold);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("Please !", null, TMPro.FontStyles.Bold);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("Leave me alone !", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("FIGHT !", null, TMPro.FontStyles.Bold);

        references.GetPlayerController().disableControl = false;
        references.GetPlayerController().SetIsInActivity(false);

        StartCoroutine(FadeOut(damage));
        lastCoroutine = StartCoroutine(FadeOut(keyboardAttack));
        secondLastCoroutine = StartCoroutine(FadeOut(controllerAttack));

        zombie = FindObjectOfType<EnemyHealth>();
        
        yield return new WaitUntil(() => zombie.GetIsDead() == true);

        rooms[3].OpenRoom();

        yield return new WaitUntil(() => rooms[4].gameObject.activeInHierarchy == true);

        references.GetPlayerController().disableControl = true;
        references.GetPlayerController().StopPlayer(true);
        references.GetPlayerController().FlipSprite(false);

        yield return new WaitForSeconds(0.4f);

        references.GetPlayerHealth().UpdateStats();
        references.GetPlayerHealth().TakeDamage(60, false, true, false, false, false);

        yield return IALaunchDialogue.LaunchDialogue("To help you in your journey, you have a potion.", null, TMPro.FontStyles.Bold);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("Only that ?", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("Why do you ask so many questions ?", null, TMPro.FontStyles.Bold);

        StartCoroutine(FadeOut(potion));
        lastCoroutine = StartCoroutine(FadeOut(keyboardPotion));
        secondLastCoroutine = StartCoroutine(FadeOut(controllerPotion));

        references.GetPotion().CanUsePotion();

        references.GetPlayerController().disableControl = false;

        yield return new WaitUntil(() => rooms[5].gameObject.activeInHierarchy == true);

        references.GetPlayerController().disableControl = true;
        references.GetPlayerController().SetIsInActivity(true);
        references.GetPlayerController().StopPlayer(true);
        references.GetPlayerController().FlipSprite(false);

        yield return new WaitForSeconds(0.4f);

        references.GetFighter().SetWeaponType(WeaponType.DoubleAxe, 45f, 30f);
        references.GetWeaponDisplay().SetWeapon(WeaponType.DoubleAxe, false);

        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("Again ?!", null, TMPro.FontStyles.Bold);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("Why is he coming back ?", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("If you don’t stop crying, I will spawn 1000 zombies !", null, TMPro.FontStyles.Bold);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("...", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("So ! In your journey you will have a weapon to help you out.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("Each weapon have his own power.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("The weapon you hold will deal damage twice when you attack.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("But there is a time for the weapon to reload.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("You can't used it all the time.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("Try it on this zombie.", null, TMPro.FontStyles.Bold);

        StartCoroutine(FadeOut(weapon));
        lastCoroutine = StartCoroutine(FadeOut(keyboardWeapon));
        secondLastCoroutine = StartCoroutine(FadeOut(controllerWeapon));

        references.GetFighter().UnBlockWeaponPower();

        zombie = FindObjectOfType<EnemyHealth>();

        references.GetPlayerController().disableControl = false;
        references.GetPlayerController().SetIsInActivity(false);

        yield return new WaitUntil(() => rooms[6].gameObject.activeInHierarchy == true);

        FindObjectOfType<Room>().SetCanExitRoom(true);

        references.GetPlayerController().disableControl = true;
        references.GetPlayerController().StopPlayer(true);
        references.GetPlayerController().FlipSprite(false);

        yield return new WaitForSeconds(0.4f);

        yield return IALaunchDialogue.LaunchDialogue("After finishing a room, you have a chance to gain a bonus.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("Open the stats up board and make your choice.", null, TMPro.FontStyles.Bold);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("And if I don’t want one ?", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("Please. Shut up.", null, TMPro.FontStyles.Bold);

        lastCoroutine = StartCoroutine(FadeOut(keyboardStatsUpBoard));
        secondLastCoroutine = StartCoroutine(FadeOut(controllerStatsUpBoard));

        references.GetStatsUpBoard().CantUseStatsUpBoard(false);

        references.GetPlayerController().disableControl = false;

        yield return new WaitUntil(() => rooms[7].gameObject.activeInHierarchy == true);

        references.GetStatsUpBoard().CantUseStatsUpBoard(true);

        references.GetPlayerHealth().UpdateStats();
        references.GetPlayerHealth().TakeDamage(20, false, true, false, false, false);

        references.GetTrinket().FullChargeTrinket();

        references.GetPlayerController().disableControl = true;
        references.GetPlayerController().SetIsInActivity(true);
        references.GetPlayerController().StopPlayer(true);
        references.GetPlayerController().FlipSprite(false);

        yield return new WaitForSeconds(0.4f);

        yield return IALaunchDialogue.LaunchDialogue("Last thing.", null, TMPro.FontStyles.Bold);
        yield return references.GetPlayerDisplayDialogue().LaunchDialogue("It’s endless...", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("You are the most unbearable clone.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("When you complete an activity, you can charge your trinket.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("When fully charged, you can use it to gain a bonus.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("The trinket you got will heal you.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("There is several trinket with each a unique ability.", null, TMPro.FontStyles.Bold);

        lastCoroutine = StartCoroutine(FadeOut(keyboardTrinket));
        secondLastCoroutine = StartCoroutine(FadeOut(controllerTrinket));
        StartCoroutine(FadeOut(trinket));
        StartCoroutine(FadeOut(trinket2));

        references.GetPlayerController().disableControl = false;
        references.GetPlayerController().SetIsInActivity(false);

        yield return new WaitUntil(() => disableTrinket == true);

        references.GetPlayerController().disableControl = true;
        references.GetPlayerController().StopPlayer(true);

        yield return new WaitForSeconds(0.4f);

        yield return IALaunchDialogue.LaunchDialogue("Now you have everything you need to beat the simulation.", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("Use everything you've learned to beat the simulation !", null, TMPro.FontStyles.Bold);
        yield return IALaunchDialogue.LaunchDialogue("Past this door and start your journey.", null, TMPro.FontStyles.Bold);

        rooms[7].OpenRoom();

        references.GetPlayerController().disableControl = false;
    }

    private void Update()
    {
        if (!findRooms)
        {
            findRooms = true;

            rooms = FindObjectsOfType<Room>(true);
            Array.Reverse(rooms);
        }

        if (references.GetPlayerController().GetHorizontalAxis() != 0 && keyboardRightAndLeft.alpha == 1 && !disableRightAndLeftCanvas)
        {
            disableRightAndLeftCanvas = true;

            StartCoroutine(FadeIn(keyboardRightAndLeft));
            StartCoroutine(FadeIn(controllerRightAndLeft));
        }
        else if (!references.GetPlayerController().GetFeetCollider().IsTouchingLayers(LayerMask.GetMask("Ground")) && !disableJumpCanvas
            && keyboardJump.alpha == 1)
        {
            disableJumpCanvas = true;

            StartCoroutine(FadeIn(keyboardJump));
            StartCoroutine(FadeIn(controllerJump));

            rooms[1].OpenRoom();
        }
        else if (references.GetPlayerController().isRolling && !disableRollCanvas && keyboardRoll.alpha == 1)
        {
            disableRollCanvas = true;

            StartCoroutine(FadeIn(keyboardRoll));
            StartCoroutine(FadeIn(controllerRoll));

            rooms[2].OpenRoom();
        }

        if (!disableSpikes && rooms[2].gameObject.activeInHierarchy && references.GetPlayerHealth().GetCurrentHealth() <= 20)
        {
            disableSpikes = true;

            GameObject spikes = GameObject.FindWithTag("TutoSpikes");
            ParticleSystem[] particles = spikes.GetComponentsInChildren<ParticleSystem>();
            SpriteRenderer[] spriteRenderers = spikes.GetComponentsInChildren<SpriteRenderer>();

            foreach (var sprite in spriteRenderers)
            {
                sprite.gameObject.SetActive(false);
            }

            foreach (var particle in particles)
            {
                particle.Play();
            }
        }

        if (zombie && !disableAttackCanvas && references.GetPlayerController().GetIsInActivity() && keyboardAttack.alpha == 1)
        {
            disableAttackCanvas = true;

            StartCoroutine(FadeIn(keyboardAttack));
            StartCoroutine(FadeIn(controllerAttack));
        }

        if (!disablePotion && references.GetPlayerHealth().GetCurrentHealth() == 80 && rooms[4].gameObject.activeInHierarchy
            && keyboardPotion.alpha == 1)
        {
            disablePotion = true;

            StartCoroutine(FadeIn(keyboardPotion));
            StartCoroutine(FadeIn(controllerPotion));

            rooms[4].OpenRoom();
        }

        if (zombie && !disableWeapon && references.GetFighter().GetCanUseWeapon() && rooms[5].gameObject.activeInHierarchy && keyboardWeapon.alpha == 1)
        {
            disableWeapon = true;

            FindObjectOfType<EnemyController>().UnBlockCanAttack();

            StartCoroutine(FadeIn(keyboardWeapon));
            StartCoroutine(FadeIn(controllerWeapon));
        }

        if (!killZombie && rooms[5].gameObject.activeInHierarchy && zombie.GetIsDead())
        {
            killZombie = true;

            rooms[5].OpenRoom();
        }

        if (!disableStatsUpBoard && references.GetStatsUpBoard().IsStatsUpBoardOpen() && rooms[6].gameObject.activeInHierarchy && keyboardStatsUpBoard.alpha == 1)
        {
            disableStatsUpBoard = true;

            StartCoroutine(FadeIn(keyboardStatsUpBoard));
            StartCoroutine(FadeIn(controllerStatsUpBoard));

            rooms[6].OpenRoom();
        }

        if (!disableTrinket && references.GetTrinket().GetCurrentChargeValue() == 0 && rooms[7].gameObject.activeInHierarchy && keyboardTrinket.alpha == 1)
        {
            disableTrinket = true;

            StartCoroutine(FadeIn(keyboardTrinket));
            StartCoroutine(FadeIn(controllerTrinket));
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (references.GetGameManager().GetKeyboardControl())
        {
            keyboardRightAndLeft.gameObject.SetActive(true);
            controllerRightAndLeft.gameObject.SetActive(false);

            keyboardJump.gameObject.SetActive(true);
            controllerJump.gameObject.SetActive(false);

            keyboardRoll.gameObject.SetActive(true);
            controllerRoll.gameObject.SetActive(false);

            keyboardAttack.gameObject.SetActive(true);
            controllerAttack.gameObject.SetActive(false);

            keyboardPotion.gameObject.SetActive(true);
            controllerPotion.gameObject.SetActive(false);

            keyboardWeapon.gameObject.SetActive(true);
            controllerWeapon.gameObject.SetActive(false);

            keyboardStatsUpBoard.gameObject.SetActive(true);
            controllerStatsUpBoard.gameObject.SetActive(false);

            keyboardTrinket.gameObject.SetActive(true);
            controllerTrinket.gameObject.SetActive(false);
        }
        else
        {
            keyboardRightAndLeft.gameObject.SetActive(false);
            controllerRightAndLeft.gameObject.SetActive(true);

            keyboardJump.gameObject.SetActive(false);
            controllerJump.gameObject.SetActive(true);

            keyboardRoll.gameObject.SetActive(false);
            controllerRoll.gameObject.SetActive(true);

            keyboardAttack.gameObject.SetActive(false);
            controllerAttack.gameObject.SetActive(true);

            keyboardPotion.gameObject.SetActive(false);
            controllerPotion.gameObject.SetActive(true);

            keyboardWeapon.gameObject.SetActive(false);
            controllerWeapon.gameObject.SetActive(true);

            keyboardStatsUpBoard.gameObject.SetActive(false);
            controllerStatsUpBoard.gameObject.SetActive(true);

            keyboardTrinket.gameObject.SetActive(false);
            controllerTrinket.gameObject.SetActive(true);

            if (references.GetGameManager().GetChangeUI())
            {
                jumpImg.sprite = jumpSprites[1];
                jumpImg.SetNativeSize();

                rollImg.sprite = rollSprites[1];
                rollImg.SetNativeSize();

                attackImg.sprite = attackSprites[1];
                potionImg.sprite = potionSprites[1];
                weaponImg.sprite = weaponSprites[1];
                statsUpBoardImg.sprite = statsUpBoardSprites[1];
                trinketImg.sprite = trinketSprites[1];
            }
            else
            {
                jumpImg.sprite = jumpSprites[0];
                jumpImg.SetNativeSize();

                rollImg.sprite = rollSprites[0];
                rollImg.SetNativeSize();

                attackImg.sprite = attackSprites[0];
                potionImg.sprite = potionSprites[0];
                weaponImg.sprite = weaponSprites[0];
                statsUpBoardImg.sprite = statsUpBoardSprites[0];
                trinketImg.sprite = trinketSprites[0];
            }
        }
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        if (lastCoroutine != null)
        {
            StopCoroutine(lastCoroutine);
        }
        
        if (secondLastCoroutine != null)
        {
            StopCoroutine(secondLastCoroutine);
        }

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime;

            yield return null;
        }
    }
}
