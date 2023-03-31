using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerGhost : EnemyController
{
    [Header("Villager Ghost")]
    [SerializeField] RuntimeAnimatorController[] skins;
    [SerializeField] float attackSpeed;

    bool incanted;

    private void Start()
    {
        AddEnemyToList(gameObject);
        SetCanAttack(true);

        if (incanted) return;

        if (GetSimulationsPlaceHolder().GetEnableActivityTimer())
        {
            attackSpeed += 1;
        }
        
        if (GetSimulationsPlaceHolder().GetDamageKeySeries())
        {
            attackSpeed += 1;
        }
    }

    private void OnEnable()
    {
        int index = Random.Range(0, skins.Length);

        GetAnimator().runtimeAnimatorController = skins[index];
    }

    public override void Attack()
    {
        FlipSprite(GetTarget().transform.position.x);

        transform.position = Vector2.MoveTowards(transform.position, GetTarget().transform.position, Time.deltaTime * attackSpeed);
    }

    public void Incanted(bool enemyDeath)
    {
        incanted = true;

        if (enemyDeath)
        {
            attackSpeed = 3f;

            if (GetSimulationsPlaceHolder().GetEnableActivityTimer() && GetSimulationsPlaceHolder().GetDamageKeySeries())
            {
                attackSpeed += 1;
            }
        }
        else
        {
            attackSpeed = 5;
        }
    }
}
