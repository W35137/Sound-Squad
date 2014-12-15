#pragma strict
private var used : boolean;
//private var timer : float;
private var distTraveled : float;
//private var randGen : Random;
private var randNum : int;

var amplitude: float = 0.5; // platform excursion (+/- 5 units, in this case)
var speed: float = 0.5; // movements per second
var direction: Vector3 = Vector3.up; // direction relative to the platform
private var p0: Vector3;
var colCheck: boolean = false;

function Start () {
used = false;
this.rigidbody.AddForce(-transform.forward*1000);
randNum = Random.Range(0, 20);
//timer = 20.0f;
}

function Update () {
	distTraveled = Vector3.Distance(this.transform.position, this.transform.parent.transform.position);
	if(distTraveled > 20){
		Debug.Log ("delete child");
		Destroy(this.gameObject);
		//timer = 20.0f;
	}
	
	if(randNum == 1  && !colCheck)
	{
		p0 = transform.position;
		//while (true){
			// convert direction to local space
			var dir = transform.TransformDirection(direction);
			// set platform position:
			transform.position = p0+(amplitude*0.1)*dir*Mathf.Sin(6*speed*Time.time);
			//yield; // let Unity free till the next frame
		//}
	}
	//Debug.Log (this.rigidbody.GetPointVelocity(this.transform.InverseTransformPoint(Vector3(-0.019, -4.94, -6.35))));
	//Debug.Log(this.rigidbody.velocity);
	//if(!this.renderer.isVisible)
	//{
		//Destroy(this);
	//}
	//timer = timer - Time.deltaTime;
	//if(timer <= 0){
		//Debug.Log ("delete road row");
		//Destroy(this.gameObject);
		//timer = 20.0f;
	//}
}

function OnBecameInvisible() 
{
	//Debug.Log ("boom");
    //Destroy(this.gameObject);
}

function OnCollisionEnter(other: Collision)
{

	if(other.collider.tag != "levelCube")
	{
		colCheck = true;
		Debug.Log("WOWOWOOW");
		
		if(other.collider.tag == "Player")
		{
			GameObject.Destroy(other.collider.gameObject);
		}
	}
}

function OnTriggerExit (other : Collider) {
	if(other.name == "SpawnRoadCubeCheck" && used == false)
	{
		this.collider.isTrigger = false;
		used = true;
	}
}
function OnTriggerEnter (other : Collider) {
	if(other.name == "SpawnRoadCubeCheck" && used == true){
		Destroy(this.gameObject);
	}
}