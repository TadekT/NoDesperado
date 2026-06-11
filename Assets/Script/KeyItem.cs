using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotateSpeed = 90f;

    [Header("Hover")]
    [SerializeField] private float hoverAmplitude = 0.15f;
    [SerializeField] private float hoverFrequency = 1f;

    private float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

        float y = startY + Mathf.Sin(Time.time * hoverFrequency * Mathf.PI * 2f) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.CollectKey();
        Destroy(gameObject);
    }
}
