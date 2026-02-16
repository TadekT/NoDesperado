using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(Rigidbody))]


public class Enemy1Controller : MonoBehaviour
{

    private NavMeshAgent _agent;
    [SerializeField] private LayerMask playerLayer;



    [SerializeField] private float debugSpehereRadius;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        if(_agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);

        }
        StartCoroutine(PlayerInRangeCoroutine());

    
    
    
    }

    private IEnumerator PlayerInRangeCoroutine()
    {
        int findCouter = 0;
        Debug.Log("Starting PlayerInRangeCoroutine");
        while (true)
        {
            Collider[] playerCollider = Physics.OverlapSphere(transform.position, debugSpehereRadius, playerLayer);
            if(playerCollider.Length > 0)
            {
                Debug.Log("Player detected in range");
                yield return new WaitForSecondsRealtime(.5f);
                findCouter++;
                if(findCouter >= 5)
                {
                    Debug.Log("Stoppingcorutine and destrouing;");
                }
            }
        
        Debug.Log("Player not detected in range");
        
        yield return new WaitForSecondsRealtime(2f);
        
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugSpehereRadius);
    }
#endif


}
