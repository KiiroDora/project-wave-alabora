using System.Collections;
using UnityEngine;

public class EnemyBehavior : EntityBehavior
{
    [SerializeField] private Transform groundCheckRight;
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private Transform groundCheckMiddle;
    [SerializeField] LayerMask groundLayer;
    private Transform target;
    Vector2 relativePosition;

    public int maxhp;
    public int hp;
    [SerializeField] private float moveSpeed = 2;


    protected override void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        moveSpeed += Random.Range(0, 0.2f);
        maxhp = 30;
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
            else if (canWalk)
            {
                if (Mathf.Abs(relativePosition.x) > 2f && Mathf.Abs(relativePosition.x) < 4f)
                {
                    transform.Translate(relativePosition.normalized.x * moveSpeed * Time.deltaTime, 0, 0);
                }
            }

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
        return IsWalkableAhead() && target.GetComponent<PlayerControls>().IsGrounded() && isPlayerInRange;
    }

    public void TakeDamage(int damage)
    {
        hp = Mathf.Clamp(hp - damage, 0, 999);  // reduce hp (cannot be lower than 0)
        OnDamage?.Invoke();  // invoke damage events
        if (hp == 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Destroy(gameObject); // temporary
        // TODO: stuff to add for Enemy's OnDeath -> Play animation then Destroy gameobject in animation's OnExit
        base.Die();  // handles death here
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
            yield return new WaitForSeconds(hitboxToUse.entryTime);  // wait for windup

            hitboxToUse.col2D.enabled = true;  // activate hitbox
                
            yield return new WaitForSeconds(hitboxToUse.contactTime);
                
            hitboxToUse.col2D.enabled = false;  // deactivate hitbox
                
            yield return new WaitForSeconds(hitboxToUse.exitTime);

            canAttack = true;
        }
    }

    public override void Attack()
    {
        base.Attack();
    }
}
