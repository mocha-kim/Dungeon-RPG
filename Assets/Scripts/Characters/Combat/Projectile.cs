using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class Projectile : MonoBehaviour
{
    #region Variables

    public float speed;

    public GameObject muzzlePrefab;
    public GameObject hitPrefab;

    public AudioClip shotSFX;
    public AudioClip hitSFX;

    private bool isCollided = false;
    private new Rigidbody rigidbody;

    [HideInInspector]
    public AttackBehaviour attackBehaviour;

    [HideInInspector]
    public GameObject owner;
    [HideInInspector]
    public GameObject target;

    #endregion Variables

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (target)
        {
            Vector3 dest = target.transform.position;
            dest.y += 1.2f;
            transform.LookAt(dest);
        }

        if (owner)
        {
            Collider projectileCollider = GetComponent<Collider>();
            Collider[] ownerColliders = owner.GetComponentsInChildren<Collider>();

            //foreach (Collider collider in ownerColliders)
            //    Physics.IgnoreCollision(projectileCollider, collider);

            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
        }

        rigidbody = GetComponent<Rigidbody>();

        if (muzzlePrefab)
        {
            GameObject muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            if (muzzleVFX)
            {
                muzzleVFX.transform.forward = gameObject.transform.forward;
                ParticleSystem particleSystem = muzzleVFX.GetComponent<ParticleSystem>();
                if (particleSystem)
                {
                    Destroy(muzzleVFX, particleSystem.main.duration);
                }
                else
                {
                    ParticleSystem childparticleSystem = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    if (childparticleSystem)
                    {
                        Destroy(muzzleVFX, particleSystem.main.duration);
                    }
                }
            }

            if (shotSFX != null && GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().PlayOneShot(shotSFX);
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (speed != 0 && rigidbody != null)
        {
            rigidbody.position += transform.forward * (speed * Time.deltaTime);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (isCollided)
            return;

        isCollided = true;

        Collider projectileCollider = GetComponent<Collider>();
        projectileCollider.enabled = false;

        if (hitSFX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(hitSFX);
        }

        speed = 0;
        rigidbody.isKinematic = true; // collision calculation disable

        ContactPoint contact = collision.contacts[0];
        Quaternion contactRotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 contactPosition = contact.point;

        if (hitPrefab)
        {
            GameObject hitVFX = Instantiate(hitPrefab, contactPosition, contactRotation);

            ParticleSystem particleSystem = hitVFX.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                Destroy(hitVFX, particleSystem.main.duration);
            }
            else
            {
                ParticleSystem childparticleSystem = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                if (childparticleSystem)
                {
                    Destroy(hitVFX, particleSystem.main.duration);
                }
            }
        }

        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(attackBehaviour?.damage ?? 0, null);
        }
        StartCoroutine(DestroyParticle(0.1f));
    }

    public IEnumerator DestroyParticle(float waitTime)
    {
        if (transform.childCount > 0 && waitTime != 0)
        {
            List<Transform> childs = new List<Transform>();

            foreach (Transform t in transform.GetChild(0).transform)
            {
                childs.Add(t);
            }

            while (transform.GetChild(0).localScale.x > 0)
            {
                yield return new WaitForSeconds(0.01f);

                transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                for (int i = 0; i < childs.Count; ++i)
                    childs[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            }
        }

        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

}