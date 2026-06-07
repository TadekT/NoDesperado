using UnityEngine;

public class EnemyAI3_Combat : MonoBehaviour
{

    [SerializeField] private OnTriggerEnterCombatDetection combatTrigger;
    [SerializeField] private int damage = 10;

    [SerializeField] private AudioSource shootAudio;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
    private BulletPool bulletPool;

    [Header("Magazine")]
    [SerializeField] private int magazineSize = 5;
    [SerializeField] private float reloadTime = 2f;

    private int currentAmmo;
    private bool isReloading = false;
    private float reloadTimer = 0f;

    private IDamageable currentTarget;
    public bool IsPlayerInAttackRange => currentTarget!= null;
    public bool IsReloading => isReloading;

    private void Awake()
    {
        currentAmmo = magazineSize;
        bulletPool = GetComponentInChildren<BulletPool>();

        if(bulletPool == null)
        {
            Debug.LogWarning("[EnemyAI3_Combat] Brak BulletPool ", this);
        }

        if(combatTrigger == null)
        {
            combatTrigger = GetComponentInChildren<OnTriggerEnterCombatDetection>();
        }
        if(shootAudio == null)
        {
            shootAudio = GetComponentInChildren<AudioSource>();
        }
    }

    private void Update()
    {
        // Przeładowanie obsługujemy w Update zamiast przez Coroutine —
        // prościej, łatwiej debugować, można przerwać jeśli wróg zginie
        if (!isReloading) return;
 
        reloadTimer += Time.deltaTime;
 
        if (reloadTimer >= reloadTime)
        {
            // Przeładowanie zakończone — uzupełniamy magazynek
            currentAmmo = magazineSize;
            isReloading = false;
            reloadTimer = 0f;
        }
    }


    private void OnEnable()
    {
        if(combatTrigger != null)
        {
            combatTrigger.InAttackRange += HandleCombatSignal;
        }

    }

    private void OnDisable()
    {
        if(combatTrigger != null)
        {
            combatTrigger.InAttackRange -= HandleCombatSignal;
        }
        currentTarget= null;
    }

    private void HandleCombatSignal(IDamageable target)
    {
        currentTarget= target;

    }

    public void Attack()
    {
        if(currentTarget== null ) return;
        if(isReloading) return;

        Shoot();
    }

    private void Shoot()
    {
        if (bulletSpawn == null) return;
 
        // Pobieramy pocisk z puli zamiast Instantiate —
        // obiekt już istnieje w pamięci, tylko go "budzimy"
        GameObject bullet = bulletPool != null
            ? bulletPool.Get()
            : Instantiate(bulletPrefab, transform); // fallback bez puli
 
        // Ustawiamy pozycję i rotację według bulletSpawn —
        // rotacja determinuje kierunek lotu (Projectile używa Vector3.forward)
        bullet.transform.SetPositionAndRotation(bulletSpawn.position, bulletSpawn.rotation);
 
        // Init() zamiast konstruktora — ustawiamy świeże wartości dla "obudzonego" pocisku
        Projectile projectile = bullet.GetComponent<Projectile>();
        if (projectile != null)
            projectile.Init(damage, bulletPool);
        
        PlayShootSound();

        currentAmmo--;
 
        // Sprawdzamy po strzale czy magazynek jest pusty
        if (currentAmmo <= 0)
        {
            isReloading = true;
            reloadTimer = 0f;
        }
    }

    private void PlayShootSound()
    {
        if(shootAudio != null)
        {
            shootAudio.PlayOneShot(shootAudio.clip);
        }
    }



}
