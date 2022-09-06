using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ScrollViewUtillity : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector3 direction;
    public float speed;
    private bool isTouch = false;

    public Transform camera;

    public void OnDrag(PointerEventData eventData)
    {
        print(eventData.delta);
        if(eventData.delta.x < 0)
        {
            direction = Vector3.right;
        }
        else
        {
            direction = Vector3.left;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isTouch)
        {
            camera.position += direction * speed * Time.deltaTime;
        }
    }
}
