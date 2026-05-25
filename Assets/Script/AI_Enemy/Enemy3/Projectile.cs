using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    private AudioSource audioSource;
    private int damage;
    private BulletPool pool;

    private float timer;

    public void Init(int damageAmoutn, BulletPool bulletPool)
    {
        
        damage = damageAmoutn;
        pool = bulletPool;
        timer = 0;
        audioSource = GetComponentInParent<AudioSource>();
        if (audioSource != null)
            audioSource.Play();
    }

    private void Update()
    {
        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        timer += Time.deltaTime;
        if(timer >= lifeTime)
        {
            ReturnToPool();
        }

    }



    private void OnTriggerEnter(Collider other)
    {
        
        IDamageable target = other.GetComponent<IDamageable>();
        if(target != null)
        {
            target.TakeDamage(damage);
            ReturnToPool();
            return;
        }
        if (!other.isTrigger)
        {
            ReturnToPool();
        }
    }


    private void ReturnToPool()
    {
        pool.Return(gameObject);
    }



}
