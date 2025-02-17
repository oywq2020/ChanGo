﻿using System;
using System.Collections.Generic;
using System.Linq;
using Script.Abstract;
using UnityEngine;

namespace Script.UI
{
    public enum UILayer
    {
        Common,
        Pop
    }

    public class UIkit : IUIkit
    {
        private IGameObjectPool _gameObjectPool;

        private Dictionary<string, GameObject> _openedUiPanel = new Dictionary<string, GameObject>();


        //configure resource loading method via this constructor method
        public UIkit(IGameObjectPool gameObjectPool)
        {
            _gameObjectPool = gameObjectPool;
        }

        public GameObject OpenPanel(string key, UILayer layer = UILayer.Common, bool canDuplicate = false)
        {
            GameObject gameObject;
            if (!canDuplicate)
            {
                if (_openedUiPanel.TryGetValue(key, out GameObject obj))
                {
                    var uiPanel = obj.GetComponent<IUIPanel>();
                    if (!uiPanel.isOnOpen)
                    {
                        //if this panel is not opened than init it
                        uiPanel.Init();
                    }

                    gameObject = obj;
                }
                else
                {
                    gameObject = _gameObjectPool.Dequeue(key, LayerAdaptor.GetTransform(layer));
                }
            }
            else
            {
                gameObject = _gameObjectPool.Dequeue(key, LayerAdaptor.GetTransform(layer));
            }

            if (!_openedUiPanel.ContainsKey(key))
            {
                _openedUiPanel.Add(key, gameObject);
            }

            return gameObject;
        }

        public void ClosePanel(GameObject obj)
        {
            _gameObjectPool.Enqueue(obj);
            //remove from opened panel list of panel

            if (_openedUiPanel.ContainsKey(obj.name))
            {
                _openedUiPanel.Remove(obj.name);
            }
        }

        public void ClosePanel(string key)
        {
            _gameObjectPool.Enqueue(_openedUiPanel[key]);

            //remove from opened panel list of panel
            if (_openedUiPanel.ContainsKey(key))
            {
                _openedUiPanel.Remove(key);
            }
        }


        //Release 
        public void Release()
        {
            _openedUiPanel.Clear();

            _gameObjectPool.ReleaseAll();
        }
    }
}