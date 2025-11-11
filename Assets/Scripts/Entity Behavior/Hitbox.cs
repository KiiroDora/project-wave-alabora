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
            collision.gameObject.GetComponentInParent<EntityBehavior>().TakeDamage(damage);  // target takes damage

            if (knockbackRate > 0)  // if attack has knockback, target gets knocked back
            {
                collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(
                    (collision.gameObject.transform.position - transform.position).normalized * knockbackRate, ForceMode2D.Impulse
                );
            }
        }
    }
}
