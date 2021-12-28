using System.Collections;
using System.Collections.Generic;
using Unity_RPG.AI;
using UnityEngine;


namespace Unity_RPG.Characters
{

    public class EnemyController : MonoBehaviour
    {
        #region Variables

        protected StateMachine<EnemyController> stateMachine;
        public StateMachine<EnemyController> StateMachine => stateMachine;

        private FieldOfView fov;

        //public LayerMask targetMask;
        //public Transform target;
        //public float viewRadius = 5f;
        public float attackRange = 1.5f;
        public Transform Target => fov?.NearestTarget;
        public Transform[] waypoints;
        [HideInInspector]
        public Transform targetWaypoint = null;
        private int waypointIndex = 0;

        #endregion Variables

        // Start is called before the first frame update
        void Start()
        {
            stateMachine = new StateMachine<EnemyController>(this, new MoveToWaypoints());
            IdleState idleState = new IdleState();
            idleState.isPatrol = true;
            stateMachine.AddState(idleState);
            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());

            fov = GetComponent<FieldOfView>();
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.Update(Time.deltaTime);
        }

        public bool IsAvailableAttack
        {
            get
            {
                if (!Target)
                    return false;

                float distance = Vector3.Distance(transform.position, Target.position);
                return (distance <= attackRange);
            }
        }

        public Transform SearchEnemy()
        {
            //    target = null;

            //    Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
            //    if (targetInViewRadius.Length > 0)
            //        target = targetInViewRadius[0].transform;

            //    return target;

            return Target;
        }

        public Transform FindNextWaypoint()
        {
            targetWaypoint = null;
            if (waypoints.Length > 0)
            {
                targetWaypoint = waypoints[waypointIndex];
            }

            waypointIndex = (waypointIndex + 1) % waypoints.Length;

            return targetWaypoint;
        }
    }

}