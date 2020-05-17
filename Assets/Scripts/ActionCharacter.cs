using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCharacter : MonoBehaviour
{


    public Transform cameraTarget;
    public Transform myCamera;

    public BoxCollider _wpCollider1;
    public BoxCollider _wpCollider2;

    private bool isAttacking;
    private float speed;
    private bool comboState=true;

    private Animator _animator;
    private PlayerCamera _pc;

    //OLD
    private Vector2 savedDir;
    Vector3 playerMovement2;

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        savedDir = Vector2.zero;
        playerMovement2 = Vector3.zero;
        _pc = myCamera.GetComponent<PlayerCamera>();
        ToggleColliders(false);
        unlockMovement();
    }

    // Update is called once per frame
    void Update()
    {

            PlayerMovement4();
    }
    public void  setSpeed(float amnt) => speed = amnt;
    public float getSpeed() => speed;

    void PlayerMovement4() //Seth Berrier
    {

        if (!isAttacking)
        {
            // Input direction
            float hori = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");
            Vector3 inputVector = new Vector3(hori, 0.0f, vert);
            Debug.DrawRay(transform.position, inputVector * 2, Color.red);

            // Camera direction
            Vector3 cameraDirection = cameraTarget.position - myCamera.position;
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

            if ( (Mathf.Abs(hori) > 1e-5 || Mathf.Abs(vert) > 1e-5) && !isAttacking)
            {
                float facingAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) / Mathf.PI * 180f;
                transform.eulerAngles = new Vector3(0.0f, facingAngle, 0.0f);
                _animator.SetBool("isMoving", true);

                //Angle Camera - unused , now handled in cam script
                // cameraTarget.transform.Rotate(0, hori * 0.2f, 0);

                return;

            }

        }
        _animator.SetBool("isMoving", false);

    }
    void PlayerMovement5() // my adaptation but broken? need to ask in person
    {

        if (!isAttacking)
        {
            // Input direction
            float hori = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");
            Vector3 inputVector = new Vector3(hori, 0.0f, vert);
            Debug.DrawRay(transform.position, inputVector * 2, Color.red);

            // Camera direction
            Vector3 cameraDirection = myCamera.forward;
            cameraDirection.y = 0.0f; // we dont want up/down dir
                                      //Vector3 option2= mainCamera.forward; //zero out 
            Debug.DrawRay(transform.position, cameraDirection.normalized * 2.0f, Color.green);

            // Movement angle
            inputVector = inputVector.normalized;
            float angle = Mathf.Atan2(inputVector.x, inputVector.z) / Mathf.PI * 180.0f;
            //Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.Euler(0f, angle, 0f));

            //angle of movement
            float angle2 =Mathf.Acos(Vector3.Dot(inputVector.normalized, cameraDirection.normalized));

            //since we want a unit vector for H assume lenth =1
            float lengthofX = 1 * Mathf.Cos(angle2);
            float lengthofY = 1 * Mathf.Sin(angle2);
            Vector3 movementDirection = new Vector3(lengthofY, 0, lengthofX);


            // Camera offset by input
           // Vector3 movementDirection = rotation.MultiplyVector(cameraDirection.normalized);
            Debug.DrawRay(transform.position, movementDirection.normalized * 2.0f, Color.blue);

            if ((Mathf.Abs(hori) > 1e-5 || Mathf.Abs(vert) > 1e-5))
            {
                float facingAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) / Mathf.PI * 180f;
                transform.eulerAngles = new Vector3(0.0f, facingAngle, 0.0f);
                _animator.SetBool("isMoving", true);

                //Angle Camera - unused? done in cam script i believe
                cameraTarget.transform.Rotate(0, hori * 0.2f, 0);

                return;

            }

        }



        _animator.SetBool("isMoving", false);
        if (_pc)
            _pc.setMobile(false);

    }


    public void MoveForward()
    {
        //Move forward via code (no root motion)
        transform.position += (transform.forward * speed * Time.deltaTime);
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

       // UIDirectionPressed._instance.UpdateText(s);
    }
    private void TellCanvas(string s)
    {
       // UIDirectionPressed._instance.UpdateText(s);
    }

    public void lockMovement()
    {
        if (_pc)
            _pc.setMobile(false);
        //TellCanvas("Attacking");
    }
    public void unlockMovement()
    {
        //TellCanvas("Free");
        if (_pc)
            _pc.setMobile(true);
    }
    public void CommitToAttack()
    {
        isAttacking = true;
    }
    public void UnCommitToAttack()
    {
        isAttacking = false;
    }
    public void comboOpen()
    {
        comboState = true;
    }
    public void comboClosed()
    {
        comboState = false;
    }
    public bool getComboState() => comboState;
    public void StartAttack()
    {
        comboOpen();
        //turn on colliders
        ToggleColliders(true);
    }
    public void EndAttack()
    {
        comboClosed();
        //turn off coliders
        ToggleColliders(false);
    }

    private void ToggleColliders(bool cond)
    {
        if (_wpCollider1)
            _wpCollider1.enabled = cond;
        if (_wpCollider2)
            _wpCollider2.enabled = cond;
    }

}
