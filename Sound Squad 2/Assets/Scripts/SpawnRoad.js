#pragma strict
public var roadPiece : GameObject;
//private var instRoadPiece : GameObject;
private var spawned : boolean;
public var timerInterval: double;

function Start () {
	spawned = false;
}

function Update () {

	if( spawned == true)
	{
		timerInterval += Time.deltaTime;
		if(GameObject.Find("RoadSpawnRow(Clone)") == null)
		{
			spawned = false;
			Debug.Log("Ran out of road rows, so made more.(This shouldn't happen.)");
		}
	}
	if(timerInterval > 1.0f)
	{
		timerInterval = 0.0f;
		spawned = false;
	}
}

function OnTriggerExit (other : Collider) {
	
	if(spawned == false)
	{
		//Debug.Log("Trigger exited.");
		var instRoadPiece = Instantiate (roadPiece, Vector3(-0.019, -4.94, -6.35), Quaternion.identity);
		spawned = true;
		
	}
}