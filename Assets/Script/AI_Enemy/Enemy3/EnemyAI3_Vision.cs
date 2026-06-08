using UnityEngine;

public class EnemyAI3_Vision : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform eyes;
    [SerializeField] private Transform playerTransform;


    [Header("Layers")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask obstacleLayerMask;

    [Header("Field of View")]
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 60f; 
    [SerializeField] private float scanInterval = 0.2f;
    

    private float scanTimer;

    public bool IsAlerted{get ; set ;}
    public bool CanSeePlayer { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public Vector3 LastKnownPlayerPosition { get; private set; }

    public LayerMask ObstacleLayerMask => obstacleLayerMask;

    private void Awake()
    {

        if(eyes == null)
        {
            eyes = transform;
        } 
        
        if(playerTransform == null)
        {
            GameObject _playerGO = GameObject.FindGameObjectWithTag("Player");
            if(_playerGO != null)
            {
                playerTransform = _playerGO.transform;
            }
        }  

        
    }

    private void Update()
    {
        scanTimer -= Time.deltaTime;

        if (scanTimer > 0f)
            return;

        scanTimer = scanInterval;
        CheckPlayerVisibility ();
    }

    #region Check Player Visibility 
    private void CheckPlayerVisibility ()
    {
        if(playerTransform == null) return;

        if (IsTargetVisible(playerTransform))
        {
            CanSeePlayer = true;
            PlayerTransform = playerTransform;
            LastKnownPlayerPosition = playerTransform.position;
            return;
        }
        else
        {

        CanSeePlayer = false;
        PlayerTransform = null;
            
        }
        

    }
#endregion



#region Is Target Visible in FOV
    private bool IsTargetVisible(Transform target)
    {
        if (target == null)
            return false;

        Vector3 origin = eyes.position;
        Vector3 targetPosition = target.position + Vector3.up;

        Vector3 directionToTarget = targetPosition - origin;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > viewRadius)
            return false;

        if (!IsAlerted)
        {            
            float angleToTarget = Vector3.Angle(eyes.forward, directionToTarget.normalized);
            if (angleToTarget > viewAngle / 2f)
                return false;
        }
        
        PlayerStatus playerStatus = target.GetComponent<PlayerStatus>();
        if(playerStatus != null && playerStatus.IsHidden)
        {
            return false;
        }

        /*
        Sprawdzamy czy wystrzelony Raycast trafi w obstacleLayerMask(przeszkody zaznaczone w insprzktorze jako LayerMask)
        jeśli blockedByObstacle = true to cała funkcja IsTargetVisible zwraca fail
        jeśli blockedByObstacle = false to cała funkcja IsTargetVisible zwraca true
        */ 
        bool blockedByObstacle = Physics.Raycast(
            origin,
            directionToTarget.normalized,
            distanceToTarget,
            obstacleLayerMask,
            QueryTriggerInteraction.Ignore
        );

        return !blockedByObstacle;
    }
#endregion

 

// gizmos'y do debugu 
#region OnDrawGizmos
    private void OnDrawGizmos()
    {

        Transform eyeRef = (eyes != null) ? eyes : transform;
        Vector3 forward = eyeRef.forward * viewRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * forward;
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyeRef.position, eyeRef.position + rightBoundary);
        Gizmos.DrawLine(eyeRef.position, eyeRef.position + leftBoundary);
        Gizmos.DrawLine(eyeRef.position + rightBoundary, eyeRef.position + leftBoundary);

    }
#endregion


}
