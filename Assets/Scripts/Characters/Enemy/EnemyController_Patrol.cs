using UnityEngine;
using System.Collections;
using RPG.AI;

namespace RPG.Characters
{
    public class EnemyController_Patrol : EnemyController, IDamagable, IAttackable
    {
        #region Variables

        public Collider weaponCollider;
        public Transform hitPoint;
        public GameObject hitEffect = null;

        public Transform[] waypoints;

        public float maxHealth = 100f;
        private float health;

        private int hashHit = Animator.StringToHash("Hit");
        private int hashIsAlive = Animator.StringToHash("IsAlive");

        #endregion Variables

        protected override void Start()
        {
            base.Start();

            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new MoveToWaypoints());
        }

        public override bool IsAvailableAttack
        {
            get
            {
                if (!Target)
                {
                    return false;
                }

                float distance = Vector3.Distance(transform.position, Target.position);
                return (distance <= AttackRange);
            }
        }

        public void EnableAttackCollider()
        {
            Debug.Log("Check Attack Event");
            if (weaponCollider)
                weaponCollider.enabled = true;

            StartCoroutine("DisableAttackCollider");
        }

        IEnumerator DisableAttackCollider()
        {
            yield return new WaitForFixedUpdate();

            if (weaponCollider)
                weaponCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != weaponCollider)
                return;

            if (((1 << other.gameObject.layer) & TargetMask) != 0)
            {
                //It matched one
                Debug.Log("Attack Trigger: " + other.name);
                PlayerCharacter playerCharacter = other.gameObject.GetComponent<PlayerCharacter>();
                playerCharacter?.TakeDamage(10, hitEffect);

            }
        }

        #region IDamagable

        public bool IsAlive => (health > 0);

        public void TakeDamage(int damage, GameObject hitEffectPrefab)
        {
            if (!IsAlive)
            {
                return;
            }

            health -= damage;

            if (hitEffectPrefab)
            {
                Instantiate(hitEffectPrefab, hitPoint);
            }

            if (IsAlive)
            {
                animator?.SetTrigger(hashHit);
            }
            else
            {
                animator?.SetBool(hashIsAlive, false);

                Destroy(gameObject, 3.0f);
            }
        }

        public void OnExcuteAttack(int attackIndex)
        {
            throw new System.NotImplementedException();
        }

        #endregion IDamagable

        #region IAttakable

        public AttackBehaviour CurrentAttackBehaviour
        {
            get;
            private set;
        }



        #endregion IAttackable
    }

}