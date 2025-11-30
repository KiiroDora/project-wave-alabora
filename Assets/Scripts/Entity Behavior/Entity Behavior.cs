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
    [SerializeField] protected AudioClip[] attackAudioClips;
    [SerializeField] protected AudioClip[] hurtAudioClips;
    [SerializeField] protected AudioClip[] deadAudioClips;
    protected AudioSource audioSource; 


    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void Die()
    {
        StartCoroutine(WaitAndDie());
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

    public IEnumerator WaitAndDie()
    {
        audioSource.clip = deadAudioClips[Random.Range(0, deadAudioClips.Length)];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 0.2f);
        Destroy(gameObject);
    }

}
