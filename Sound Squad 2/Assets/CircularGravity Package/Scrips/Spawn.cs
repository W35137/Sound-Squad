/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 2.00
 * Created Date: 11/24/13 
 * Last Updated: 11/24/13
 *  
 *  Description: 
 *      
 *      Used to spawn gameobjects.   
 * 
 *  Inputs:
 * 
 *      inputControl: Control to spawn the gameobject.
 *      
 *      spawnObject: Gameobject to spawn.
 *      
 *      spawnLife: Life time of spawned gameobject.
 *      
*******************************************************************************************/
using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour
{
    //Input to spawn
    public string inputControl = "Jump";

    //GameObject to spawn
    public GameObject spawnObject;

    //Spawn life
    public float spawnLife = 10f;

    //Use this for initialization
    void Start()
    {
    }

    //Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown(inputControl))
        {
            GameObject instance = Instantiate(spawnObject, transform.position, transform.rotation) as GameObject;

            DestroyObject(instance, spawnLife);
        }
    }
}