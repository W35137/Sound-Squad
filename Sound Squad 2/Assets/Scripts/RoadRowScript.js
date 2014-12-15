#pragma strict
//private var timer : float;
//private var distTraveled : float;

function Start () {
//	timer = 20.0f;
}

function Update () {
	if(this.transform.childCount == 0){
		Destroy(this.gameObject);
		Debug.Log("Children all died: suicide");
	}
	//distTraveled = Vector3.Distance(this.transform.position, this.transform.GetChild(0).transform.position);//(this.GetComponentInChildren(Transform).gameObject.transform.position));
	//Debug.Log(this.transform.position);
	//timer = timer - Time.deltaTime;
	//Debug.Log(timer);
	//if(distTraveled > 20){
		//Debug.Log ("delete road row");
		//Destroy(this.gameObject);
		//timer = 20.0f;
	//}
	//Debug.Log(distTraveled);
}