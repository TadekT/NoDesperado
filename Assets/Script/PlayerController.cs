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
        if(Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if(TryGetMouseWorldPosition(out Vector3 target))
            {
                agent.SetDestination(target);
            }
        }
    }

    private bool TryGetMouseWorldPosition(out Vector3 position)
    {
        Vector3 mouse_position = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mouse_position);

        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, groundlayer))
        {
            position = hit.point;
            return true;
        }

        position = Vector3.zero;
        return false;
    }






}   
