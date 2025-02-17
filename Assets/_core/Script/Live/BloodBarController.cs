﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _core.Script.Enemy
{
    public class BloodBarController : MonoBehaviour
    {
        private Transform _playerTransform;

        private bool _isInVisualField = false;

        private Image _bloodBar;
        private Image _remainder;

        private void Start()
        {
            _playerTransform = Camera.main.transform;

            _bloodBar = transform.Find("BloodBar").GetComponent<Image>();
            _remainder = transform.Find("Remainder").GetComponent<Image>();
        }
        public void UpdateBloodBar(float percent)
        {
            Debug.Log("Percent: "+percent);
            //Update BloodBar 
            _bloodBar.fillAmount = percent;

            if (percent <= 0.6 && percent > 0.3)
            {
                //take blood bar color to yellow
                _bloodBar.color = Color.yellow;

                //convert color and make it more transparent
                _remainder.color = Color.red;
                _remainder.color = _remainder.color - new Color(0, 0, 0, 0.4f);
            }
            else if (percent <= 0.3)
            {
                //take blood bar color to red
                _bloodBar.color = Color.red;

                //convert color and make it more transparent
                _remainder.color = Color.red;
                _remainder.color = _remainder.color - new Color(0, 0, 0, 0.4f);
            }
            else
            {
                //convert color and make it more transparent
                _remainder.color = Color.red;
                _remainder.color = _remainder.color - new Color(0, 0, 0, 0.4f);
            }

            //Update Remainder smoothly
            // await UpdateRemainder(percent);
          DOTween.To(() => _remainder.fillAmount, x => _remainder.fillAmount = x, percent, 0.7f)
                .SetEase(Ease.Flash).onComplete+= () =>
          {
              if (_remainder.fillAmount==0)
              {
                  //this blood slider is to zero than disappear it 
                  gameObject.SetActive(false);
              }
          };
        }
        
        void OnBecameVisible()
        {
            _isInVisualField = true;
        }

        private void OnBecameInvisible()
        {
            _isInVisualField = false;
        }


        private void Update()
        {
            if (!_isInVisualField && !_playerTransform) return;

            transform.LookAt(new Vector3(_playerTransform.position.x, transform.position.y,
                _playerTransform.transform.position.z));
        }
    }
}