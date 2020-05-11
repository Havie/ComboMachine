using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCharacter : MonoBehaviour
{

    public float speed;
    public Transform cameraTarget;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement2();
    }

    void PlayerMovement()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        if (hori != 0 || vert != 0)
        {
            Vector3 playerMovement = new Vector3(hori, 0f, vert).normalized * speed * Time.deltaTime;
            Quaternion LastKnown = this.transform.rotation;
            Quaternion newRotation = Quaternion.LookRotation(playerMovement);
            print(playerMovement);
            if (newRotation != LastKnown)
            {
                transform.rotation = Quaternion.LookRotation(playerMovement);
                Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerMovement), 0.15F);
            }
           // transform.Translate(playerMovement, Space.Self);
            _animator.SetBool("isMoving", true);
        }
        else
            _animator.SetBool("isMoving", false);
    }

    void PlayerMovement2()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        if (hori != 0 || vert != 0)
        {
            Vector3 playerMovement = new Vector3(hori, 0f, vert).normalized * speed * Time.deltaTime;
            //print(playerMovement);

            FaceDirectionPressed(playerMovement);
            TellCanvas(hori, vert);

            //Movement
            //transform.Translate(playerMovement, Space.Self);
            //Handled instead by root motion in animation
            _animator.SetBool("isMoving", true);
        }
        else
            _animator.SetBool("isMoving", false);
    }

    private void FaceDirectionPressed(Vector3 dir)
    {
        //float no = cameraTarget.transform.position.z - dir.z;
        // float angle= Vector3.Dot(dir, cameraTarget.transform.position);
        // Vector3 RelativePos = Vector3.zero; // ???  I have the angle between the two vectors, how to put that back into a vector?
        //transform.rotation = Quaternion.LookRotation(RelativePos);
        /*
        Quaternion rotationRaw = Quaternion.FromToRotation(dir, cameraTarget.transform.forward);
        Vector3 rotationVector = rotationRaw.eulerAngles;
        Quaternion final = Quaternion.Euler(0, rotationVector.y, 0);
        transform.rotation = final;
        */

        //Left and Right and Inverted, and sometimes become same DIR?
        Quaternion rotationRaw = Quaternion.FromToRotation(dir, cameraTarget.transform.forward);
        Vector3 rotationVector = rotationRaw.eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotationVector.y, 0);


        //GETS ME LEFT
        // float no = cameraTarget.transform.position.z - dir.z;
        //Vector3 RelativePos = new Vector3(dir.x, dir.y, no);

        //ALmost works but just is really the same thing as where we started.. doesnt care ab camera
        //Vector3 RelativePos = Vector3.Scale(dir, cameraTarget.transform.position);


        //transform.rotation = Quaternion.LookRotation(cameraTarget.forward);
    }


    void PlayerMovement3()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        if (hori != 0 || vert != 0)
        {
            Vector3 playerMovement = new Vector3(hori, 0f, vert).normalized * speed * Time.deltaTime;
            //print(playerMovement);

            FaceDirectionPressed(playerMovement);
            TellCanvas(hori, vert);

            //Movement
            //transform.Translate(playerMovement, Space.Self);
            //Handled instead by root motion in animation
            _animator.SetBool("isMoving", true);
        }
        else
            _animator.SetBool("isMoving", false);
    }

    private void TellCanvas(float h, float v)
    {
        string s = "";

        if (h == 0 && v > 0)
            s = "Forward";
        else if (h == 0 && v < 0)
            s = "Backward";
        else if (h >0 && v == 0)
            s = "Right";
        else if (h < 0 && v == 0)
            s = "Left";
        else
            s = "Unknown";

        UIDirectionPressed._instance.UpdateText(s);
    }
}
