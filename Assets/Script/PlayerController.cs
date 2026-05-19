using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 mouse_position;

    [SerializeField] private Vector3 mouse_world_position;
    
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    
    
    [SerializeField] private LayerMask groundlayer;

    
    
    private void Awake()
    {
        if(agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            Debug.Log("NavMeshAgent component not assigned, trying to get it from the GameObject.");
        }



    }

    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {   
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            agent.SetDestination(getMouseWorldPosition());
        }
    }

    private Vector3 getMouseWorldPosition()
    {
        mouse_position = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouse_position);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, groundlayer))
        {
            //Debug.Log("Hit: " + hit.point);
            return hit.point;
        }

        return Vector3.zero;
    }






}   
