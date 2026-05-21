using System;
using UnityEngine;

public class OnTriggerEnterCombatDetection : MonoBehaviour
{

    public event Action<IDamageable> InAttackRange;

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        IDamageable damageable = other.GetComponent<IDamageable>();
        if(damageable != null )
        {
            InAttackRange?.Invoke(damageable);
        }
        
    }

    void OnTriggerExit(Collider other)
    {
            if (!other.CompareTag("Player")) return;
            InAttackRange?.Invoke(null);
    }

}
