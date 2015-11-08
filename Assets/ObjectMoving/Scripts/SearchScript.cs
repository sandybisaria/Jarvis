using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;

public class SearchScript : MonoBehaviour {

	private bool isTriggered = false;
    public InputField inputField;

    private string lastInput = "";

    //string[] libraryFile = new string[15];
    // Use this for initialization
    void Start () {
    //    int x = 0;
    //    DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());

    //    foreach (DirectoryInfo d in dir.GetDirectories())
    //    {
    //        Console.WriteLine("{0, -30}\t directory", d.Name);
    //        //			Debug.Log("directory !: "+ d.Name);

    //    }

    //    foreach (FileInfo f in dir.GetFiles())
    //    {

    //        //			Console.WriteLine("{0, -30}\t File", f.Name);
    //        Debug.Log("File : " + "at location " + x + f.Name);
    //        Debug.Log("XXXXXXXXFile Full detail : " + f);
    //        //			Debug.Log("STRINGIFY : "+ f.DirectoryName);
    //        x = x + 1;
    //        libraryFile[x] = f.FullName;

    //    }

    //    Debug.Log("Start of Array iteration !");
    //    for (int i = 0; i < libraryFile.Length; i++)
    //    {
    //        Debug.Log("Array info at" + i + " " + libraryFile[i]);
    //    }


    //    for (int i = 0; i < x; i++)
    //    {

    //        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //        cube.name = libraryFile[i];
    //        cube.AddComponent<Rigidbody>();
    //        cube.transform.position = new Vector3(i * 2, 0, 0);

    //    }
    }

	// When triggered
	void OnTriggerEnter(Collider other) {
		if (!isTriggered) {
			string input = inputField.text;
			if (input != "" && input != lastInput) {
				isTriggered = true;
                lastInput = input;
				BingAPIManager.executeSearch(input);
			} else {
                SearchResultsDisplayer.SetResultsText("No input!");
			}
		}
	}

    void OnTriggerExit(Collider other)
    {
        isTriggered = false;
    }
}
