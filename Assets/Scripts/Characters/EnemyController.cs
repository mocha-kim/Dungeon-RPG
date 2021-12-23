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
    }

}