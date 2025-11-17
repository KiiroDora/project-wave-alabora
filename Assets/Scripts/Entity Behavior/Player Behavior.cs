using System.Collections;
using UnityEngine;

public class PlayerBehavior : EntityBehavior
{
    public int timesGotHit = 0;
    public int timesHit = 2;

    public enum PulseState {DEAD, SMALL, MEDIUM, LARGE, XL, XXL};
    public PulseState pulseState = PulseState.MEDIUM;

    public float attackSpeed = 1;


    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(CheckPulse(3f));
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
            yield return new WaitForSeconds(hitboxToUse.entryTime / attackSpeed);  // wait for windup
            hurtbox.enabled = false;  // invincibility frames start

            hitboxToUse.col2D.enabled = true;  // activate hitbox

            yield return new WaitForSeconds(hitboxToUse.contactTime);

            hurtbox.enabled = true;  // invincibility frames end
            hitboxToUse.col2D.enabled = false;  // deactivate hitbox

            yield return new WaitForSeconds(hitboxToUse.exitTime / attackSpeed);

            canAttack = true;
        }
    }

    public IEnumerator CheckPulse(float time)
    {
        yield return new WaitForSeconds(time);

        int pulse = Mathf.Clamp(timesHit - timesGotHit, 0, 999);
        Debug.Log("Pulse: " + pulse + "\nHit: " + timesHit + "\nGot Hit: " + timesGotHit);

        if (pulse > 5)
        {
            pulseState++;
            if ((int)pulseState > 5) { pulseState = PulseState.XXL; }
            Debug.Log(pulseState);
        }
        else if (pulse >= 3)
        {
            Debug.Log(pulseState);
        }
        else
        {
            pulseState--;
            if ((int)pulseState <= 0) { pulseState = PulseState.DEAD; }
            Debug.Log(pulseState);
        }

        timesGotHit = 0;
        timesHit = 0;

        attackSpeed = 1f + (int)pulseState * 0.1f;
        PlayerControls.jumpForce = 6f + (int)pulseState * 1f;
        PlayerControls.moveSpeed = 6f + (int)pulseState * 1f;

        StartCoroutine(CheckPulse(time));
    }

    // TODO: this is a temporary gameover
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sea"))
        {
            Destroy(gameObject);
            Time.timeScale = 0;
            Debug.Log("YOU DIED FOR REALS");
        }
    }

    public override void Attack()
    {
        base.Attack();
    }
}
