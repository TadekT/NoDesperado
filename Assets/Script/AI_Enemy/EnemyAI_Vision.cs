using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Vision : MonoBehaviour
{

    public event Action OnPlayerInFovAction;


    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _eyes;
    //[SerializeField] private float rayDistance = 20f;

    [Header("Field of View")]
    [SerializeField] private float viewAngle = 60f; 
    


    [Header("Sphere Cast arg")]
    [SerializeField] private float sphereRadius = 10f;
    [SerializeField] private int maxColliderSize = 10;
    [SerializeField] private float scanInterval = 1f;
    [SerializeField] int scanColl;
    private Collider[] _hitColliders;
    
    private Coroutine CheckingTheSurroundingsReference;

    // bool variables
    private bool _playerInFov;

    private void Awake()
    {
        _hitColliders = new Collider[maxColliderSize];
        
    }

    private void Start()
    {
        if(_eyes == null)
        {
            Debug.Log("THER IS NO EYES TRANSFORM");
            _eyes = transform;
        } 
        if(_playerTransform == null)
        {
            GameObject _playerGO = GameObject.FindGameObjectWithTag("Player");
            _playerTransform = _playerGO.transform;
            Debug.Log(" I can't find a player GameObject in scene !!");
        }  
        CheckingTheSurroundingsReference = StartCoroutine(CheckingTheSurroundings());

    }


    private IEnumerator CheckingTheSurroundings()
    {
        while (true)
        {
            SphereScan();
            yield return new WaitForSecondsRealtime(scanInterval);
        }
    }
    
#region Sphere Scan 
    private  bool SphereScan()
    {
        Physics.SyncTransforms();
        
        scanColl = Physics.OverlapSphereNonAlloc(transform.position, sphereRadius ,_hitColliders,_playerLayer);
        
        if (scanColl > 0)
        {
            ConeScan();
            Debug.Log("Player IN SPHERE RANGE ");
            return true;
        }
        else
        {
            Debug.Log("Sphere find shiit ");
            return false;
        }
    }
#endregion


#region Cone Scan 
    private bool ConeScan()
    {
        
        Vector3 directionToTarget = (_playerTransform.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position,_playerTransform.position);
        
        if(distanceToTarget > sphereRadius) return false;

        float angleToTarget = Vector3.Angle(_eyes.transform.forward,directionToTarget);
        if(angleToTarget < viewAngle / 2f)
        {
            RaycastHit hit;

            if(Physics.Raycast(_eyes.transform.position, directionToTarget, out hit, distanceToTarget,_playerLayer))
            {
                    if (!_playerInFov)
                    {
                        Debug.Log("Player in FOV");
                        _playerInFov = true;
                        QuestionMarkPlayerVisableInFov();
                        return true;
                    }
            _playerInFov = false;
            return false;
            }
        }

        Debug.Log("No player in FOV");
        return false;
    }
#endregion

#region Question Mar invoke funcion
    private void QuestionMarkPlayerVisableInFov()
    {
        Debug.Log("Player in FOV signal");
        OnPlayerInFovAction?.Invoke();
    }
#endregion

#region OnDrawGizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,sphereRadius);


        Vector3 forward = transform.forward * sphereRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * forward;
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position + rightBoundary, transform.position + leftBoundary);

    }
#endregion


}
