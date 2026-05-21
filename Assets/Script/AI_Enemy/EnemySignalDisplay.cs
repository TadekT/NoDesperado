using UnityEngine;

public class EnemySignalDisplay : MonoBehaviour
{
    [SerializeField] private EnemyAI_Brain brain;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private AudioSource audioSource;

    [Header("Który stan pokazuje ten sygnał?")]
    [SerializeField] private EnemyAI_Brain.EnemyState triggerState;

    private bool hasPlayedSound = false;

    private void Awake()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        if (brain == null)
            brain = GetComponentInParent<EnemyAI_Brain>();

        meshRenderer.enabled = false;
    }

    private void OnEnable()
    {
        if (brain == null)
            brain = GetComponentInParent<EnemyAI_Brain>();

        if (brain != null)
            brain.OnChangeState += HandleStateChange;
    }

    private void OnDisable()
    {
        if (brain != null)
            brain.OnChangeState -= HandleStateChange;
    }

    private void HandleStateChange(EnemyAI_Brain.EnemyState previous, EnemyAI_Brain.EnemyState current)
    {
        if (current == triggerState)
        {
            meshRenderer.enabled = true;
            if (!hasPlayedSound && audioSource != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
                hasPlayedSound = true;
            }
        }
        else if (previous == triggerState)
        {
            meshRenderer.enabled = false;
            hasPlayedSound = false;
        }
    }
}