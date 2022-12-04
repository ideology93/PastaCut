using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{

    public Transform bottle;

    public float speed = 1.0f;

    Vector3 prev;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            prev = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - prev;

            var step = new Vector3(
                delta.x * speed * Time.deltaTime,
                0.0f,
                delta.y * speed * Time.deltaTime
            );

            bottle.position += step;
        }
    }
}
