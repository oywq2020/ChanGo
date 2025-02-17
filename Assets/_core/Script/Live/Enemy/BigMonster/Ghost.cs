﻿using _core.Script.Live;
using UnityEngine;

namespace _core.Script.Enemy.BigMonster
{
    public class Ghost : EnemyAI
    {
        public Transform attack1Transform;

        public GameObject attack2GB;
        
        void OnAttack1()
        {
            var overlapSphere = Physics.OverlapSphere(attack1Transform.position,0.5f);
            foreach (var collider1 in overlapSphere)
            {
                if (collider1.gameObject.CompareTag("Player"))
                {
                    Debug.Log("Cause damage1");
                    collider1.gameObject.GetComponent<IDamageable>().GetHit(attack1Damage);
                }
            }
        }

        void StartAttack2()
        {
            attack2GB.GetComponent<TriggerKit>().StartListening(gb =>
            {
                gb.GetComponent<IDamageable>().GetHit(attack2Damage);
            },"Player");
        }

        void EndAttack2()
        {
            attack2GB.GetComponent<TriggerKit>().StopListening();
        }

    }
}