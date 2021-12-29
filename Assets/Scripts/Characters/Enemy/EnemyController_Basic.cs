﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.AI;

namespace RPG.Characters
{
    public class EnemyController_Basic : EnemyController, IDamagable, IAttackable
    {
        #region Variables

        public Transform projectilePoint;
        [SerializeField]
        private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();
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
            stateMachine.AddState(new AttackState());
            stateMachine.AddState(new DeadState());
            InitAttackBehaviour();
        }

        protected override void Update()
        {

            base.Update();
        }

        private void InitAttackBehaviour()
        {
            foreach (AttackBehaviour behaviour in attackBehaviours)
            {
                if (CurrentAttackBehaviour == null)
                    CurrentAttackBehaviour = behaviour;
                behaviour.targetMask = TargetMask;
            }
        }

        private void CheckAttackBehaviour()
        {
            if ((CurrentAttackBehaviour == null) || !(CurrentAttackBehaviour.IsAvailable))
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
                animator?.SetBool(hashIsAlive, false);

                Destroy(gameObject, 3.0f);
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
            {
                CurrentAttackBehaviour.ExecuteAttack(Target.gameObject, projectilePoint);
            }
        }

        #endregion IAttackable
    }

}