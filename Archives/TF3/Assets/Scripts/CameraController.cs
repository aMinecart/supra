using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class CameraControl : MonoBehaviour
{
    // private Camera main_camera;
    private PlayerControl player_control;
    private float rot_y;

    void Awake()
    {
        player_control = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    // Start is called before the first frame update
    /*
    void Start()
    {
        main_camera = GetComponent<Camera>();
    }
    */

    private void OnLook(InputValue look_value)
    {
        rot_y += look_value.Get<Vector2>().y * player_control.sens;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // transform.localRotation = Quaternion.Euler(-rot_y, 0.0f, 0.0f);
    }
}