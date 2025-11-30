using System.Collections;
using UnityEngine;

public class PlayerBehavior : EntityBehavior
{
    public int timesGotHit = 0;
    public int timesHit = 2;

    public enum PulseState {DYING, CALMING, NORMAL, STIRRING, STORMING, RAGING};
    public PulseState pulseState = PulseState.NORMAL;

    public Animator animator;

    public float attackSpeed = 1;
    public float baseMoveSpeed = 8;
    public float baseJumpForce = 8;

    [SerializeField] private ParticleSystem pulseIndicator;


    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponentInChildren<AudioSource>();
        animator = GetComponent<Animator>();

        PlayerControls.jumpForce = baseJumpForce;
        PlayerControls.moveSpeed = baseMoveSpeed;

        DynamicAudioSwitcher.instance.SwitchAudioSeamless(DynamicAudioSwitcher.instance.dynamicBGM_Array[(int)pulseState]);
        StartCoroutine(CheckPulse(5f));
    }

    void Start()
    {
        TextController.UpdatePulseText(pulseState.ToString());
    }

    public void TakeDamage()
    {
        timesGotHit++;
        if (!audioSource.isPlaying) 
        {
            audioSource.clip = hurtAudioClips[Random.Range(0, hurtAudioClips.Length)];
            audioSource.Play();
        }
    }

    public override void Die()
    {
        GameController.loseTrigger?.Invoke();
        base.Die(); 
    }

    public override IEnumerator AttackCoroutine()
    {
        if (canAttack)
        {
            animator.SetBool("isAttacking", true);
            canAttack = false;
            yield return new WaitForSeconds(hitboxes[0].entryTime / attackSpeed);  // wait for windup
            hurtbox.enabled = false;  // invincibility frames start

            hitboxes[0].col2D.enabled = true;  // activate hitbox
            audioSource.clip = attackAudioClips[Random.Range(0, attackAudioClips.Length)];
            audioSource.Play();

            yield return new WaitForSeconds(hitboxes[0].contactTime);

            hurtbox.enabled = true;  // invincibility frames end
            hitboxes[0].col2D.enabled = false;  // deactivate hitbox

            yield return new WaitForSeconds(hitboxes[0].exitTime / attackSpeed);

            canAttack = true;
            animator.SetBool("isAttacking", false);
        }
    }

    public IEnumerator CheckPulse(float time)
    {
        yield return new WaitForSeconds(time);

        int pulse = Mathf.Clamp(timesHit - timesGotHit, 0, 999);

        if (pulse > 5)
        {
            pulseState++;
            if ((int)pulseState > 5) { pulseState = PulseState.RAGING; }
            else 
            {
                DynamicAudioSwitcher.instance.SwitchAudioSeamless(DynamicAudioSwitcher.instance.dynamicBGM_Array[(int)pulseState]);
            }
            var shape = pulseIndicator.shape;
            shape.scale = new Vector3(0.5f * (int)pulseState, 1, 1);  
        }
        else if (pulse >= 3)
        {
        }
        else
        {
            pulseState--;
            if ((int)pulseState < 0) { pulseState = PulseState.DYING; }
            else 
            {
                DynamicAudioSwitcher.instance.SwitchAudioSeamless(DynamicAudioSwitcher.instance.dynamicBGM_Array[(int)pulseState]);
            }
            var shape = pulseIndicator.shape;
            shape.scale = new Vector3(0.5f * (int)pulseState, 1, 1);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sea"))
        {
            Die();
        }
    }

    public override void Attack()
    {
        base.Attack();
    }
}
