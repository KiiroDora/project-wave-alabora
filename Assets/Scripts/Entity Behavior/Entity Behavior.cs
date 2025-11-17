using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class EntityBehavior : MonoBehaviour
{

    public bool canAttack = true;
    public UnityEvent OnDeath;
    public UnityEvent OnDamage;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Collider2D hurtbox;
    [SerializeField] protected Hitbox[] hitboxes;


    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
