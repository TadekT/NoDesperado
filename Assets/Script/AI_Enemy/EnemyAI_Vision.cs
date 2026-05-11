using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Vision : MonoBehaviour
{

    public event Action OnPlayerInFovAction;


    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform eyes;
    //[SerializeField] private float rayDistance = 20f;

    [Header("Field of View")]
    [SerializeField] private float viewAngle = 60f; 
    


    [Header("Sphere Cast arg")]
    [SerializeField] private float sphereRadius = 10f;
    [SerializeField] private int maxColliderSize = 10;
    [SerializeField] private float scanInterval = 1f;
    [SerializeField] int detectedCollidersCount;
    private Collider[] _hitColliders;
    
    private Coroutine CheckingTheSurroundingsReference;

    // bool variables
    public bool IsPlayerInFieldOfView;
    public bool IsPlayerOutOfFieldOfView;

    private void Awake()
    {
        _hitColliders = new Collider[maxColliderSize];
        
    }

    private void Start()
    {
        if(eyes == null)
        {
            Debug.Log("THER IS NO EYES TRANSFORM");
            eyes = transform;
        } 
        if(playerTransform == null)
        {
            GameObject _playerGO = GameObject.FindGameObjectWithTag("Player");
            playerTransform = _playerGO.transform;
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
        
        detectedCollidersCount = Physics.OverlapSphereNonAlloc(transform.position, sphereRadius ,_hitColliders, playerLayerMask);
        
        if (detectedCollidersCount > 0)
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
        
        Vector3 directionToTarget = (playerTransform.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position,playerTransform.position);
        
        if(distanceToTarget > sphereRadius) return false;

        float angleToTarget = Vector3.Angle(eyes.transform.forward,directionToTarget);
        if(angleToTarget < viewAngle / 2f)
        {
            RaycastHit hit;

            if(Physics.Raycast(eyes.transform.position, directionToTarget, out hit, distanceToTarget,playerLayerMask))
            {
                    if (!IsPlayerInFieldOfView)
                    {
                        Debug.Log("Player in FOV");
                        IsPlayerInFieldOfView = true;
                        OnPlayerEnteredFOV();
                        return true;
                    }
            
            IsPlayerInFieldOfView = false;
            return false;
            }
        }

        Debug.Log("No player in FOV");
        return false;
    }
#endregion


#region 
    private void OnPlayerEnteredFOV()
    {
        Debug.Log("Player in  Field Of View signal");
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
