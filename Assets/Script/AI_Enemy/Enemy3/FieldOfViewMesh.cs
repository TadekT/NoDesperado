using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FieldOfViewMesh : MonoBehaviour
{
    [Header("Field of View (copy values from EnemyAI3_Vision)")]
    [SerializeField] private float viewRadius = 15f;
    [Range(0, 360)]
    [SerializeField] private float viewAngle = 60f;

    [Header("Mesh Quality")]
    [SerializeField] private int rayCount = 50;

    [Header("Vertical Position")]
    [SerializeField] private float heightOffset = 0.05f;

    [Header("Occlusion")]
    [SerializeField] private EnemyAI3_Vision vision;

    [Header("Alert Colors (adjust in Inspector)")]
    // calm: Idle / Patrol / None
    [SerializeField] private Color calmColor = new Color(0f, 0.68f, 0.02f, 0.44f);
    // suspicious: Suspicious / Search
    [SerializeField] private Color suspiciousColor = new Color(1f, 0.55f, 0f, 0.5f);
    // alert: Chase / Attack
    [SerializeField] private Color alertColor = new Color(0.9f, 0.1f, 0.1f, 0.55f);

    [Header("Color Transition Smoothness")]
    // higher value = faster transition between colors
    [SerializeField] private float colorLerpSpeed = 4f;

    [Header("References")]
    [SerializeField] private EnemyAI3_Brain brain;

    private Mesh mesh;
    private LayerMask obstacleMask;
    private MeshRenderer meshRenderer;
    private Material materialInstance;

    private Color targetColor;
    private Color currentColor;

    // color property ID — chosen once, depending on the shader
    private int colorPropId;

    private void Awake()
    {
        if (vision == null) vision = GetComponentInParent<EnemyAI3_Vision>();
        if (brain == null) brain = GetComponentInParent<EnemyAI3_Brain>();
    }

    private void OnEnable()
    {
        if (brain != null) brain.OnChangeState3 += HandleStateChange;
    }

    private void OnDisable()
    {
        if (brain != null) brain.OnChangeState3 -= HandleStateChange;
    }

    private void Start()
    {
        mesh = new Mesh();
        mesh.name = "FieldOfViewMesh";
        GetComponent<MeshFilter>().mesh = mesh;

        if (vision != null) obstacleMask = vision.ObstacleLayerMask;

        meshRenderer = GetComponent<MeshRenderer>();
        // material instance (per-enemy copy) — we don't modify the shared asset
        materialInstance = meshRenderer.material;

        // URP/Unlit uses _BaseColor; older shaders use _Color — pick whichever is available
        colorPropId = materialInstance.HasProperty("_BaseColor")
            ? Shader.PropertyToID("_BaseColor")
            : Shader.PropertyToID("_Color");

        // start from calm color
        currentColor = calmColor;
        targetColor = calmColor;
        materialInstance.SetColor(colorPropId, currentColor);

        meshRenderer.enabled = false;
    }

    public void TogglePreview()
    {
        meshRenderer.enabled = !meshRenderer.enabled;
    }

    public void SetPreviewVisible(bool visible)
    {
        meshRenderer.enabled = visible;
    }

    private void LateUpdate()
    {
        if (!meshRenderer.enabled) return;
        DrawFieldOfView();
        UpdateColor();
    }

    // === COLOR ===

    private void HandleStateChange(EnemyAI3_Brain.EnemyState previous, EnemyAI3_Brain.EnemyState next)
    {
        targetColor = ColorForState(next);
    }

    private Color ColorForState(EnemyAI3_Brain.EnemyState state)
    {
        switch (state)
        {
            case EnemyAI3_Brain.EnemyState.Suspicious:
            case EnemyAI3_Brain.EnemyState.Search:
                return suspiciousColor;

            case EnemyAI3_Brain.EnemyState.Chase:
            case EnemyAI3_Brain.EnemyState.Attack:
                return alertColor;

            // Idle, Patrol, None
            default:
                return calmColor;
        }
    }

    private void UpdateColor()
    {
        if (materialInstance == null) return;

        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * colorLerpSpeed);
        materialInstance.SetColor(colorPropId, currentColor);
    }

    // === MESH + OCCLUSION ===

    private void DrawFieldOfView()
    {
        int vertexCount = rayCount + 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[rayCount * 3];

        Vector3 origin = transform.position;

        vertices[0] = Vector3.up * heightOffset;

        float angleStep = viewAngle / rayCount;
        float currentAngle = -viewAngle / 2f;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 localDir = DirFromAngle(currentAngle);
            Vector3 worldDir = transform.TransformDirection(localDir);

            Vector3 worldVertex;
            if (Physics.Raycast(origin, worldDir, out RaycastHit hit, viewRadius, obstacleMask, QueryTriggerInteraction.Ignore))
            {
                worldVertex = hit.point;
            }
            else
            {
                worldVertex = origin + worldDir * viewRadius;
            }

            Vector3 localVertex = transform.InverseTransformPoint(worldVertex);
            localVertex.y = heightOffset;
            vertices[i + 1] = localVertex;

            currentAngle += angleStep;
        }

        for (int i = 0; i < rayCount; i++)
        {
            int t = i * 3;
            triangles[t]     = 0;
            triangles[t + 1] = i + 1;
            triangles[t + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private Vector3 DirFromAngle(float angleInDegrees)
    {
        float rad = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad));
    }

    private void OnDestroy()
    {
        // clean up material instance to avoid memory leak
        if (materialInstance != null) Destroy(materialInstance);
    }
}