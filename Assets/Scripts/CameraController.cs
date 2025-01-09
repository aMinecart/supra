using UnityEngine;

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
        transform.localPosition = player_control.transform.position + new Vector3(0.0f, 0.5f, 0.0f);
        transform.localRotation = Quaternion.Euler(-player_control.rot.y, player_control.rot.x, 0.0f);
    }
}