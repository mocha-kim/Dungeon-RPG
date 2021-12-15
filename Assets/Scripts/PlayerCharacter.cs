using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacter : MonoBehaviour
{
    #region Variables

    private CharacterController controller;
    [SerializeField] private LayerMask groundLayerMask;

    private NavMeshAgent agent;
    private Camera camera;

    [SerializeField] private Animator animator;

    readonly int moveHash = Animator.StringToHash("Move");
    //readonly int fallingHash = Animator.StringToHash("Falling");

    #endregion Variables

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

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
        {
            controller.Move(agent.velocity * Time.deltaTime);
            animator.SetBool(moveHash, true);
        }
        else
        {
            controller.Move(Vector3.zero);
            animator.SetBool(moveHash, false);
        }
    }

    private void LateUpdate()
    {
        animator.rootPosition = agent.nextPosition;
        transform.position = agent.nextPosition;
    }
}
