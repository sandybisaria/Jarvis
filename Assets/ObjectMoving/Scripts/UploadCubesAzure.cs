﻿using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class UploadCubesAzure : MonoBehaviour
{    
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Col :" + col);

        if (col.collider.gameObject != null)
        {

            var testobj = col.gameObject.GetComponent<FileBoxAttribute>();

            if (testobj != null)
            {
                Debug.Log("ColMMMMMMMMMMMMMMM:" + testobj.typeThingy);
                Debug.Log("COLLOCATTTTIOOOONNN:" + testobj.location);
                MessageReceiver.Log("File uploaded to Azure\n" + testobj.location);
                Destroy(col.gameObject);
            }
        }


    }




}
