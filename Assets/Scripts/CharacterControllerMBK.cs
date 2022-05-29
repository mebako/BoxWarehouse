using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterControllerMBK : MonoBehaviour
{
    [Header("General")]
    public bool IsTouchControls = false;
    public Camera CAM_Main;
    public Transform PlayerBody;
    public CharacterController Controller;
    public float MovementSpeed = 10;
    Vector3 velocity;

    [Header("Mouse Look")]
    public float mouse_sensivity = 100f;

    [Header("Touch Controls")]
    public float TouchCameraSensivity;
    //public float TouchMoveDeadZone;
    int leftID, rightID;
    Vector2 lookInput;
    float cameraPitch;
    Vector2 moveStartPos;
    Vector2 moveInput;

    float xRot = 0f;
    private void Start()
    {
        if (!IsTouchControls)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            leftID = -1;
            rightID = -1;

            //TouchMoveDeadZone = Mathf.Pow(Screen.height / TouchMoveDeadZone, 2);
        }
    }
    private void Update()
    {
        if (IsTouchControls)
        {
            GetTouchInput();
            if (leftID != -1 && !EventSystem.current.IsPointerOverGameObject(leftID)) TouchMove();
            if (rightID != -1 && !EventSystem.current.IsPointerOverGameObject(rightID)) TouchLook();
        }
        else
        {
            //Look
            float mouseX = Input.GetAxis("Mouse X") * mouse_sensivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouse_sensivity * Time.deltaTime;
            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -85, 85);

            CAM_Main.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
            PlayerBody.Rotate(Vector3.up * mouseX);

            //Move
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 movement = transform.right * moveX + transform.forward * moveZ;
            Controller.Move(movement * MovementSpeed * Time.deltaTime);
        }
    }
    void TouchLook()
    {
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -85f, 85f);
        var rot = CAM_Main.transform.localRotation = Quaternion.Euler(cameraPitch,0,0);
        Quaternion.Lerp(CAM_Main.transform.localRotation, rot, Time.deltaTime);

        PlayerBody.Rotate(transform.up, lookInput.x);
    }
    void TouchMove()
    {
        //if (moveInput.sqrMagnitude <= TouchMoveDeadZone) return;
        Vector2 moveDir = moveInput.normalized * MovementSpeed * Time.deltaTime;
        Controller.Move(transform.right * moveDir.x + transform.forward * moveDir.y);
    }
    void GetTouchInput()
    {
        int i;
        for (i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            switch (t.phase)
            {
                case TouchPhase.Began:
                    if (t.position.x < Screen.width / 2 && leftID == -1)
                    {
                        leftID = t.fingerId;
                        moveStartPos = t.position;
                    }
                    else if (t.position.x > Screen.width / 2 && rightID == -1)
                    {
                        rightID = t.fingerId;
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (t.fingerId == leftID)
                    {
                        leftID = -1;
                    }
                    else if (t.fingerId == rightID)
                    {
                        rightID = -1;
                    }
                    break;
                case TouchPhase.Moved:
                    if (t.fingerId == rightID) lookInput = t.deltaPosition * TouchCameraSensivity * Time.deltaTime;
                    if (t.fingerId == leftID) moveInput = t.position - moveStartPos;
                    break;
                case TouchPhase.Stationary:
                    if (t.fingerId == rightID) lookInput = Vector2.zero;
                    break;
            }
        }
    }
}
