using System.Collections;
using System.Collections.Generic;
using RPG.Characters;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public float damageInterval = 0.5f;
    public float damageDuration = 5f;
    public int damage = 5;

    private float calcDuration = 0.0f;

    [SerializeField]
    private ParticleSystem effect;

    public Animator animator;
    private int hashPop = Animator.StringToHash("Pop");

    private IDamagable damagable;

    private void Update()
    {
        if (damagable != null)
            calcDuration -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trap");
        damagable = other.GetComponent<IDamagable>();
        if (damagable != null)
        {
            calcDuration = damageDuration;

            if (effect != null)
                effect.Play();
            StartCoroutine(ProcessDamage());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        damagable = null;
        StopAllCoroutines();
        if (effect != null)
            effect.Stop();
    }

    IEnumerator ProcessDamage()
    {
        while (calcDuration > 0 && damagable != null)
        {
            damagable.TakeDamage(damage, null);
            animator.SetTrigger(hashPop);

            yield return new WaitForSeconds(damageInterval);
        }

        damagable = null;
        if (effect != null)
            effect.Stop();
    }
}
