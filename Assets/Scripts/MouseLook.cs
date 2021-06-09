using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;
    string xAxis, yAxis;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (gameObject.name.Equals("Player1 camera")) 
        {
            xAxis = "Mouse X";
            yAxis = "Mouse Y";
        }
        else
        {
            xAxis = "Mouse X2"; yAxis = "Mouse Y2";
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis(xAxis) *mouseSensitivity *Time.deltaTime;
        float mouseY = Input.GetAxis(yAxis) * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
