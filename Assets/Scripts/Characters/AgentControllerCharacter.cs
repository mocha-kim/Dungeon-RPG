using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{

    public class AgentControllerCharacter : MonoBehaviour
    {
        #region Variables

        public LayerMask groundLayerMask;
        public float groundCheckDistance = 0.3f;

        private CharacterController characterController;
        private NavMeshAgent agent;
        private new Camera camera;

        #endregion Variables

        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;
            agent.updateRotation = true;

            camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            // Get mouse left click
            if (Input.GetMouseButtonDown(0))
            {
                // Screen to world
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
                {
                    Debug.Log("Ray hit " + hit.collider.name + " " + hit.point);

                    // Move character
                    agent.SetDestination(hit.point);
                }
            }

            if (agent.remainingDistance > agent.stoppingDistance)
                characterController.Move(agent.velocity * Time.deltaTime);
            else
                characterController.Move(Vector3.zero);
        }

        private void LateUpdate()
        {
            transform.position = agent.nextPosition;
        }
    }

}