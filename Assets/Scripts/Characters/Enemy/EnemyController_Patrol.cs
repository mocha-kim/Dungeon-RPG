using UnityEngine;
using System.Collections;
using RPG.AI;

namespace RPG.Characters
{
    public class EnemyController_Patrol : EnemyController_Basic
    {
        #region Variables


        #endregion Variables

        protected override void Start()
        {
            base.Start();

            stateMachine.AddState(new MoveToWaypoints());
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
                playerCharacter?.TakeDamage(10, hitEffectPrefab);

            }
        }
    }

}