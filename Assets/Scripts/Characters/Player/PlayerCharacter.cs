using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{

    public class PlayerCharacter : MonoBehaviour, IAttackable, IDamagable
    {
        #region Variables

        private CharacterController controller;
        [SerializeField]
        private LayerMask groundLayerMask;

        private NavMeshAgent agent;
        private Animator animator;
        private new Camera camera;

        readonly int hashIsMoving = Animator.StringToHash("IsMoving");
        readonly int hashMoveSpeed = Animator.StringToHash("MoveSpeed");
        readonly int hashAttack = Animator.StringToHash("Attack");
        readonly int hashAttackIndex = Animator.StringToHash("AttackIndex");
        readonly int hashIsAlive = Animator.StringToHash("IsAlive");
        readonly int hashHit = Animator.StringToHash("Hit");
        //readonly int hashFalling = Animator.StringToHash("IsFalling");

        [SerializeField]
        private LayerMask targetMask;
        public Transform target;

        public bool IsInAttackState => GetComponent<AttackStateController>()?.IsInAttackState ?? false;
        [SerializeField]
        private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

        [SerializeField]
        private Transform hitPoint;

        public bool IsAlive => health > 0;

        public float maxHealth = 100f;
        private float health;

        public Collider weaponCollider;

        public AttackBehaviour CurrentAttackBehaviour
        {
            get;
            private set;
        }

        #endregion Variables

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;
            agent.updateRotation = true;

            camera = Camera.main;

            health = maxHealth;

            InitAttackBehaviour();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsAlive)
                return;

            // Get mouse left click
            if (Input.GetMouseButtonDown(0) && !IsInAttackState)
            {
                // Screen to world
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
                {
                    Debug.Log("Ray hit " + hit.collider.name + " " + hit.point);
                    RemoveTarget();

                    // Move character
                    agent.SetDestination(hit.point);
                }
            }
            // Get mouse right click
            else if (Input.GetMouseButtonDown(1))
            {
                // Screen to world
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.Log("We hit " + hit.collider.name + " " + hit.point);

                    IDamagable damagable = hit.collider.GetComponent<IDamagable>();
                    if (damagable != null && damagable.IsAlive)
                    {
                        SetTarget(hit.collider.transform);
                    }
                }
            }

            if (target != null)
            {
                if (!(target.GetComponent<IDamagable>()?.IsAlive ?? false))
                    RemoveTarget();
                else
                {
                    agent.SetDestination(target.position);
                    FaceToTarget();
                }
            }

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                controller.Move(agent.velocity * Time.deltaTime);
                animator.SetFloat(hashMoveSpeed, agent.velocity.magnitude / agent.speed, .1f, Time.deltaTime);
                animator.SetBool(hashIsMoving, true);
            }
            else
            {
                controller.Move(Vector3.zero);
                if (!agent.pathPending)
                {
                    animator.SetFloat(hashMoveSpeed, 0);
                    animator.SetBool(hashIsMoving, false);
                    agent.ResetPath();
                }
            }

            AttackTarget();
        }

        private void LateUpdate()
        {
            animator.rootPosition = agent.nextPosition;
            transform.position = agent.nextPosition;
        }

        protected void OnAnimatorMove()
        {
            Vector3 position = transform.position;
            position.y = agent.nextPosition.y;

            animator.rootPosition = position;
            agent.nextPosition = position;
        }

        private void InitAttackBehaviour()
        {
            foreach (AttackBehaviour behaviour in attackBehaviours)
                behaviour.targetMask = targetMask;
        }

        private void CheckAttackBehaviour()
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

        private void SetTarget(Transform newTarget)
        {
            target = newTarget;

            agent.stoppingDistance = CurrentAttackBehaviour?.range ?? 0;
            agent.updatePosition = false;
            agent.SetDestination(newTarget.transform.position);
        }

        void RemoveTarget()
        {
            target = null;
            agent.stoppingDistance = 0f;
            agent.updateRotation = true;

            agent.ResetPath();
        }

        void AttackTarget()
        {
            if (CurrentAttackBehaviour == null)
            {
                return;
            }

            if (target != null && !IsInAttackState && CurrentAttackBehaviour.IsAvailable)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= CurrentAttackBehaviour?.range)
                {
                    animator.SetInteger(hashAttackIndex, CurrentAttackBehaviour.animationIndex);
                    animator.SetTrigger(hashAttack);
                }
            }
        }

        void FaceToTarget()
        {
            if (target)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f);
            }
        }

        #region IAttackable

        public void OnExcuteAttack(int attackIndex)
        {
            if (CurrentAttackBehaviour != null)
                CurrentAttackBehaviour.ExecuteAttack(target.gameObject);
        }

        #endregion IAttackable


        #region IDamagable

        public void TakeDamage(int damage, GameObject hitEffectPrefab)
        {
            if (!IsAlive)
            {
                return;
            }

            health -= damage;

            if (hitEffectPrefab)
            {
                Instantiate<GameObject>(hitEffectPrefab, hitPoint);
            }

            if (IsAlive)
            {
                animator?.SetTrigger(hashHit);
            }
            else
            {
                animator?.SetBool(hashIsAlive, false);
            }
        }

        #endregion IDamagable
    }

}