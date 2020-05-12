using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCharacter : MonoBehaviour
{

    public float speed;
    public Transform cameraTarget;
    public Transform mainCamera;

    Vector3 playerMovement2;

    private Vector2 savedDir;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        savedDir = Vector2.zero;
        playerMovement2 = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement4();
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

    private void FaceDirectionPressed2(Vector3 dir)
    {

            savedDir = dir;
            //Left and Right and Inverted, and sometimes become same DIR?
            Quaternion rotationRaw = Quaternion.FromToRotation(dir, mainCamera.transform.forward);
            Vector3 rotationVector = rotationRaw.eulerAngles;
            transform.rotation = Quaternion.Euler(0, rotationVector.y, 0);

    }


    void PlayerMovement3()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(hori, vert);

        if (hori != 0 || vert != 0)
        {
            if (input.x != savedDir.x || input.y != savedDir.y) // pick a new rotation from camera
            {
                Debug.LogError("Different  " + input.x + " , " + input.y);
                savedDir = input;
                Vector3 playerMovement = new Vector3(hori, 0f, vert).normalized * speed * Time.deltaTime;
                playerMovement2 = new Vector3(playerMovement2.x + playerMovement.x, 0, playerMovement2.z + playerMovement.z);

                transform.eulerAngles = new Vector3(playerMovement2.x + playerMovement.x, 0, playerMovement2.z + playerMovement.z);

                TellCanvas(hori, vert);
            }
            else // keep moving at last rotation
            {
                Debug.LogWarning("SAME");
            }

            //Movement
           // transform.Translate(playerMovement, Space.Self);
            //Handled instead by root motion in animation
            _animator.SetBool("isMoving", true);
        }
       else
           _animator.SetBool("isMoving", false);
    }

    void PlayerMovement4()
    {
     
        // Input direction
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector3 inputVector = new Vector3(hori, 0.0f, vert);
        Debug.DrawRay(transform.position, inputVector * 2, Color.red);

        // Camera direction
        Vector3 cameraDirection = cameraTarget.position - mainCamera.position;
        cameraDirection.y = 0.0f; // we dont want up/down dir
        //Vector3 option2= mainCamera.forward; //zero out 
        Debug.DrawRay(transform.position, cameraDirection.normalized * 2.0f, Color.green);

        // Movement angle
        inputVector = inputVector.normalized;
        float angle = Mathf.Atan2(inputVector.x, inputVector.z) / Mathf.PI * 180.0f;
        Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.Euler(0f, angle, 0f));

        // Camera offset by input
        Vector3 movementDirection = rotation.MultiplyVector(cameraDirection.normalized);
        Debug.DrawRay(transform.position, movementDirection.normalized * 2.0f, Color.blue);

        if (Mathf.Abs(hori) > 1e-5 || Mathf.Abs(vert) > 1e-5)
        {
            float facingAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) / Mathf.PI * 180f;
            transform.eulerAngles = new Vector3(0.0f, facingAngle, 0.0f);
            _animator.SetBool("isMoving", true);

            //Angle Camera
            cameraTarget.transform.Rotate(0, hori * 0.2f, 0);

        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
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
