using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class BodyManager : MonoBehaviour {

	private KinectSensor Sensor;
	private BodyFrameReader Reader;
	private Body[] Data = null;

	// Return the body data read from the sensor
	public Body[] GetData() {
		return Data;
	}

	// Use this for initialization
	void Start () {
		Sensor = KinectSensor.GetDefault();
		
		if (Sensor != null) {
			Reader = Sensor.BodyFrameSource.OpenReader ();
			
			if (!Sensor.IsOpen) {
				Sensor.Open ();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Reader != null) {
			// Acquire body data
			var frame = Reader.AcquireLatestFrame();
			if (frame != null) {
				if (Data == null) {
					Data = new Body[Sensor.BodyFrameSource.BodyCount];
				}
				
				frame.GetAndRefreshBodyData(Data);
				
				frame.Dispose();
				frame = null;
			}
		}
	}

	// Called when the application closes
	void OnApplicationQuit()
	{
		if (Reader != null) {
			Reader.Dispose();
			Reader = null;
		}
		
		if (Sensor != null) {
			if (Sensor.IsOpen) {
				Sensor.Close();
			}
			
			Sensor = null;
		}
	}
}
