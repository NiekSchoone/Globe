using UnityEngine;
using System.Collections;

public class TrackFingerMovement : MonoBehaviour {


    private HandModel handModel;
    public Dig digOrBuildController;
    public GameObject handcontroller;
	// Use this for initialization
	void Start () 
    {
        handcontroller = GameObject.FindGameObjectWithTag("HandController");
        handModel = GetComponent<HandModel>();
        digOrBuildController = GameObject.FindGameObjectWithTag("GameController").GetComponent<Dig>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (this.name.Contains("Left"))
        {
            //dig
            digOrBuildController.DigFunction(handModel.fingers[1].GetBoneCenter(3));
        }
        else
        {
            //build
            digOrBuildController.Build(handModel.fingers[1].GetBoneCenter(3));
        }
	}
}
