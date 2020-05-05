using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour,IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public int id;
    Transform parentToReturnTo = null;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
     //   Debug.Log(eventData.pointerDrag.transform.parent + "drop to " + gameObject.transform.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.localPosition = Vector3.zero;
       
    }

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponentInParent<InventorySlot>() == null)
        {
            id = 45;
        }
        else
        {
            id = GetComponentInParent<InventorySlot>().id;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
