#pragma strict

function Start () {

}

function Update () {
	
}
function OnCollisionEnter(clsn : Collision) {
	if(clsn.collider.tag.Equals("bullet"))
	{
		Debug.Log("got into check");
		Destroy(this.gameObject, 0.0);
		Destroy(clsn.gameObject);
	}
	Debug.Log(clsn.gameObject.name);
	Debug.Log((clsn.collider.tag.Equals("bullet")));
}