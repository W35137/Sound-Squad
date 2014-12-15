/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 2.00
 * Created Date: 11/25/13 
 * Last Updated: 11/25/13
 *  
 *  Description: 
 *  
 *      Rotation controls.
 * 
 * 
 *  Inputs:
 * 
 *      speed: How fast to rotate the object.
 * 
*******************************************************************************************/
using UnityEngine;
using System.Collections;

public class RotateControls : MonoBehaviour
{
    //How fast the turret turns
    public float speed = 50f;

    //Use this for initialization
    void Start()
    {

    }

    //Update is called once per frame
    void Update()
    {
        this.transform.rotation = this.transform.localRotation;

        float horMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        if (horMovement != 0)
        {
            this.transform.Rotate(new Vector3(0f, horMovement, 0f) * speed * Time.deltaTime);
        }

        if (verMovement != 0)
        {
            this.transform.Rotate(new Vector3(0f, 0f, verMovement) * speed * Time.deltaTime);
        }
    }
}
