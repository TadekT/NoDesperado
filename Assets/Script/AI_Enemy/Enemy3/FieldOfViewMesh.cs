using UnityEngine;

/// <summary>
/// Krok 1: generuje proceduralny mesh wachlarza wzroku (bez okluzji ścianami).
/// Skrypt jest CZYSTO WIZUALNY — nie wpływa na detekcję w EnemyAI3_Vision.
/// Podpiąć na osobnym obiekcie-dziecku przeciwnika (np. pod "Eyes").
/// Wymaga MeshFilter + MeshRenderer (auto-dodawane przez RequireComponent).
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FieldOfViewMesh : MonoBehaviour
{
    [Header("Field of View (kopiuj wartości z EnemyAI3_Vision)")]
    [SerializeField] private float viewRadius = 10f;
    [Range(0, 360)]
    [SerializeField] private float viewAngle = 60f;

    [Header("Jakość mesha")]
    // ile promieni na cały kąt — więcej = gładsza krawędź łuku, ale drożej
    [SerializeField] private int rayCount = 50;

    [Header("Pozycja pionowa")]
    // delikatne uniesienie nad podłogę, by mesh nie migotał z terenem (z-fighting)
    [SerializeField] private float heightOffset = 0.05f;

    private Mesh mesh;

    private void Start()
    {
        mesh = new Mesh();
        mesh.name = "FieldOfViewMesh";
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    private void DrawFieldOfView()
    {
        int vertexCount = rayCount + 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.up * heightOffset;

        float angleStep = viewAngle / rayCount;
        float currentAngle = -viewAngle / 2f;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 dir = DirFromAngle(currentAngle);
            vertices[i + 1] = dir * viewRadius + Vector3.up * heightOffset;

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
}