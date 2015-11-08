using System;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class SearchResultsDisplayer : MonoBehaviour {

    public static void ParseResults(JObject results) {
        JObject contents = (JObject)results["d"]["results"][0];
        JArray webContents = (JArray)contents["Web"];

        string resultString = "";

        foreach (JObject result in webContents) {
            string resultTitle = (string)result["Title"];
            string resultUrl = (string)result["Url"];
            resultString += "<b>" + resultTitle + "</b> " + resultUrl + "\n";
        }
        SetResultsText(resultString);
    }

    public static void SetResultsText(string text) {

        GameObject resultsTextObj = GameObject.Find("ResultsText");
        Text resultsText = resultsTextObj.GetComponent<Text>();
        resultsText.text = text;
    }
}
