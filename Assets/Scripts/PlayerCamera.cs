using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public float rotationSpeed = 2;
    public Transform player;
    public Transform target;
    public Transform backfacing;

    private float mouseX, mouseY;

    private bool blocking;
    private float blockTime;
    private bool routineStarted;
    private Vector3 backStartPos;
    private Vector3 _offset;


    
    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
            Debug.LogWarning("No target for camera");

        _offset = this.transform.position - target.transform.position;


        //  Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
        backStartPos = backfacing.position;

        //Prevent divide by zero error (TMP)
        blockTime = 1;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        CamControl();
    }
    private float readInputX()
    {
        float x=0;
        x = Input.GetAxis("Mouse X");

        float a3=(Input.GetAxis("Axis 3"));
        if (Mathf.Abs(a3) > 1e-2)
            x += a3 *3;

        return x;
    }
    private float readInputY()
    {
        float y = 0;
        y = Input.GetAxis("Mouse Y");

        float a6 = (Input.GetAxis("Axis 6"));
        if (Mathf.Abs(a6) > 1e-2)
            y += a6;


        return y;
    }
    private bool readBlockDown()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton4) || (Input.GetKeyDown(KeyCode.LeftShift)))
        {
            return true;
        }
        return false;
    }
    private bool readBlockUp()
    {
        if (Input.GetKeyUp(KeyCode.JoystickButton4) || (Input.GetKeyUp(KeyCode.LeftShift)))
        {
            return true;
        }
        return false;
    }
    void CamControl()
    {
        if (!blocking)
        {
            float hori = Input.GetAxis("Horizontal");
            mouseX += readInputX() * rotationSpeed;
            mouseY += readInputY() * rotationSpeed;
            mouseY = Mathf.Clamp(mouseY, -15, 30);

            //allows camera to tilt and chase player like DW
            if (Mathf.Abs(hori) > 0.01f)
                mouseX += hori;

            //BLOCK
            if (readBlockDown())
            {
                //push camera forward to create zoom in effect 
                backfacing.position = backfacing.position + backfacing.forward *1.15f;

                //reset camera directly behind character
                computeBehindPlayer();
                blocking = true;
                blockTime = Time.time;
                mouseX = 0;
                mouseY = 0;
            }
            else
                target.rotation = Quaternion.Euler(mouseY, mouseX, 0);


            this.transform.LookAt(target);
        }
        else if (readBlockUp())
        {
            blocking = false;
            //Reset depth of camera to zoom out
            StartCoroutine(LerpBack());

        }
       else
           computeBehindPlayer();


        //player.rotation = Quaternion.Euler(0, mouseX, 0);

    }

    private void computeBehindPlayer()
    {
       float dis = (backfacing.forward - target.forward).sqrMagnitude;

        if (dis > 1)
            dis = dis * 0.55f;

        //The more time passes, the faster we get there, far distance also speeds it up;
        float weight = ((Time.time / blockTime) + (dis)) /2;

         target.position = Vector3.Slerp(target.position, backfacing.position, weight);
         target.rotation = Quaternion.Slerp(target.rotation, backfacing.rotation, weight);
    }

    IEnumerator LerpBack()
    {
        if (!routineStarted)
        {
            routineStarted = true;
            bool reached = false;
            float startTime = Time.time;
            Vector3 positionToReach = backfacing.position - backfacing.forward * 1.15f;
            while (!reached)
            {
                backfacing.position = Vector3.Slerp(backfacing.position, backStartPos, (Time.time / startTime) * 0.45f);
                yield return new WaitForSeconds(Time.deltaTime);
                target.position = backfacing.position;
                if (backfacing.position == backStartPos)
                    reached = true;
            }
            routineStarted = false;
        }
    }

}
