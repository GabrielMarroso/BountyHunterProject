using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erika : Character
{

    [SerializeField] GameObject arrowPrefab;

    protected override void SimpleAttack()
    {
        base.SimpleAttack();        
    }

    public void CreateArrow()
    {
        Bullet _arrow = Instantiate(arrowPrefab, posAttack.position, posAttack.rotation).GetComponent<Bullet>();
        _arrow.SetBullet(target, simpleAttackDamage);

    }
}
