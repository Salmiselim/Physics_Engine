using UnityEngine;

public class AvionOrientationExact : MonoBehaviour
{
    [Header("Points donnés dans l'exercice")]
    public Vector3 centreGravite = new Vector3(20f, -10f, 35f);
    public Vector3 pointAvant = new Vector3(23f, -14f, 40f);

    void Start()
    {
        ReproduireCalculSlide47();
    }

    void ReproduireCalculSlide47()
    {
        // Étape 1 : vecteur direction ramené à l'origine -> (3, -4, 5)
        float dx = pointAvant.x - centreGravite.x; // 3
        float dy = pointAvant.y - centreGravite.y; // -4
        float dz = pointAvant.z - centreGravite.z; // 5

        Debug.Log($"Direction (d) = ({dx}, {dy}, {dz})");

        // Étape 2 : calcul de Psi (slide 45)
        // sin(Psi) = 4/sqrt(50), cos(Psi) = sqrt(34)/sqrt(50)
        float racine50 = Mathf.Sqrt(50f);
        float racine34 = Mathf.Sqrt(34f);
        float sinPsi = 4f / racine50;
        float cosPsi = racine34 / racine50;
        float psi = Mathf.Atan2(sinPsi, cosPsi);

        // Étape 3 : calcul de Phi (slide 46)
        // sin(Phi) = 3/sqrt(34), cos(Phi) = 5/sqrt(34)
        float sinPhi = 3f / racine34;
        float cosPhi = 5f / racine34;
        float phi = Mathf.Atan2(sinPhi, cosPhi);

        Debug.Log($"Psi = {psi * Mathf.Rad2Deg}°, Phi = {phi * Mathf.Rad2Deg}°");

        // Étape 4 : construction de Rx(Psi) -- slide 40/45
        Matrix4x4 Rx = Matrix4x4.identity;
        Rx.m00 = 1; Rx.m01 = 0;      Rx.m02 = 0;
        Rx.m10 = 0; Rx.m11 = cosPsi; Rx.m12 = -sinPsi;
        Rx.m20 = 0; Rx.m21 = sinPsi; Rx.m22 = cosPsi;

        // Étape 5 : construction de Ry(Phi) -- slide 40/46
        Matrix4x4 Ry = Matrix4x4.identity;
        Ry.m00 = cosPhi;  Ry.m01 = 0; Ry.m02 = sinPhi;
        Ry.m10 = 0;       Ry.m11 = 1; Ry.m12 = 0;
        Ry.m20 = -sinPhi; Ry.m21 = 0; Ry.m22 = cosPhi;

        // Étape 6 : R = Ry(Phi) * Rx(Psi)  -- slide 44 et 47
        Matrix4x4 R = Ry * Rx;

        Debug.Log("R (doit correspondre à ) :\n" +
            $"{R.m00:F4} {R.m01:F4} {R.m02:F4}\n" +
            $"{R.m10:F4} {R.m11:F4} {R.m12:F4}\n" +
            $"{R.m20:F4} {R.m21:F4} {R.m22:F4}");

        // Étape 7 : Translation T_(20,-10,35) -- 
        Matrix4x4 T = Matrix4x4.Translate(centreGravite);

        // Étape 8 : Transformation finale = T * R
        Matrix4x4 M = T * R;

        Debug.Log("Transformation finale (doit correspondre à ) :\n" +
            $"{M.m00:F4} {M.m01:F4} {M.m02:F4} {M.m03:F4}\n" +
            $"{M.m10:F4} {M.m11:F4} {M.m12:F4} {M.m13:F4}\n" +
            $"{M.m20:F4} {M.m21:F4} {M.m22:F4} {M.m23:F4}\n" +
            $"{M.m30:F4} {M.m31:F4} {M.m32:F4} {M.m33:F4}");

        // Application au Transform Unity
        transform.position = M.MultiplyPoint3x4(Vector3.zero);
        transform.rotation = R.rotation;

        Debug.Log("Position finale : " + transform.position);
        Debug.Log("Rotation finale (Euler) : " + transform.rotation.eulerAngles);
    }
}