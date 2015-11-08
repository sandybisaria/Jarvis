using UnityEngine;
using Windows.Kinect;

class HandPalmAttribute : MonoBehaviour
{

    string HandStateString = "";

    public void UpdateState(HandState handState)
    {
        switch (handState)
        {
            case HandState.Open:
                HandStateString = "open";
                break;
            case HandState.Closed:
                HandStateString = "closed";
                break;
            case HandState.Lasso:
                HandStateString = "lasso";
                break;
        }
    }

    public string GetHandState()
    {
        return HandStateString;
    }

    public Color GetStateColor()
    {
        switch (HandStateString)
        {
            case "open":
                return Color.yellow;
            case "closed":
                return Color.green;
            case "lasso":
                return Color.blue;
            default:
                return Color.white;
        }
    }

}