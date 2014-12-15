/*******************************************************************************************
 *       Author: Lane Gresham, AKA LaneMax
 *         Blog: http://lanemax.blogspot.com/
 *      Version: 2.00
 * Created Date: 04/15/13 
 * Last Updated: 11/25/13
 *  
 *  Description: 
 *  
 *      The Circular Gravity Force v2.0 script allows you to easily drag and drop a customizable
 *      gravitational force on to any GameObject within your project with a positive or negative
 *      power. Features include, shaping the gravitational force to ether a sphere, capsule, or
 *      even a raycast, also customizable force points giving you the flexibility to set one or
 *      multiple points where the force is coming from within the gravitational area giving you
 *      tons of ways to customize your effect, also tag and trigger filtering allows you to 
 *      filter out or include unwanted objects. Circular Gravity Force makes it incredibly easy 
 *      to add customizable gravity/force to planets, cannons, magnets, explosions, tractor 
 *      beams, or even rail guns, the possibilities are endless!
 *      
 *  How To Use:
 *  
 *      Simply drag and drop / assign this script to whatever GameObject you would like the
 *      circular gravity force to emanate from.
 * 
 *  Inputs:
 * 
 *      enable: Enable/Disable CircularGravity.
 * 
 * 		shape: Shape of the CirularGravity
 * 		
 * 		forcePoint: Force starting point where the force is created, if not supplied it will use this.transform.position instead.
 * 	
 * 		forcePoints: Force points allowing you to add multiple force points, be careful this can affect performance if not used correctly.
 * 
 *      forcePower: Power for the force, can be negative or positive.
 *      
 *      sizeProperties->
 *          size: Size of the force.
 *      	capsuleRadius: apsule Radius, used only when using the capsule shape.
 * 
 *      triggerAreaFilter->
 *          triggerArea: This is the trigger that will be used for the area filtering.
 *          triggerAreaFilterOptions: Trigger filter options.
 * 
 *      tagFilter->
 *          tagFilterOptions: Tag filter options.
 *          tagsList: Tags used for the filter option.
 *      
 *      pulseProperties->
 *          pulse: Enable a Pulse.
 *          speed: Pulsing speed if pulse if enabled.
 *          minSize: Minimum pulse size.
 *          maxSize: Maximum pulse size.
 * 
 *      DrawGravityProperties->
 *          thickness: Thinkness of the line drawn.
 *          gravityLineMaterial: Material for line drawn.
 *          drawGravityForce: Used to see gravity area of gravity.
 *          
 * 
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using System.Linq;

public class CircularGravity : MonoBehaviour
{
    #region Properties

    //Enable/Disable CircularGravity
    public bool enable = true;

    //Shape of the CirularGravity
    public Shape shape = CircularGravity.Shape.Sphere;

    //Force Point where the force is created, if not supplied it will use this.transform.position instead
    public GameObject forcePoint;

    //Force points allowing you to add multiple force points, be careful this can affect performance if not used correctly
    public GameObject[] forcePoints;

    //Power for the force, can be negative or positive
    public float forcePower = 10f;

    //Radius properties
    [System.Serializable]
    public class SizeProperties
    {
        //Radius of the force
        public float size = 10f;

        //Capsule Radius, used only when using the capsule shape
        public float capsuleRadius = 2f;
    }
    public SizeProperties sizeProperties;

    //Trigger Area Filter
    [System.Serializable]
    public class TriggerAreaFilter
    {
        //Trigger Object
        public Collider triggerArea;

        //Trigger Options
        public enum TriggerAreaFilterOptions
        {
            Disabled,
            OnlyEffectWithinTigger,
            DontEffectWithinTigger,
        }
        public TriggerAreaFilterOptions triggerAreaFilterOptions = TriggerAreaFilterOptions.Disabled;
    }
    public TriggerAreaFilter triggerAreaFilter;

    //Tag filtering options
    [System.Serializable]
    public class TagFilter
    {
        //Tag filter options
        public enum TagFilterOptions
        {
            Disabled,
            OnlyEffectListedTags,
            DontEffectListedTags,
        }
        public TagFilterOptions tagFilterOptions = TagFilterOptions.Disabled;

        //Tags used for the filter option
        public string[] tagsList;
    }
    public TagFilter tagFilter;

    //Pulse properties
    [System.Serializable]
    public class PulseProperties
    {
        //Enable a Pulse
        public bool pulse = false;

        //Pulsing speed if pulse if enabled
        public float speed = 50f;

        //Minimum pulse size
        public float minSize = 1f;

        //Maximum pulse size
        public float maxSize = 5f;
    }
    public PulseProperties pulseProperties;

    //Draw gravity properties
    [System.Serializable]
    public class DrawGravityProperties
    {
        //Thinkness of the line drawn
        public float thickness = 0.05f;

        public Material gravityLineMaterial;

        //Used to see gravity area of gravity
        public bool drawGravityForce = true;
    }
    public DrawGravityProperties drawGravityProperties;

    //Used to tell whether to add or subtract to pulse
    private bool pulse_Positive;

    //Line Object
    static private string CirularGravityLineName = "CirularGravityForce_LineDisplay";
    private GameObject CirularGravityLine;

    //Force Types
    public enum Shape
    {
        Sphere,
        Capsule,
        RayCast,
    }

    #endregion

    #region Unity Events

    void Awake()
    {
        #region Default Settings

        //Used for when dynamically creating a CircularGracity object
        if (sizeProperties == null)
        {
            sizeProperties = new SizeProperties();
        }
        if (triggerAreaFilter == null)
        {
            triggerAreaFilter = new TriggerAreaFilter();
        }
        if (tagFilter == null)
        {
            tagFilter = new TagFilter();
        }
        if (pulseProperties == null)
        {
            pulseProperties = new PulseProperties();
        }
        if (drawGravityProperties == null)
        {
            drawGravityProperties = new DrawGravityProperties();
        }

        //Sets up pulse
        if (pulseProperties.pulse)
        {
            sizeProperties.size = pulseProperties.minSize;
            pulse_Positive = true;
        }

        #endregion
    }

    //Use this for initialization
    void Start()
    {
    }

    //Update is called once per frame
    void Update()
    {
        if (enable)
        {
            if (pulseProperties.pulse)
            {
                CalculatePulse();
            }

            //Sets up the line that gets rendered showing the area of forces
            if (drawGravityProperties.drawGravityForce)
            {
                if (CirularGravityLine == null)
                {
                    //Creates line for showing the force
                    CirularGravityLine = new GameObject(CirularGravityLineName);
                    CirularGravityLine.transform.position = this.transform.position;
                    CirularGravityLine.transform.parent = this.gameObject.transform;
                    CirularGravityLine.AddComponent<LineRenderer>();
                }

                DrawGravityForce();
            }
            else
            {
                if (CirularGravityLine != null)
                {
                    //Destroys line when not using
                    Destroy(CirularGravityLine);
                }
            }
        }
        else
        {
            if (CirularGravityLine != null)
            {
                //Destroys line when not using
                Destroy(CirularGravityLine);
            }
        }
    }

    //This function is called every fixed frame
    void FixedUpdate()
    {
        if (enable)
        {
            CalculateAndEstimateForce();
        }
    }

    #endregion

    #region Functions

    //Calculatie the given pulse
    private void CalculatePulse()
    {
        if (pulseProperties.pulse)
        {
            if (pulse_Positive)
            {
                if (sizeProperties.size <= pulseProperties.maxSize)
                    sizeProperties.size = sizeProperties.size + (pulseProperties.speed * Time.deltaTime);
                else
                    pulse_Positive = false;
            }
            else
            {
                if (sizeProperties.size >= pulseProperties.minSize)
                    sizeProperties.size = sizeProperties.size - (pulseProperties.speed * Time.deltaTime);
                else
                    pulse_Positive = true;
            }
        }
    }

    //Calculate and Estimate the force
    private void CalculateAndEstimateForce()
    {
        Collider[] colliders = null;
        RaycastHit[] raycastHits = null;

        if (shape == Shape.Sphere)
        {
            #region Sphere

            colliders = Physics.OverlapSphere(this.transform.position, sizeProperties.size);

            if (colliders.Length > 0)
            {
                var hits =
                    from h in colliders
                    select h;

                hits = hits.Where(h => h.rigidbody != this.rigidbody);
                hits = hits.Where(h => h.rigidbody == true);

                //Used for Tag filtering
                switch (tagFilter.tagFilterOptions)
                {
                    case TagFilter.TagFilterOptions.Disabled:
                        break;
                    case TagFilter.TagFilterOptions.OnlyEffectListedTags:
                        hits = hits.Where(h => tagFilter.tagsList.Contains<string>(h.tag));
                        break;
                    case TagFilter.TagFilterOptions.DontEffectListedTags:
                        hits = hits.Where(h => !tagFilter.tagsList.Contains<string>(h.tag));
                        break;
                }

                //Used for Trigger Area Filtering
                switch (triggerAreaFilter.triggerAreaFilterOptions)
                {
                    case TriggerAreaFilter.TriggerAreaFilterOptions.Disabled:
                        break;
                    case TriggerAreaFilter.TriggerAreaFilterOptions.OnlyEffectWithinTigger:
                        hits = hits.Where(h => triggerAreaFilter.triggerArea.collider.bounds.Contains(h.transform.position));
                        break;
                    case TriggerAreaFilter.TriggerAreaFilterOptions.DontEffectWithinTigger:
                        hits = hits.Where(h => !triggerAreaFilter.triggerArea.collider.bounds.Contains(h.transform.position));
                        break;
                    default:
                        break;
                }

                foreach (var hit in hits)
                {
                    //If forcePoint is not supplied and no forcePoints, it will use the this.transform.position
                    if (forcePoint == null && forcePoints.Length == 0)
                    {
                        hit.rigidbody.AddExplosionForce(forcePower, this.transform.position, sizeProperties.size);
                    }
                    else if (forcePoint != null)
                    {
                        hit.rigidbody.AddExplosionForce(forcePower, forcePoint.transform.position, sizeProperties.size);
                    }

                    //Adds force to all the GameObjects listed in the forcePoints array
                    if (forcePoints.Length > 0)
                    {
                        foreach (var forcepoint in forcePoints)
                        {
                            GameObject updateForcepoint = forcepoint as GameObject;

                            hit.rigidbody.AddExplosionForce(forcePower, updateForcepoint.transform.position, sizeProperties.size);
                        }
                    }
                }
            }
            #endregion
        }
        else
        {
            #region RayCast

            //Circular Gravity Force Transform
            Transform cgfTran = this.transform;

            if (shape == Shape.RayCast)
            {
                raycastHits = Physics.RaycastAll(cgfTran.position, cgfTran.rotation * Vector3.forward, sizeProperties.size);
            }
            else if (shape == Shape.Capsule)
            {
                raycastHits = Physics.CapsuleCastAll
                (
                    cgfTran.position,
                    cgfTran.position + ((cgfTran.rotation * Vector3.back)),
                    sizeProperties.capsuleRadius,
                    cgfTran.position - ((cgfTran.position + (cgfTran.rotation * (Vector3.back) )))
                );  
            }

            if (raycastHits.Length > 0)
            {
                var rayhits =
                from h2 in raycastHits
                select h2;

                rayhits = rayhits.Where(h2 => h2.rigidbody != this.rigidbody);
                rayhits = rayhits.Where(h2 => h2.rigidbody == true);

                //Used for Tag filtering
                switch (tagFilter.tagFilterOptions)
                {
                    case TagFilter.TagFilterOptions.Disabled:
                        break;
                    case TagFilter.TagFilterOptions.OnlyEffectListedTags:
                        rayhits = rayhits.Where(h2 => tagFilter.tagsList.Contains<string>(h2.transform.gameObject.tag));
                        break;
                    case TagFilter.TagFilterOptions.DontEffectListedTags:
                        rayhits = rayhits.Where(h2 => !tagFilter.tagsList.Contains<string>(h2.transform.gameObject.tag));
                        break;
                }

                //Used for Trigger Area Filtering
                switch (triggerAreaFilter.triggerAreaFilterOptions)
                {
                    case TriggerAreaFilter.TriggerAreaFilterOptions.Disabled:
                        break;
                    case TriggerAreaFilter.TriggerAreaFilterOptions.OnlyEffectWithinTigger:
                        rayhits = rayhits.Where(h => triggerAreaFilter.triggerArea.collider.bounds.Contains(h.transform.position));
                        break;
                    case TriggerAreaFilter.TriggerAreaFilterOptions.DontEffectWithinTigger:
                        rayhits = rayhits.Where(h2 => !triggerAreaFilter.triggerArea.collider.bounds.Contains(h2.transform.position));
                        break;
                    default:
                        break;
                }

                foreach (var hit in rayhits)
                {
                    //If forcePoint is not supplied and no forcePoints, it will use the this.transform.position
                    if (forcePoint == null && forcePoints.Length == 0)
                    {
                        hit.rigidbody.AddExplosionForce(forcePower, cgfTran.position, sizeProperties.size);
                    }
                    else if (forcePoint != null)
                    {
                        hit.rigidbody.AddExplosionForce(forcePower, forcePoint.transform.position, sizeProperties.size);
                    }

                    //Adds force to all the GameObjects listed in the forcePoints array
                    if (forcePoints.Length > 0)
                    {
                        foreach (var forcepoint in forcePoints)
                        {
                            GameObject updateForcepoint = forcepoint as GameObject;

                            hit.rigidbody.AddExplosionForce(forcePower, updateForcepoint.transform.position, sizeProperties.size);
                        }
                    }
                }
            }

            #endregion
        }
    }

    #endregion

    #region Draw

    //Draws effected area by forces
    private void DrawGravityForce()
    {
        //Circular Gravity Force Transform
        Transform cgfTran = this.transform;

        Color DebugGravityLineColor;

        if (forcePower == 0)
            DebugGravityLineColor = Color.white;
        else if (forcePower > 0)
            DebugGravityLineColor = Color.green;
        else
            DebugGravityLineColor = Color.red;

        //Line setup
        LineRenderer lineRenderer = CirularGravityLine.GetComponent<LineRenderer>();
        lineRenderer.SetWidth(drawGravityProperties.thickness, drawGravityProperties.thickness);
        lineRenderer.material = drawGravityProperties.gravityLineMaterial;
        lineRenderer.SetColors(DebugGravityLineColor, DebugGravityLineColor);

        //Renders type outline
        switch (shape)
        {
            case Shape.Sphere:

                //Models line
                lineRenderer.SetVertexCount(12);

                lineRenderer.SetPosition(0, cgfTran.position + ((cgfTran.rotation * Vector3.up) * sizeProperties.size));
                lineRenderer.SetPosition(1, cgfTran.position);
                lineRenderer.SetPosition(2, cgfTran.position + ((cgfTran.rotation * Vector3.down) * sizeProperties.size));
                lineRenderer.SetPosition(3, cgfTran.position);
                lineRenderer.SetPosition(4, cgfTran.position + ((cgfTran.rotation * Vector3.left) * sizeProperties.size));
                lineRenderer.SetPosition(5, cgfTran.position);
                lineRenderer.SetPosition(6, cgfTran.position + ((cgfTran.rotation * Vector3.right) * sizeProperties.size));
                lineRenderer.SetPosition(7, cgfTran.position);
                lineRenderer.SetPosition(8, cgfTran.position + ((cgfTran.rotation * Vector3.forward) * sizeProperties.size));
                lineRenderer.SetPosition(9, cgfTran.position);
                lineRenderer.SetPosition(10, cgfTran.position + ((cgfTran.rotation * Vector3.back) * sizeProperties.size));
                lineRenderer.SetPosition(11, cgfTran.position);

                break;

            case Shape.Capsule:

                //Model Capsule
                lineRenderer.SetVertexCount(17);

                //Starting Point
                lineRenderer.SetPosition(0, cgfTran.position + ((cgfTran.rotation * Vector3.up) * sizeProperties.capsuleRadius));
                lineRenderer.SetPosition(1, cgfTran.position);
                lineRenderer.SetPosition(2, cgfTran.position + ((cgfTran.rotation * Vector3.down) * sizeProperties.capsuleRadius));
                lineRenderer.SetPosition(3, cgfTran.position);
                lineRenderer.SetPosition(4, cgfTran.position + ((cgfTran.rotation * Vector3.left) * sizeProperties.capsuleRadius));
                lineRenderer.SetPosition(5, cgfTran.position);
                lineRenderer.SetPosition(6, cgfTran.position + ((cgfTran.rotation * Vector3.right) * sizeProperties.capsuleRadius));
                lineRenderer.SetPosition(7, cgfTran.position);

                //Middle Line
                Vector3 endPointLoc = cgfTran.position + ((cgfTran.rotation * Vector3.forward) * sizeProperties.size);
                lineRenderer.SetPosition(8, endPointLoc);

                //End Point
                lineRenderer.SetPosition(9, endPointLoc + ((cgfTran.rotation * Vector3.up) * sizeProperties.capsuleRadius));
                lineRenderer.SetPosition(10, endPointLoc);
                lineRenderer.SetPosition(11, endPointLoc + ((cgfTran.rotation * Vector3.down) * sizeProperties.capsuleRadius));
                lineRenderer.SetPosition(12, endPointLoc);
                lineRenderer.SetPosition(13, endPointLoc + ((cgfTran.rotation * Vector3.left) * sizeProperties.capsuleRadius));
                lineRenderer.SetPosition(14, endPointLoc);
                lineRenderer.SetPosition(15, endPointLoc + ((cgfTran.rotation * Vector3.right) * sizeProperties.capsuleRadius));
                lineRenderer.SetPosition(16, endPointLoc);

                break;

            case Shape.RayCast:

                //Model Line
                lineRenderer.SetVertexCount(2);

                lineRenderer.SetPosition(0, cgfTran.position);
                lineRenderer.SetPosition(1, cgfTran.position + ((cgfTran.rotation * Vector3.forward) * sizeProperties.size));

                break;
        }
    }

    #endregion
}