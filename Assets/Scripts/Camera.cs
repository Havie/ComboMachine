using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public float rotationSpeed = 2;
    public Transform player;
    public Transform target;
    float mouseX, mouseY;

    private bool blocking;

    private Vector3 _offset;

    
    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
            Debug.LogWarning("No target for camera");

        _offset = this.transform.position - target.transform.position;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        CamControl();
    }

    void CamControl()
    {
        if (!blocking)
        {
            float hori = Input.GetAxis("Horizontal");
            mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            mouseY += Input.GetAxis("Mouse Y") * rotationSpeed;
            mouseY = Mathf.Clamp(mouseY, -15, 30);


            if (Mathf.Abs(hori) > 0.01f)
                mouseX += hori;

            print("MouseX: " + mouseX);

            //BLOCK
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //reset camera directly behind character
                computeBehindPlayer();
                blocking = true;
                mouseX = 0;
                mouseY = 0;
            }
           else
              target.rotation = Quaternion.Euler(mouseY, mouseX, 0);


            this.transform.LookAt(target);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
                blocking = false;


        //player.rotation = Quaternion.Euler(0, mouseX, 0);

    }

    private void computeBehindPlayer()
    {
        target.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

}
