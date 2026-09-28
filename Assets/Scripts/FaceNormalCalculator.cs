using UnityEngine;

/// <summary>
/// Computes the unit normal vector of a triangular face defined by 3 points (V1, V2, V3),
/// following the same method as the example:
///
///   S = V3->V2 = V2 - V3
///   r = V3->V1 = V1 - V3
///   n = S x r
///   n_hat = n / |n|
///
/// Runs automatically at the start of the game (Awake/Start), no manual trigger needed.
/// Attach this script to any GameObject in the scene.
/// </summary>
public class FaceNormalCalculator : MonoBehaviour
{
    [Header("Face Vertices (edit in Inspector)")]
    public Vector3 V1 = new Vector3(0f, 2f, 2f);
    public Vector3 V2 = new Vector3(0f, 1f, 4f);
    public Vector3 V3 = new Vector3(1f, 1f, 2f);

    [Header("Results (read-only, filled at runtime)")]
    public Vector3 S;          // Vector from V3 to V2
    public Vector3 r;          // Vector from V3 to V1
    public Vector3 normal;     // Raw normal (S x r)
    public float normLength;   // |n|
    public Vector3 unitNormal; // n / |n|  -> the vector "t" in the exercise

    [Header("Debug Visualization")]
    public bool drawGizmos = true;
    public float gizmoScale = 1f;

    void Start()
    {
        CalculateNormal();
    }

    /// <summary>
    /// Performs the full calculation and logs each step, mirroring the example.
    /// </summary>
    [ContextMenu("Calculate Normal")]
    public void CalculateNormal()
    {
        // Step 1: edge vectors from V3
        S = V2 - V3; // V3 -> V2
        r = V1 - V3; // V3 -> V1

        // Step 2: cross product (S x r) gives the normal vector
        normal = Vector3.Cross(S, r);

        // Step 3: normalize
        normLength = normal.magnitude;
        unitNormal = normLength > Mathf.Epsilon ? normal / normLength : Vector3.zero;

        // Log every step as its own line, like the worked example
        Debug.Log("===== Face Normal Calculation =====");
        Debug.Log($"V1 = {FormatVec(V1)}");
        Debug.Log($"V2 = {FormatVec(V2)}");
        Debug.Log($"V3 = {FormatVec(V3)}");
        Debug.Log($"S (V3->V2) = V2 - V3 = {FormatVec(S)}");
        Debug.Log($"r (V3->V1) = V1 - V3 = {FormatVec(r)}");
        Debug.Log($"n = S x r = {FormatVec(normal)}");
        Debug.Log($"|n| = {normLength:F4}");
        Debug.Log($"Unit normal t = n / |n| = {FormatVec(unitNormal)}");
        Debug.Log("====================================");
    }

    private string FormatVec(Vector3 v)
    {
        return $"({v.x:F3}, {v.y:F3}, {v.z:F3})";
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        // Draw the triangle
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(V1, V2);
        Gizmos.DrawLine(V2, V3);
        Gizmos.DrawLine(V3, V1);

        // Draw vertices
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(V1, 0.05f);
        Gizmos.DrawSphere(V2, 0.05f);
        Gizmos.DrawSphere(V3, 0.05f);

        // Draw S and r edge vectors from V3
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(V3, V3 + (V2 - V3));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(V3, V3 + (V1 - V3));

        // Draw the unit normal "t" from V3
        if (Application.isPlaying && unitNormal != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(V3, V3 + unitNormal * gizmoScale);
        }
        else
        {
            // Preview in editor without running the game
            Vector3 sEdge = V2 - V3;
            Vector3 rEdge = V1 - V3;
            Vector3 n = Vector3.Cross(sEdge, rEdge);
            if (n != Vector3.zero)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(V3, V3 + n.normalized * gizmoScale);
            }
        }
    }
}