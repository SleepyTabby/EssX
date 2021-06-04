using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScriptFollow : MonoBehaviour
{
    [Header("PlayerInfo")]
    [SerializeField] GameObject player;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] Vector3 rotateAround;
    [SerializeField] float distanceFromGround;
    

    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, (player.transform.position + cameraOffset), 0.1f);
        //transform.position = player.transform.position + cameraOffset;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0,-distanceFromGround,0),out hit, distanceFromGround))
        {
           // Debug.Log("gotem");
            if (hit.collider)
            {
               // Debug.Log("gotem");
                //Addheight.y += 0.5f;
                cameraOffset.y += 0.05f;
            }
        }
        if (Physics.Raycast(transform.position, new Vector3(0, -distanceFromGround, 0), out hit, (distanceFromGround + 1)))
        {
            if (!hit.collider)
            {
                cameraOffset.y -= 0.03f;
            }
        }
        //make the camera rotate around player


    }
}
