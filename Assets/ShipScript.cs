using UnityEngine;
using System.Collections;

public class ShipScript : MonoBehaviour {

	public Transform tail;
	private Camera camera;
	private Rigidbody rigidBody;
	
	float rotationSpeed = 3.0f;
	float startingSpeed = 150.0f;

	int cameraTrail = 10;
	ArrayList positionHistory = new ArrayList();
	ArrayList rotationHistory = new ArrayList();

	float lastX = 0;
	float lastY = 0;
	
	public void Start () {
		this.camera    = Camera.main;
		this.rigidBody = this.GetComponent<Rigidbody>();
	}

	public void FixedUpdate () {
		float x = Input.GetAxis( "Horizontal" );
		float y = Input.GetAxis( "Vertical" );

		float normalX = ( x + lastX ) / 2.0f;
		float normalY = ( y + lastY ) / 2.0f;

		lastX = x;
		lastY = y;

		// Store current tail position to adjust camera at a delayed frame
		Quaternion currentRotation = transform.rotation;

		transform.Rotate( Vector3.right, normalY * rotationSpeed );
		transform.Rotate( Vector3.back, normalX * rotationSpeed );

		Vector3 tailPosition = this.tail.position;

		this.positionHistory.Add( tailPosition );
		this.rotationHistory.Add( currentRotation );

		this.updateCameraLocation();

		// Move ship forward
		Vector3 test = this.transform.position - tailPosition;
		Debug.Log( test * this.startingSpeed );
		this.rigidBody.velocity = test * this.startingSpeed * Time.deltaTime;
	}

	private void updateCameraLocation() {
		if ( this.positionHistory.Count >= cameraTrail ) {
			Vector3 position = ( Vector3 ) this.positionHistory[ 0 ];
			Quaternion rotation = ( Quaternion ) this.rotationHistory[ 0 ];

			this.positionHistory.RemoveAt( 0 );
			this.rotationHistory.RemoveAt( 0 );

			this.camera.transform.position = position;
			this.camera.transform.rotation = rotation;
		}
	}
}
