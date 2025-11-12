using System.Collections;
using UnityEngine;

public class EnemyBehavior : EntityBehavior
{
    Rigidbody2D rb2d;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private Transform groundCheckRight;
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private Transform groundCheckMiddle;
    [SerializeField] LayerMask groundLayer;
    private Transform target;
    Vector2 relativePosition;

    protected override void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        maxhp = 30;
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
                if (Mathf.Abs(relativePosition.x) > 2f)
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

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        base.Die();  // handles death here
        // TODO: stuff to add for Enemy's OnDeath -> Play animation then Destroy gameobject in animation's OnExit
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
            Debug.Log("Enemy Attack Start!");
            yield return new WaitForSeconds(hitboxToUse.entryTime);  // wait for windup

            hitboxToUse.col2D.enabled = true;  // activate hitbox
                
            Debug.Log("Enemy Contacting...");
            yield return new WaitForSeconds(hitboxToUse.contactTime);
                
            hitboxToUse.col2D.enabled = false;  // deactivate hitbox
                
            Debug.Log("Enemy Attack finished");
            yield return new WaitForSeconds(hitboxToUse.exitTime);

            canAttack = true;
        }
    }

    public override void Attack()
    {
        base.Attack();
    }
}
