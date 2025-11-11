using System.Collections;
using UnityEngine;

public class EnemyBehavior : EntityBehavior
{
    protected override void Awake()
    {        
        maxhp = 30;
        base.Awake();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();  // handles death here
        // TODO: stuff to add for Enemy's OnDeath -> Play animation then Destroy gameobject in animation's OnExit
    }

    public override IEnumerator AttackCoroutine()  // TODO: copypaste from player and update accordingly
    {
        yield return null;
    }

    public override void Attack()
    {
        base.Attack();
    }
}
