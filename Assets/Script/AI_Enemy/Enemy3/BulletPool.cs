using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 5;

    private Queue<GameObject> pool = new Queue<GameObject>();



    private void Awake()
    {   
        
        for(int i = 0;i < poolSize; i++)
        {
            
            GameObject bullet = Instantiate(bulletPrefab,transform);
            bullet.SetActive(false);
            pool.Enqueue(bullet);

        }

    }

    public GameObject Get()
    {
        GameObject bullet;

        if(pool.Count > 0)
        {
            bullet = pool.Dequeue();
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(bulletPrefab,transform);
        }
        bullet.transform.SetParent(null);
        return bullet;

    }


    public void Return(GameObject bullet)
    {   
        if(bullet == null) return;
        bullet.SetActive(false);
        bullet.transform.SetParent(transform);
        pool.Enqueue(bullet);
    }


}

