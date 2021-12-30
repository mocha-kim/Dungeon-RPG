using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    public abstract class AttackBehaviour : MonoBehaviour
    {
        #region Variables

#if UNITY_EDITOR
        [Multiline]
        public string devDescroption = "";
#endif

        public int animationIndex;
        public int priority;

        public int damage = 10;
        public float range = 3f;

        [SerializeField]
        protected float coolTime;
        protected float calcCooltime = 0.0f;

        public GameObject effectPrefab;
        [HideInInspector]
        public LayerMask targetMask;

        [SerializeField]
        public bool IsAvailable => calcCooltime >= coolTime;

        #endregion Variables

        // Start is called before the first frame update
        protected void Start()
        {
            calcCooltime = coolTime;
        }

        // Update is called once per frame
        protected void Update()
        {
            if (calcCooltime < coolTime)
            {
                calcCooltime += Time.deltaTime;
            }
        }

        public abstract void ExecuteAttack(GameObject target = null, Transform startPoint = null);
    }

}