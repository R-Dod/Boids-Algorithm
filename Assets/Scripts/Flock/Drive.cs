using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    float angSpeed = 1f, width, height, angle = 0f;

    private void Start()
    {
        width = 15;
        height = 7;
    }

    // Update is called once per frame
    void Update()
    {

        angle += Time.deltaTime * angSpeed;

        float x = Mathf.Cos(angle) * width;
        float y = Mathf.Sin(angle) * height;


        // point object towards movement direction
        Vector2 direction = new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle));
        transform.up = direction;
        transform.position = new Vector3(x, y, transform.position.z);

    }
}
