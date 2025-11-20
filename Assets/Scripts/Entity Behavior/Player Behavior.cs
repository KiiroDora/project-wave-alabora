using System.Collections;
using UnityEngine;

public class PlayerBehavior : EntityBehavior
{
    public int timesGotHit = 0;
    public int timesHit = 2;

    public enum PulseState {DEAD, SMALL, MEDIUM, LARGE, XL, XXL};
    public PulseState pulseState = PulseState.MEDIUM;

    public float attackSpeed = 1;
    public float baseMoveSpeed = 8;
    public float baseJumpForce = 8;


    protected override void Awake()
    {
        base.Awake();

        TextController.UpdatePulseText(pulseState.ToString());

        PlayerControls.jumpForce = baseJumpForce;
        PlayerControls.moveSpeed = baseMoveSpeed;

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

            SpriteRenderer temporaryImage = hitboxToUse.GetComponentInChildren<SpriteRenderer>();  // TODO: remove temporary stuff here
            temporaryImage.enabled = true;
            temporaryImage.color = Color.blue;

            // TODO: Play animation
            canAttack = false;
            yield return new WaitForSeconds(hitboxToUse.entryTime / attackSpeed);  // wait for windup
            hurtbox.enabled = false;  // invincibility frames start

            hitboxToUse.col2D.enabled = true;  // activate hitbox

            temporaryImage.color = Color.red;

            yield return new WaitForSeconds(hitboxToUse.contactTime);

            hurtbox.enabled = true;  // invincibility frames end
            hitboxToUse.col2D.enabled = false;  // deactivate hitbox

            temporaryImage.color = Color.blue;

            yield return new WaitForSeconds(hitboxToUse.exitTime / attackSpeed);

            temporaryImage.enabled = false;
            canAttack = true;
        }
    }

    public IEnumerator CheckPulse(float time)
    {
        yield return new WaitForSeconds(time);

        int pulse = Mathf.Clamp(timesHit - timesGotHit, 0, 999);

        if (pulse > 5)
        {
            pulseState++;
            if ((int)pulseState > 5) { pulseState = PulseState.XXL; }
        }
        else if (pulse >= 3)
        {
        }
        else
        {
            pulseState--;
            if ((int)pulseState <= 0) { pulseState = PulseState.DEAD; }
        }

        TextController.UpdatePulseText(pulseState.ToString());

        timesGotHit = 0;
        timesHit = 0;

        attackSpeed = 1f + (int)pulseState * 0.1f;
        PlayerControls.jumpForce = baseJumpForce + (int)pulseState * 1f;
        PlayerControls.moveSpeed = baseMoveSpeed + (int)pulseState * 1f;
        PlayerControls.fallingMultiplier = 2f + (int)pulseState * 0.2f;

        StartCoroutine(CheckPulse(time));
    }

    // TODO: this is a temporary gameover
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sea"))
        {
            Destroy(gameObject);
            GameController.loseTrigger?.Invoke();
        }
    }

    public override void Attack()
    {
        base.Attack();
    }
}
