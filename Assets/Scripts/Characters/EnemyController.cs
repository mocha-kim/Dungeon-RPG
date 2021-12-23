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

        public LayerMask targetMask;
        public Transform target;
        public float viewRadius = 5f;
        public float AttackRange = 1.5f;

        #endregion Variables

        // Start is called before the first frame update
        void Start()
        {
            stateMachine = new StateMachine<EnemyController>(this, new IdleState());
            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());
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
                if (!target)
                    return false;

                float distance = Vector3.Distance(transform.position, target.position);
                return (distance <= AttackRange);
            }
        }

        public Transform SearchEnemy()
        {
            target = null;

            Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
            if (targetInViewRadius.Length > 0)
                target = targetInViewRadius[0].transform;

            return target;
        }
    }

}