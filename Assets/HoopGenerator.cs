using UnityEngine;

public class HoopGenerator : MonoBehaviour
{
    public int segments = 12;
    public float radius = 0.1f;
    public float thickness = 0.02f;
    public float depth = 0.05f;

    void Start()
    {
        for (int i = 0; i < segments; i++)
        {
            float angle = 360f / segments * i;
            Vector3 pos = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;

            GameObject seg = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            seg.transform.parent = transform;
            seg.transform.localPosition = pos;
            seg.transform.LookAt(transform.position);
            seg.transform.Rotate(90, 0, 0);
            seg.transform.localScale = new Vector3(thickness, depth, thickness);
            Destroy(seg.GetComponent<MeshRenderer>()); // Si solo es collider invisible
        }

        // Soporte trasero
        GameObject backSupport = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backSupport.transform.parent = transform;
        backSupport.transform.localPosition = new Vector3(0, 0, -radius - 0.05f);
        backSupport.transform.localScale = new Vector3(thickness, thickness, 0.1f);
        Destroy(backSupport.GetComponent<MeshRenderer>());
    }
}