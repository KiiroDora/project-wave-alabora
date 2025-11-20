using System.Collections;
using UnityEngine;

public class EnemyBehavior : EntityBehavior
{
    Rigidbody2D rb2d;
    [SerializeField] private Transform groundCheckRight;
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private Transform groundCheckMiddle;
    [SerializeField] LayerMask groundLayer;
    private Transform target;
    Vector2 relativePosition;

    public int maxhp = 50;
    public int hp;
    [SerializeField] private float moveSpeed = 2;
    public bool isKnockedback = false;
    public float selfKnockbackMultiplier = 1f;


    protected override void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb2d = GetComponent<Rigidbody2D>();

        moveSpeed += Random.Range(0, 0.2f);
        hp = maxhp;
        base.Awake();
    }

    void FixedUpdate()
    {
        relativePosition = target.position - transform.position;

        if (relativePosition.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        bool isInAttackRange = IsInAttackRange();
        bool canWalk = IsWalkableAhead() && !isInAttackRange;
        if (IsGrounded())
        {
            if (isInAttackRange)
            {
                Attack();
            }
            else if (canWalk && !isKnockedback)
            {
                if (Mathf.Abs(relativePosition.x) > 2f && Mathf.Abs(relativePosition.x) < 10f)
                {
                    rb2d.linearVelocityX = relativePosition.normalized.x * moveSpeed;
                }
            }

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sea"))
        {
            Die();
        }
    }

    public bool IsWalkableAhead()
    {
        // check if the area just ahead is walkable or empty space
        Transform groundCheck = spriteRenderer.flipX ? groundCheckLeft : groundCheckRight;  // use the appropriate ground check position
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    public bool IsGrounded()
    {
        // check if the area just below the player overlaps with any collider on the Ground layer
        return Physics2D.OverlapCapsule(groundCheckMiddle.position, new Vector2(1, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    public bool IsInAttackRange()
    {
        bool isPlayerInRange = Mathf.Abs(relativePosition.x) <= 1.5f;
        return target.GetComponent<PlayerControls>().IsGrounded() && isPlayerInRange;
    }

    public void TakeDamage(int damage)
    {
        hp = Mathf.Clamp(hp - damage, 1, 999);  // reduce hp (cannot be lower than 0)
        OnDamage?.Invoke();  // invoke damage events
        selfKnockbackMultiplier = 1 + 1 / hp; 
    }

    public override void Die()
    {
        GameController.enemies.Remove(gameObject);
        if (GameController.enemies.Count <= 0 && EnemySpawner.allEnemiesSpawned)
        {
            GameController.winTrigger?.Invoke();
        }
        Destroy(gameObject); // temporary
        // TODO: stuff to add for Enemy's OnDeath -> Play animation then Destroy gameobject in animation's OnExit
        base.Die();  // handles death here
    }

    public virtual IEnumerator CooldownKnockback(float time)
    {
        yield return new WaitForSeconds(time);
        isKnockedback = false;
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
            yield return new WaitForSeconds(hitboxToUse.entryTime);  // wait for windup

            hitboxToUse.col2D.enabled = true;  // activate hitbox
            temporaryImage.color = Color.red;
                
            yield return new WaitForSeconds(hitboxToUse.contactTime);
                
            hitboxToUse.col2D.enabled = false;  // deactivate hitbox
            temporaryImage.color = Color.blue;
                
            yield return new WaitForSeconds(hitboxToUse.exitTime);

            temporaryImage.enabled = false;

            canAttack = true;
        }
    }

    public override void Attack()
    {
        base.Attack();
    }
}
