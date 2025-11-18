using UnityEngine;

public class MouseInteract : MonoBehaviour
{
    Camera cam;
    Transform grabbed;
    public float dist;
    float tilt;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                grabbed = hit.transform;
                dist = hit.distance;
                tilt = 0;
                
                if (grabbed.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.useGravity = false;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (grabbed.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.useGravity = true;
            }
            grabbed = null;
        }

        if (grabbed)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            var targetPos = ray.GetPoint(dist);
            grabbed.position = Vector3.Lerp(grabbed.position, targetPos, 20 * Time.deltaTime);

            if (Input.GetKey(KeyCode.Q)) tilt = 200 * Time.deltaTime;
            else if (Input.GetKey(KeyCode.E)) tilt = -200 * Time.deltaTime;
            else tilt = 0;
            grabbed.rotation *= Quaternion.AngleAxis(tilt, Vector3.forward);
        }
    }
}