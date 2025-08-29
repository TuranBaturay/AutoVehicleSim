using UnityEngine;

public class RenderPath : MonoBehaviour
{
    private Vector3[] points;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            points[i] = transform.GetChild(i).position;

        }
        Debug.DrawLine(transform.position, transform.position + transform.forward * 3f, Color.green);

    }
}
