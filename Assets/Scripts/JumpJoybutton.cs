using UnityEngine;
using UnityEngine.EventSystems;

public class JumpJoybutton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    public bool Pressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;   
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;   
    }

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
