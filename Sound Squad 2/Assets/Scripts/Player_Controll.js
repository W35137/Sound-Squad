#pragma strict
private var moveSpeed : float;
private var position : Vector3;
var projectile : GameObject;
var bulletSpeed : float;
var parEmit: ParticleEmitter;

function Start () {
	moveSpeed = 0.1f;
	bulletSpeed = 200.0f;
}

function Update () {
	 if (Input.GetKey(KeyCode.LeftArrow))
        {
            position = this.transform.position;
            position.x = position.x - moveSpeed;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            position = this.transform.position;
            position.x = position.x + moveSpeed;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.UpArrow ) && this.transform.position.z < -15)
        {
            position = this.transform.position;
            position.z = position.z + moveSpeed;
            this.transform.position = position;
        }
        if (Input.GetKey(KeyCode.DownArrow) && this.transform.position.z > -21)
        {
            position = this.transform.position;
            position.z = position.z - moveSpeed;
            this.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Mouse0))
        {
        	var clone : GameObject;
    		clone = Instantiate(projectile, transform.position - new Vector3(0, 0,-0.5), transform.rotation);
    		clone.rigidbody.AddForce(Vector3.forward * bulletSpeed);
    		Destroy(clone, 3.5);
        }
}

function OnBecameInvisible() 
{
	//minus one life
	this.gameObject.transform.position = new Vector3(0.0f,-3.5f,-20.0f);
}