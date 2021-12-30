using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.AI;

namespace RPG.Characters
{
    public class EnemyController_Basic : EnemyController, IDamagable, IAttackable
    {
        #region Variables

        public Transform hitPoint;
        public Transform attackPoint;

        [SerializeField]
        protected List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

        public Collider weaponCollider;
        public GameObject hitEffectPrefab = null;

        public float maxHealth = 100f;
        protected float health;

        protected int hashHit = Animator.StringToHash("Hit");
        protected int hashIsAlive = Animator.StringToHash("IsAlive");

        #endregion Variables

        public override bool IsAvailableAttack
        {
            get
            {
                if (!Target)
                    return false;
                float distance = Vector3.Distance(transform.position, Target.position);
                return (distance <= AttackRange);
            }
        }

        protected override void Start()
        {
            base.Start();

            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());
            stateMachine.AddState(new DeadState());
            InitAttackBehaviour();
        }

        protected void OnAnimatorMove()
        {
            Vector3 position = transform.position;
            position.y = agent.nextPosition.y;

            animator.rootPosition = position;
            agent.nextPosition = position;
        }

        protected void InitAttackBehaviour()
        {
            foreach (AttackBehaviour behaviour in attackBehaviours)
            {
                if (CurrentAttackBehaviour == null)
                    CurrentAttackBehaviour = behaviour;
                behaviour.targetMask = TargetMask;
            }
        }

        protected void CheckAttackBehavior()
        {
            if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable)
            {
                CurrentAttackBehaviour = null;

                foreach (AttackBehaviour behaviour in attackBehaviours)
                {
                    if (behaviour.IsAvailable)
                    {
                        if ((CurrentAttackBehaviour == null) || (CurrentAttackBehaviour.priority < behaviour.priority))
                            CurrentAttackBehaviour = behaviour;
                    }
                }
            }
        }

        #region IDamagable

        public bool IsAlive => (health > 0);

        public void TakeDamage(int damage, GameObject hitEffectPrefab)
        {
            if (!IsAlive)
                return;

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
                stateMachine.ChangeState<DeadState>();
            }
        }

        #endregion IDamagable

        #region IAttakable

        public AttackBehaviour CurrentAttackBehaviour
        {
            get;
            private set;
        }

        public void OnExcuteAttack(int attackIndex)
        {
            if (CurrentAttackBehaviour != null && Target != null)
                CurrentAttackBehaviour.ExecuteAttack(Target.gameObject, attackPoint);
        }

        #endregion IAttackable
    }

}