using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class CameraControl : MonoBehaviour
{
    private Camera main_camera;
    private PlayerControl player_control;

    void Awake()
    {
        player_control = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    // Start is called before the first frame update
    void Start()
    {
        main_camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(-player_control.rot.y, 0.0f, 0.0f);
    }
}