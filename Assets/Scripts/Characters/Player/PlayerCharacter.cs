using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Characters
{

    public class PlayerCharacter : MonoBehaviour, IAttackable, IDamagable
    {
        #region Variables
        public MovePoint picker;

        private CharacterController controller;
        [SerializeField]
        private LayerMask groundLayerMask;
        [SerializeField]
        protected NPCBattleUI battleUI;

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

        [SerializeField]
        private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

        [SerializeField]
        private Transform hitPoint;

        public float maxHealth = 100f;
        public float health;

        public float attackRange = 1.5f;
        public Collider weaponCollider;

        public InventoryObject inventory;

        #endregion Variables

        #region Properties

        public bool IsAlive => health > 0;
        public bool IsInAttackState => GetComponent<AttackStateController>()?.IsInAttackState ?? false;
        public AttackBehaviour CurrentAttackBehaviour
        {
            get;
            private set;
        }

        #endregion Properties

        #region Main Methods

        // Start is called before the first frame update
        void Start()
        {
            inventory.OnUseItem += OnUseItem;

            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            agent.updatePosition = false;
            agent.updateRotation = true;

            camera = Camera.main;

            health = maxHealth;
            attackRange = CurrentAttackBehaviour?.range ?? 1.5f;

            if (battleUI)
            {
                battleUI.MinValue = 0.0f;
                battleUI.MaxValue = maxHealth;
                battleUI.CurValue = health;
            }

            InitAttackBehaviour();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsAlive)
                return;

            bool isOnUI = EventSystem.current.IsPointerOverGameObject();

            // Get mouse left click
            if (!isOnUI && Input.GetMouseButtonDown(0) && !IsInAttackState)
            {
                // Screen to world
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, targetMask))
                {
                    Debug.Log("Target set " + hit.collider.name + " " + hit.point);

                    picker.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    if (picker)
                        picker.target = hit.collider.transform;

                    IDamagable damagable = hit.collider.GetComponent<IDamagable>();
                    if (damagable != null && damagable.IsAlive)
                        SetTarget(hit.collider.transform, CurrentAttackBehaviour?.range ?? 0);

                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable != null)
                        SetTarget(hit.collider.transform, interactable.Distance);
                }
                else if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
                {
                    Debug.Log("Ray hit " + hit.collider.name + " " + hit.point);
                    RemoveTarget();

                    // Move character
                    agent.SetDestination(hit.point);

                    picker.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    if (picker)
                        picker.SetPosition(hit);
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

                if (target != null)
                {
                    if (target.GetComponent<IInteractable>() != null)
                    {
                        IInteractable interactable = target.GetComponent<IInteractable>();
                        if (interactable.Interact(this.gameObject))
                            RemoveTarget();
                    }
                    else if (target.GetComponent<IDamagable>() != null)
                    {
                        AttackTarget();
                    }
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

#endregion Main Methods

        #region Helper Methods

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

        private void SetTarget(Transform newTarget, float stoppingDistance)
        {
            target = newTarget;

            agent.stoppingDistance = stoppingDistance;
            agent.updatePosition = false;
            agent.SetDestination(newTarget.transform.position);

            if (picker)
                picker.target = newTarget.transform;
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

        #endregion Helper Methods

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

            if (battleUI)
            {
                battleUI.CurValue = health;
                battleUI.CreateDamageText(damage);
            }

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

        #region Inventory

        private void OnUseItem(ItemObject itemObject)
        {
            foreach (ItemBuff buff in itemObject.data.buffs)
            {
                if (buff.status == CharacterAttribute.HP)
                    this.health += buff.value;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponent<GroundItem>();
            if (item)
            {
                if (inventory.AddItem(new Item(item.itemObject), 1))
                    Destroy(other.gameObject);
            }
        }

        public bool PickupItem(PickupItem item, int amount = 1)
        {
            if (item.itemObject != null && inventory.AddItem(new Item(item.itemObject), amount))
            {
                Destroy(item.gameObject);
                return true;
            }
            return false;
        }

        #endregion Inventory
    }

}