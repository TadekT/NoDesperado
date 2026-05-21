using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Camera _camera;
    
    [SerializeField] private LayerMask groundlayer;

    
    
    private void Awake()
    {
        if(agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        if(_camera == null)
        {
            _camera = Camera.main;
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
        Vector3 mouse_position = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mouse_position);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, groundlayer))
        {
            //Debug.Log("Hit: " + hit.point);
            return hit.point;
        }

        return Vector3.zero;
    }






}   
