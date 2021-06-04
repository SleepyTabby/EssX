using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*
     overview
    rotate board with animations 



























     */
    [Header("Board")]
    [SerializeField] GameObject boardobj;
    [SerializeField] Rigidbody boardrbg;

    [Header("BoardPhysics")]
    [SerializeField] float speed;
    [SerializeField] float rotSpeed;
    [SerializeField] float cornerSpeed;
    [SerializeField] float cornerStopSpeed;
    bool rotateControlls; //if you rotate to far the player turns and changes controlls
    [SerializeField] int rotationAccuracy;
    [SerializeField] Vector2 minmaxAnglesL;
    [SerializeField] float[] turnAngles; //only set the max and minimum and in between get interpolated 

    [Header("SmoothAnimation")]
    [SerializeField] Transform idle;
    [SerializeField] Transform left;
    [SerializeField] Transform right;
    [SerializeField] Transform anker;
    bool[] currentInputs = {false, false, false, false };

    //animation manager
    //[Header("SmoothAnimation")]
    //keyframes interpolated provide rotations and if you want locations  
    //search trough the keyframes 
    //[SerializeField] List<Quaternion, Vector3> KeyFrames = new System.Collections.Generic.List<Quaternion[], Vector3[]>();

    void Start()
    {
        InterpolateAngles();
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
        PhysicsHandler();
    }
    //add tricks and shit 

    void InputHandler()
    {
        if (Input.GetKey(KeyCode.W))
        {
            currentInputs[0] = true;
        }
        else
        {
            currentInputs[0] = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            currentInputs[1] = true;
        }
        else
        {
            currentInputs[1] = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
            currentInputs[2] = true;
        }
        else
        {
            currentInputs[2] = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            currentInputs[3] = true;
        }
        else
        {
            currentInputs[3] = false;
        }
    }

    void AnimationManager()
    {
        if (currentInputs[0])//w
        {
            transform.rotation = Quaternion.Slerp(boardobj.transform.rotation, idle.rotation, 1);
        }

        if (currentInputs[1])//a
        {
            transform.rotation = Quaternion.Slerp(boardobj.transform.rotation, left.rotation, 1);
            //transform.forward = 
            //    transform.up
        }

        if (currentInputs[2])//s
        {
            transform.rotation = Quaternion.Slerp(boardobj.transform.rotation, anker.rotation, 1);
        }

        if (currentInputs[3])//d
        {
            transform.rotation = Quaternion.Slerp(boardobj.transform.rotation, right.rotation, 1);
        }
    }

    //make a procedurally generated music maker 
    void PhysicsHandler()
    {//implement animations stages same way as the speed
        // boardrbg
        if (currentInputs[0])//w
        {
            boardrbg.AddRelativeForce(Vector3.left * speed * Time.deltaTime, ForceMode.Force);
        }

        if (currentInputs[1])//a
        {
            boardrbg.AddRelativeForce(Vector3.forward * cornerStopSpeed * Time.deltaTime, ForceMode.Force);
            boardrbg.AddRelativeForce(-Vector3.left * cornerSpeed * Time.deltaTime, ForceMode.Force);
            boardrbg.AddTorque(-Vector3.up * rotSpeed * Time.deltaTime, ForceMode.Acceleration);
        }

        if (currentInputs[2])//s
        {

        }

        if (currentInputs[3])//d
        {
            boardrbg.AddRelativeForce(-Vector3.forward * cornerStopSpeed * Time.deltaTime, ForceMode.Force);
            boardrbg.AddRelativeForce(-Vector3.left * cornerSpeed * Time.deltaTime, ForceMode.Force);
            boardrbg.AddTorque(Vector3.up * rotSpeed * Time.deltaTime, ForceMode.Acceleration);
        }



        if(!currentInputs[0] && !currentInputs[1]&& !currentInputs[2] && !currentInputs[3])
        {
            //slowly rotate back to 0 
        }



        //automatic physics aplication when turning     make sure these will react to the landscape rotation find out how 
        //only perform when in the parameters
        speed += (speed * 0.04f) * Time.deltaTime;
        boardrbg.AddRelativeForce(Vector3.right * speed * Time.deltaTime, ForceMode.Force);
        for (int i = 1; i < (int)(turnAngles.Length * 0.5f); i++)
        {//right

            float automaticStopPush = cornerStopSpeed / (turnAngles.Length * 0.5f);
            float adjustedSpeed = automaticStopPush * i;
            if (boardrbg.rotation.eulerAngles.y > turnAngles[(i - 1)] && boardrbg.rotation.eulerAngles.y < turnAngles[i])
            {
                Debug.Log("im working an using this multiplier: " + adjustedSpeed);
                boardrbg.AddRelativeForce(-Vector3.forward * adjustedSpeed * Time.deltaTime, ForceMode.Force);
               // boardrbg.AddRelativeForce(Vector3.left * speed * Time.deltaTime, ForceMode.Force);
            }
        }




        //left
        for (int i = (int)(turnAngles.Length * 0.5f) + 1; i < turnAngles.Length; i++)
        {
                float automaticStopPushR = cornerStopSpeed / turnAngles.Length; // do this another way
                float adjustedSpeedR = automaticStopPushR * i;
                //Debug.Log(boardrbg.rotation.eulerAngles.y);
                if (boardrbg.rotation.eulerAngles.y < turnAngles[(i - 1)] && boardrbg.rotation.eulerAngles.y > turnAngles[i])
                {
                    Debug.Log("im working an using this multiplier: " + adjustedSpeedR);
                    boardrbg.AddRelativeForce(Vector3.forward * adjustedSpeedR * Time.deltaTime, ForceMode.Force);
                    //boardrbg.AddRelativeForce(-Vector3.left * speed * Time.deltaTime, ForceMode.Force);
                }
        }
    }

    void InterpolateAngles()
    {
        turnAngles = new float[rotationAccuracy];
        //middle point 0
        float m = (float)turnAngles.Length * 0.5f;
        int middlePoint = (int)m; // middlepoint 
        float leftDeviation = minmaxAnglesL.x - minmaxAnglesL.y;
        float middleLeftSlice = leftDeviation / middlePoint;
        for (int i = 0; i < middlePoint; i++)
        {
            //Left
            turnAngles[i] = 10 + (i * middleLeftSlice);


            //right
            int e = i + middlePoint;
            if (e <= turnAngles.Length)
            {
                turnAngles[e] = 360 - (i * middleLeftSlice); // - 10
            }
        }
    }
    //increase score the long player is in the air 
}
