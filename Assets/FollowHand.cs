using UnityEngine;
using System.Collections;

public class FollowHand : MonoBehaviour {

    private Vector3 startPosition;
    bool mayIFollow = false;

    void Start() 
    {
        startPosition = transform.position;
    }

    public void OntriggerStay(Collider other) 
    {
        if (other.tag == "Handpalm")
        {
            Vector3 temp = other.transform.position;
            temp.x = transform.position.x;
            temp.y = transform.position.y;
            transform.position = temp;
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        transform.position = startPosition;
    }

    public void setMayIFollow(bool input) 
    {
        mayIFollow = input;
    }

    public bool getMayIFollow() 
    {
        return mayIFollow;
    }
}
