using UnityEngine;

public class CubeRoller : MonoBehaviour
{
    public float cubeSize = 1f;
    public float actionDuration = 0.35f;   // seconds per roll / spin

    const float QUARTER_TURN = 1.5707964f; // 90° in radians

    // Current state, stored in OUR quaternion
    Vector3 currentPos;
    MyQuaternion currentRot = MyQuaternion.Identity;

    // Animation state
    bool busy;
    float timer;
    Vector3 axis, pivot, startPos;
    MyQuaternion startRot;

    void Start()
    {
        currentPos = new Vector3(transform.position.x, cubeSize * 0.5f, transform.position.z);
        currentRot = MyQuaternion.Identity;
        Apply(currentPos, currentRot);
        Debug.Log("CubeRoller is running");
    }

    void Update()
    {
        if (busy) { Animate(); return; }

        if (Input.GetKeyDown(KeyCode.RightArrow)) StartRoll(1, 0);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) StartRoll(-1, 0);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) StartRoll(0, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) StartRoll(0, -1);
        else if (Input.GetKeyDown(KeyCode.Space)) StartSpin();
    }

    void StartRoll(int dx, int dz)
    {
        float h = cubeSize * 0.5f;
        axis = new Vector3(dz, 0f, -dx);                          // up x direction
        pivot = currentPos + new Vector3(dx * h, -h, dz * h);     // bottom edge
        Begin();
    }

    void StartSpin()
    {
        axis = new Vector3(0f, 1f, 0f);   // vertical axis
        pivot = currentPos;               // rotate around own center
        Begin();
    }

    void Begin()
    {
        startPos = currentPos;
        startRot = currentRot;
        timer = 0f;
        busy = true;
    }

    void Animate()
    {
        timer += Time.deltaTime;
        float t = timer / actionDuration;
        if (t > 1f) t = 1f;

        // Rotation "step" from 0 to 90° about the world axis
        MyQuaternion step = MyQuaternion.FromAxisAngle(axis, QUARTER_TURN * t);

        MyQuaternion rot = (step * startRot).Normalized();
        Vector3 pos = pivot + step.Rotate(startPos - pivot);

        Apply(pos, rot);

        if (t >= 1f)
        {
            currentPos = pos;
            currentRot = rot;
            busy = false;
        }
    }

    // Only Unity-specific call: copy our result into the Transform.
    // Unity's order is (x, y, z, w), ours is (a, b, c, d) with a = real part.
    void Apply(Vector3 pos, MyQuaternion q)
    {
        transform.position = pos;
        transform.rotation = new Quaternion(q.b, q.c, q.d, q.a);
    }
}