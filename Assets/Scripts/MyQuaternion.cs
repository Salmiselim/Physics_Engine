using UnityEngine;

// Our own quaternion: q = a + bi + cj + dk  (a = real part, slide 53)
public struct MyQuaternion
{
    public float a, b, c, d;

    public MyQuaternion(float a, float b, float c, float d)
    {
        this.a = a; this.b = b; this.c = c; this.d = d;
    }

    public static MyQuaternion Identity
    {
        get { return new MyQuaternion(1f, 0f, 0f, 0f); }
    }

    // Slide 55: q = (cos(θ/2), v·sin(θ/2)), with v a unit vector
    public static MyQuaternion FromAxisAngle(Vector3 axis, float angleRad)
    {
        float len = Mathf.Sqrt(axis.x * axis.x + axis.y * axis.y + axis.z * axis.z);
        float x = axis.x / len, y = axis.y / len, z = axis.z / len;
        float half = angleRad * 0.5f;
        float s = Mathf.Sin(half);
        return new MyQuaternion(Mathf.Cos(half), x * s, y * s, z * s);
    }

    // Slide 53: Hamilton product (same as the 4x4 matrix on the slide)
    public static MyQuaternion operator *(MyQuaternion p, MyQuaternion q)
    {
        return new MyQuaternion(
            p.a * q.a - p.b * q.b - p.c * q.c - p.d * q.d,
            p.a * q.b + p.b * q.a + p.c * q.d - p.d * q.c,
            p.a * q.c - p.b * q.d + p.c * q.a + p.d * q.b,
            p.a * q.d + p.b * q.c - p.c * q.b + p.d * q.a);
    }

    // Slide 57: for a unit quaternion, inverse = conjugate
    public MyQuaternion Conjugate()
    {
        return new MyQuaternion(a, -b, -c, -d);
    }

    // Slide 53: norm
    public float Norm()
    {
        return Mathf.Sqrt(a * a + b * b + c * c + d * d);
    }

    // Keeps the quaternion unitary (slide 54), avoids numerical drift
    public MyQuaternion Normalized()
    {
        float n = Norm();
        return new MyQuaternion(a / n, b / n, c / n, d / n);
    }

    // Slide 58: P' = q · P · q^-1, with P = (0, x, y, z)
    public Vector3 Rotate(Vector3 v)
    {
        MyQuaternion p = new MyQuaternion(0f, v.x, v.y, v.z);
        MyQuaternion r = this * p * this.Conjugate();
        return new Vector3(r.b, r.c, r.d);
    }
}