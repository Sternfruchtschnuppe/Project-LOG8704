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
            }
        }

        if (Input.GetMouseButtonUp(0)) grabbed = null;

        if (grabbed)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            var targetPos = ray.GetPoint(dist);
            grabbed.position = Vector3.Lerp(grabbed.position, targetPos, 20 * Time.deltaTime);

            if (Input.GetKey(KeyCode.Q)) tilt -= 100 * Time.deltaTime;
            if (Input.GetKey(KeyCode.E)) tilt += 100 * Time.deltaTime;

            grabbed.rotation = Quaternion.LookRotation(cam.transform.forward) * Quaternion.AngleAxis(tilt, Vector3.forward);
        }
    }
}