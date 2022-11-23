﻿using System;
using System.Collections.Generic;
using PlayerRegion;
using Script.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _core.Script.Bag
{
    public class ItemOnBag:MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        private Transform _originalParent;
        private List<AbstractItemScrObj> _bagItemList; 

        public void OnBeginDrag(PointerEventData eventData)
        {
            //preserve original parent
            _originalParent = transform.parent;
            
            transform.SetParent(transform.parent.parent.parent);
            transform.position = eventData.position;
            
            //cancel its ray block
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            
            //get current inventory for this bag
             _bagItemList = CurrentPlayer.Instance._bag.itemList;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject&&eventData.pointerCurrentRaycast.gameObject.CompareTag("Slot"))
            {
                //if the end drag position is on slot
                if (eventData.pointerCurrentRaycast.gameObject.transform.childCount==0)
                {
                    //this slot is null
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                    
                    //exchange this inventory list
                    var index = eventData.pointerCurrentRaycast.gameObject.GetComponent<Index>().GetIndex();
                   _bagItemList[index] = _bagItemList[_originalParent.GetComponent<Index>().GetIndex()];
                   _bagItemList[_originalParent.GetComponent<Index>().GetIndex()] = null;

                }
              
            }
            else if (eventData.pointerCurrentRaycast.gameObject&&eventData.pointerCurrentRaycast.gameObject.CompareTag("Item"))
            {
                //get index of current slot
                var index = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<Index>()
                    .GetIndex();
                    
                if ( _bagItemList[index].itemName ==_bagItemList[_originalParent.GetComponent<Index>().GetIndex()].itemName)
                {
                    var temp = _bagItemList[_originalParent.GetComponent<Index>().GetIndex()];
                    _bagItemList[index].count += temp.count;
                    _bagItemList[_originalParent.GetComponent<Index>().GetIndex()] = null;
                    //destroy self
                    Destroy(gameObject);
                }
                else
                {
                    //exchange both position
                    eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).SetParent(_originalParent);
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                    
                    //exchange this inventory list
                    var temp = _bagItemList[index];
                    _bagItemList[index] = _bagItemList[_originalParent.GetComponent<Index>().GetIndex()];
                    _bagItemList[_originalParent.GetComponent<Index>().GetIndex()] = temp;
                }
            }
            else
            {
                transform.SetParent(_originalParent);
            }
            
            //reset its ray block
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            
            //refresh bag
            GameFacade.Instance.SendEvent<OnPlayerBagRefresh>();
        }
    }
}