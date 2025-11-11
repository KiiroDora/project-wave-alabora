using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class EntityBehavior : MonoBehaviour
{
    public int maxhp;
    public int hp;
    public bool canAttack = true;
    public UnityEvent OnDeath;
    public UnityEvent OnDamage;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Collider2D hurtbox;
    [SerializeField] protected Hitbox[] hitboxes;


    protected virtual void Awake()
    {
        hp = maxhp;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void TakeDamage(int damage)
    {
        hp = Mathf.Clamp(hp - damage, 0, 999);  // reduce hp (cannot be lower than 0)
        OnDamage?.Invoke();  // invoke damage events
        if (hp == 0)
        {
            Die();
        }
        Debug.Log("Took " + damage + " damage");
    }

    public virtual void Die()
    {
        OnDeath?.Invoke();  // invoke death events
    }

    public virtual IEnumerator AttackCoroutine()
    {
        yield return null;
    }
    
    public virtual void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

}
