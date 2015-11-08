using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class test : MonoBehaviour {

	// Use this for initialization

	string[] libraryFile = new string[15];
	public string typeThingy = "filebox";
	public string location = "";


	void Start () {
		int x = 0;
		DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Files");
		
		foreach (DirectoryInfo d in dir.GetDirectories())
		{
			//Console.WriteLine("{0, -30}\t directory", d.Name);
            Debug.Log("directory !: " + d.Name);

        }

        foreach (FileInfo f in dir.GetFiles())
		{

//			Console.WriteLine("{0, -30}\t File", f.Name);
			Debug.Log("File : "+ "at location "+ x + f.Name);
			Debug.Log("XXXXXXXXFile Full detail : "+ f);
			x = x +1;
			this.location = f.FullName;
			libraryFile[x] = f.FullName;

		}

		Debug.Log("Start of Array iteration !");
		for(int i=0; i< libraryFile.Length; i++){
			Debug.Log("Array info at"+i+ " " +libraryFile[i]);
		}


		for(int i=0; i< x; i++){
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.name = libraryFile[i];
			Rigidbody rigidBody = cube.AddComponent<Rigidbody>();
            rigidBody.maxAngularVelocity = 0;
            rigidBody.mass = 100000;
			cube.AddComponent<FileBoxAttribute>();
			var fba = cube.GetComponent<FileBoxAttribute>();
			fba.location = libraryFile[i];
			cube.transform.position = new Vector3((15 * i*2) - 100, 100, 100);
            float newScale = 20f;
            cube.transform.localScale = new Vector3(newScale, newScale, newScale);

		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
