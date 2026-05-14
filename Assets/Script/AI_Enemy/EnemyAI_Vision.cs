using UnityEngine;

public class EnemyAI_Vision : MonoBehaviour
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
    


    private int maxColliderSize = 10;
    private Collider[] _hitColliders;
    private float scanTimer;


    public bool CanSeePlayer { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public Vector3 LastKnownPlayerPosition { get; private set; }


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
    private void Update()
    {
        scanTimer -= Time.deltaTime;

        if (scanTimer > 0f)
            return;

        scanTimer = scanInterval;
        SphereScanForPlayer();
    }

    #region Sphere Scan 
    private void SphereScanForPlayer()
    {
        Physics.SyncTransforms();
        
        int hits = Physics.OverlapSphereNonAlloc(
            transform.position,
            viewRadius ,
            _hitColliders,
            playerLayerMask,
            QueryTriggerInteraction.Ignore
            );
        
        for(int i = 0; i < hits; i++)
        {
            Transform candidate = _hitColliders[i].transform;

            if (IsTargetVisible(candidate))
            {
                CanSeePlayer = true;
                PlayerTransform = candidate;
                LastKnownPlayerPosition = candidate.position;
                return;
            }

        }

        CanSeePlayer = false;
        PlayerTransform = null;
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

        float angleToTarget = Vector3.Angle(eyes.forward, directionToTarget.normalized);

        if (angleToTarget > viewAngle / 2f)
            return false;

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

 


#region OnDrawGizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,viewRadius);


        Vector3 forward = transform.forward * viewRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * forward;
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position + rightBoundary, transform.position + leftBoundary);

    }
#endregion


}
