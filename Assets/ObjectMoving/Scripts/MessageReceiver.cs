using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageReceiver : MonoBehaviour {

	public static void Log(string message)
    {
        GameObject messageTextObj = GameObject.Find("MessageText");
        Text messageText = messageTextObj.GetComponent<Text>();
        messageText.text = message;
    }
}
