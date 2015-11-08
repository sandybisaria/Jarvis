using UnityEngine;
using System.Collections;

public class FileBoxAttribute : MonoBehaviour {

	public string typeThingy = "filebox";
	public string location = "";
    
    private bool isBeingHeld = false;
    private GameObject lastPalm = null;

    void OnCollisionEnter(Collision col) {
        GameObject lastCollider = col.gameObject;
        if (lastCollider != null) {
            Rigidbody colliderBody = lastCollider.GetComponent<Rigidbody>();
            HandPalmAttribute hpa = lastCollider.GetComponent<HandPalmAttribute>();
            if (colliderBody != null && hpa != null)
            {
                Rigidbody thisBody = GetComponent<Rigidbody>();
                if (hpa.GetHandState() == "closed")
                {
                    this.transform.parent = colliderBody.transform;
                    this.transform.localPosition = Vector3.zero;
                    thisBody.useGravity = false;
                    //thisBody.isKinematic = false;
                    //thisBody.WakeUp();
                    isBeingHeld = true;
                    lastPalm = lastCollider;
                }
            }
        }
    }

    void Update()
    {
        if (lastPalm != null)
        {
            HandPalmAttribute hpa = lastPalm.GetComponent<HandPalmAttribute>();
            if (hpa.GetHandState() == "open" && isBeingHeld)
            {
                Rigidbody thisBody = GetComponent<Rigidbody>();
                isBeingHeld = false;
                //thisBody.isKinematic = true;
                thisBody.useGravity = true;
                this.transform.parent = null;
            }
        }
    }
}
