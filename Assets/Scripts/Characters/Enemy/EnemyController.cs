using System.Collections;
using System.Collections.Generic;
using RPG.AI;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{

    public abstract class EnemyController : MonoBehaviour
    {
        #region Variables

        protected StateMachine<EnemyController> stateMachine;
        protected FieldOfView fov;
        protected NavMeshAgent agent;
        protected Animator animator;

        public virtual float AttackRange => 3.0f;
        public virtual bool IsAvailableAttack => false;

        #endregion Variables

        #region Properties

        public Transform Target => fov.NearestTarget;
        public LayerMask TargetMask => fov.targetMask;

        #endregion Properties

        // Start is called before the first frame update
        protected virtual void Start()
        {
            stateMachine = new StateMachine<EnemyController>(this, new IdleState());

            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;
            agent.updateRotation = true;

            animator = GetComponent<Animator>();

            fov = GetComponent<FieldOfView>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            stateMachine.Update(Time.deltaTime);
            if (!(stateMachine.Currentstate is MoveState))
                FaceTarget();
        }

        void FaceTarget()
        {
            if (Target)
            {
                Vector3 direction = (Target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }

        public R ChangeState<R>() where R : State<EnemyController>
        {
            return stateMachine.ChangeState<R>();
        }
    }

}