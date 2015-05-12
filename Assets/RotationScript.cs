using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour 
{
	public bool isRotating;
	private bool rotateRight;
	private bool rotateLeft;
	
	void Start () 
	{
		isRotating = false;
		rotateRight = false;
		rotateLeft = false;
	}
	
	void Update () 
	{
		//ChoiceRotation("right");
		//ChoiceRotation("left");
		//RotateRight()
		//RotateLeft()
	}

	public void ChoiceRotation(string direction)
	{
		if(direction == "left")
		{
			isRotating = true;
			rotateLeft = true;
			transform.Rotate(Vector3.up * (Time.deltaTime * 5));
		}else if(direction == "right")
		{
			isRotating = true;
			rotateRight = true;
			transform.Rotate(Vector3.down * (Time.deltaTime * 5));
		}else
		{
			isRotating = false;
			rotateLeft = false;
			rotateRight = false;
		}
	}

	/*public void RotateLeft()
	{
		transform.Rotate(Vector3.up * (Time.deltaTime * 5));
	}
	public void RotateRight()
	{
		transform.Rotate(Vector3.down * (Time.deltaTime * 5));
	}*/
}
