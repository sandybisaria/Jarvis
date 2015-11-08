using UnityEngine;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodyManagerSpawner : MonoBehaviour {
	public GameObject BodyManager;
	private BodyManager SourceManager;

	// Store the GameObjects with their associated IDs
	private Dictionary<ulong, GameObject> Bodies = new Dictionary<ulong, GameObject>();

	private Kinect.JointType[] Joints = 
	{
		Kinect.JointType.HandTipLeft, Kinect.JointType.HandTipRight,
		Kinect.JointType.ThumbLeft, Kinect.JointType.ThumbRight,
		Kinect.JointType.HandLeft, Kinect.JointType.HandRight
	};

	public Material BoneMaterial;

	void Update() {
		if (BodyManager == null) {
			return;
		}

		SourceManager = BodyManager.GetComponent<BodyManager>();
		if (SourceManager == null) {
			return;
		}

		Kinect.Body[] data = SourceManager.GetData();
		if (data == null) {
			return;
		}

		List<ulong> trackedIds = new List<ulong>();
		foreach(var body in data) {
			if (body == null) {
				continue;
			}
			
			if (body.IsTracked) {
				trackedIds.Add(body.TrackingId);
			}
		}

		List<ulong> knownIds = new List<ulong>(Bodies.Keys);
		// First delete untracked bodies
		foreach(ulong trackingId in knownIds) {
			if(!trackedIds.Contains(trackingId)) {
				Destroy(Bodies[trackingId]);
				Bodies.Remove(trackingId);
			}
		}

		foreach(var body in data) {
			if (body == null) {
				continue;
			}
			
			if(body.IsTracked) {
				if(!Bodies.ContainsKey(body.TrackingId)) {
					Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
				}
				
				RefreshBodyObject(body, Bodies[body.TrackingId]);
			}
		}
	}

	private GameObject CreateBodyObject(ulong id) {
		GameObject body = new GameObject("Body:" + id);
		
		foreach (Kinect.JointType jt in Joints) {
			GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            if (jt == Kinect.JointType.HandLeft || jt == Kinect.JointType.HandRight)
            {
                Rigidbody rigidBody = jointObj.AddComponent<Rigidbody>();
                rigidBody.mass = 10;
                rigidBody.drag = 100;
                rigidBody.angularDrag = 100;
                rigidBody.maxAngularVelocity = 0;
                rigidBody.useGravity = false;
                createPalm(jointObj.transform);
            }
            else if (jt == Kinect.JointType.HandTipLeft || jt == Kinect.JointType.HandTipRight)
            {
                createHandTip(jointObj.transform);
            }
            else
            {
                // Determines the size of the joints
                float objScale = 20f;
                jointObj.transform.localScale = new Vector3(objScale, objScale, objScale);
            }

            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }
		
		return body;
	}

    private void createPalm(Transform jointTransform) {
        // Determines the size of the joints
        float xObjScale = 20f;
        float yObjScale = 50f;
        float zObjScale = 50f;
        jointTransform.localScale = new Vector3(xObjScale, yObjScale, zObjScale);

        jointTransform.gameObject.AddComponent<HandPalmAttribute>();
    }

    private void createHandTip(Transform jointTransform) {
        // Determines the size of the joints
        float xObjScale = 20f;
        float yObjScale = 20f;
        float zObjScale = 40f;
        jointTransform.localScale = new Vector3(xObjScale, yObjScale, zObjScale);
    }

    private void updatePalm(Transform jointTransform, Transform thumbTransform, Kinect.HandState handState) {
        jointTransform.LookAt(thumbTransform);
        GameObject jointObj = jointTransform.gameObject;
        HandPalmAttribute hpa = jointObj.GetComponent<HandPalmAttribute>();
        hpa.UpdateState(handState);
        jointObj.GetComponent<Renderer>().material.color = hpa.GetStateColor();
    }

	private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject) {

		foreach (Kinect.JointType jt in Joints) {
			Kinect.Joint sourceJoint = body.Joints[jt];
            
			Transform jointTransform = bodyObject.transform.FindChild(jt.ToString());
			Vector3 vector = GetVector3FromJoint(sourceJoint);
			jointTransform.localPosition = vector;

            if (jt == Kinect.JointType.HandLeft) {
                Transform thumbTransform = bodyObject.transform.FindChild(Kinect.JointType.HandTipLeft.ToString());
                updatePalm(jointTransform, thumbTransform, body.HandLeftState);                
            } else if (jt == Kinect.JointType.HandRight) {
                Transform thumbTransform = bodyObject.transform.FindChild(Kinect.JointType.HandTipRight.ToString());
                updatePalm(jointTransform, thumbTransform, body.HandRightState);
            }
        }
	}
	
	private static Color GetColorForState(Kinect.TrackingState state) {
		switch (state) {
		case Kinect.TrackingState.Tracked:
			return Color.green;
			
		case Kinect.TrackingState.Inferred:
			return Color.red;
			
		default:
			return Color.black;
		}
	}
	
	private static Vector3 GetVector3FromJoint(Kinect.Joint joint) {
		int scaleFactor = 1000;
		int zScaleFactor = -scaleFactor;
		int zShiftFactor = 1100;
		return new Vector3(joint.Position.X * scaleFactor,
		                   joint.Position.Y * scaleFactor,
		                   joint.Position.Z * zScaleFactor + zShiftFactor);
	}
}