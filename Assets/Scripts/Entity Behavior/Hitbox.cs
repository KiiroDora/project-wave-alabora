using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float entryTime;
    public float contactTime;
    public float exitTime;

    public Collider2D col2D;

    public int damage;
    public float knockbackRate;

    void Awake()
    {
        col2D = GetComponent<Collider2D>();
    }
    
    void OnTriggerEnter2D(Collider2D collision)  // this is where the results of the attack actually happens
    {
        if (collision.gameObject.CompareTag("Hurtbox"))
        {
            EntityBehavior target = collision.gameObject.GetComponentInParent<EntityBehavior>();
            EntityBehavior attacker = gameObject.GetComponentInParent<EntityBehavior>();

            if (target is PlayerBehavior targetPlayer && attacker is EnemyBehavior)
            {
                targetPlayer.timesGotHit++;
                Debug.Log("got hit");

                if (targetPlayer.pulseState == PlayerBehavior.PulseState.DEAD)
                {
                    Destroy(targetPlayer.gameObject);  // TODO: make actual gameover screen
                    Time.timeScale = 0;
                    Debug.Log("YOU DIED FOR REALS");
                }

                if (knockbackRate > 0)  // if attack has knockback, target gets knocked back
                {
                    PlayerControls.isKnockedback = true;
                    StartCoroutine(PlayerControls.CooldownKnockback(0.5f));

                    Vector2 knockbackVector = collision.gameObject.transform.position - transform.position;
                    collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(
                        knockbackVector.normalized * knockbackRate, ForceMode2D.Impulse
                    );
                }
            }
            else if (target is EnemyBehavior enemy && attacker is PlayerBehavior attackerPlayer)
            {
                attackerPlayer.timesHit++;
                Debug.Log("hit");

                enemy.TakeDamage(damage);  // target takes damage

                if (knockbackRate > 0)  // if attack has knockback, target gets knocked back
                {
                    enemy.isKnockedback = true;
                    StartCoroutine(enemy.CooldownKnockback(1f));

                    Vector2 knockbackVector = collision.gameObject.transform.position - transform.position;
                    collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(
                        (int)attackerPlayer.pulseState * 0.2f * knockbackRate * knockbackVector.normalized, ForceMode2D.Impulse
                    );
                }
            }
        }
    }
}
