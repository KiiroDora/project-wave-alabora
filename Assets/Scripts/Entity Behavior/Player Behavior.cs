using System.Collections;
using UnityEngine;

public class PlayerBehavior : EntityBehavior
{
    protected override void Awake()
    {
        maxhp = 100; // TODO: regen? replace with "don't get hit too many times in a row"?
        base.Awake();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        // TODO: Wave power modifiers (damage / remaining hp^2 ????)
    }

    public override void Die()
    {
        base.Die();  // handles death here
        // TODO: stuff to add for Player's OnDeath -> set gameover screen, stop game time
    }

    public override IEnumerator AttackCoroutine()
    {
        if (canAttack)
        {
            Hitbox hitboxToUse;
            if (spriteRenderer.flipX == true)
            {
                // left hitbox
                hitboxToUse = hitboxes[1];
            }
            else
            {
                // right hitbox
                hitboxToUse = hitboxes[0];
            }

            // TODO: Play animation
            canAttack = false;
            Debug.Log("Attack Start!");
            yield return new WaitForSeconds(hitboxToUse.entryTime);  // wait for windup
            hurtbox.enabled = false;  // invincibility frames start

            hitboxToUse.col2D.enabled = true;  // activate hitbox
            
            Debug.Log("Contacting...");
            yield return new WaitForSeconds(hitboxToUse.contactTime);
            
            hurtbox.enabled = true;  // invincibility frames end
            hitboxToUse.col2D.enabled = false;  // deactivate hitbox
            
            Debug.Log("Attack finished");
            yield return new WaitForSeconds(hitboxToUse.exitTime);
            
            canAttack = true;
        }
    }

    public override void Attack()
    {
        base.Attack();
    }
}
