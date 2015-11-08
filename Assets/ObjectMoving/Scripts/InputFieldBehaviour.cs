using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputFieldBehaviour : MonoBehaviour {

    public InputField self;

	void OnTriggerEnter(Collider collider)
    {
        var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
        ExecuteEvents.Execute(self.gameObject, pointer, ExecuteEvents.pointerClickHandler);
    }
}
