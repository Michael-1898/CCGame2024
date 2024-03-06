﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikeLook : MonoBehaviour
{
    public Transform playerCamera;
    public Vector2 sensitivities;

    private Vector2 XYRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseInput = new Vector2 {
            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        XYRotation.x -= mouseInput.y * sensitivities.y;
        XYRotation.y += mouseInput.x * sensitivities.x;

        XYRotation.x = Mathf.Clamp(XYRotation.x, -90f, 90f);

        print("x: " + XYRotation.x);
        print("y: " + XYRotation.y);

        playerCamera.localEulerAngles = new Vector3(XYRotation.x, 0f, 0f);
    }

    void LateUpdate()
    {
        transform.eulerAngles = new Vector3(0f, XYRotation.y, 0f);
    }
}
