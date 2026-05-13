using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_Vision : MonoBehaviour
{



    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform eyes;
    //[SerializeField] private float rayDistance = 20f;

    [Header("Field of View")]
    [SerializeField] private float viewAngle = 60f; 
    


    [Header("Sphere Cast arg")]
    [SerializeField] private float sphereRadius = 10f;
    [SerializeField] private int maxColliderSize = 10;
    [SerializeField] int detectedCollidersCount;
    private Collider[] _hitColliders;
    


    private void Awake()
    {
        _hitColliders = new Collider[maxColliderSize];

        if(eyes == null)
        {
            eyes = transform;
        } 
        
    }

    private void Start()
    {

        if(playerTransform == null)
        {
            GameObject _playerGO = GameObject.FindGameObjectWithTag("Player");
            playerTransform = _playerGO.transform;
        }  
        

    }

    
#region Sphere Scan 
    private  bool SphereScan()
    {
        Physics.SyncTransforms();
        
        detectedCollidersCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            sphereRadius ,
            _hitColliders,
            playerLayerMask);
        
        if (detectedCollidersCount > 0)
        {
            FieldOfViewScan();
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
    private bool FieldOfViewScan()
    {
        
        Vector3 directionToTarget = (playerTransform.position - transform.position).normalized;

        float distanceToTarget = Vector3.Distance(transform.position,playerTransform.position);  

        if(distanceToTarget > sphereRadius)
        {

            return false;
        } 

        float angleToTarget = Vector3.Angle(eyes.transform.forward,directionToTarget);

        if(angleToTarget < viewAngle / 2f)
        {
            RaycastHit hit;
            if(Physics.Raycast(eyes.transform.position, directionToTarget, out hit, distanceToTarget,playerLayerMask))
            {

                return true;
            }
        }

     
        return false;
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
