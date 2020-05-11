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

    
    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
            Debug.LogWarning("No target for camera");

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
            mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            mouseY += Input.GetAxis("Mouse Y") * rotationSpeed;
            mouseY = Mathf.Clamp(mouseY, -15, 30);

            //BLOCK
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //reset camera directly behind character
                target.transform.rotation = Quaternion.Euler(0, 0, 0);
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

    void CamControl2()
    {
        if (!blocking)
        {
          // this.transform.LookAt(target);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            blocking = false;


        //player.rotation = Quaternion.Euler(0, mouseX, 0);

    }
}
