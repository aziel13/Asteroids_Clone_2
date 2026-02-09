using System;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {

        
        
        [SerializeField] private float bulletForce = 1000f;
        private Transform _shipTransform;

        public Transform ShipTransform
        {
            get => _shipTransform;
            set => _shipTransform = value;
        }


        private void Awake()
        {
              
              
        }
        
        private void FixedUpdate()
        {
            GetComponent<Rigidbody2D>().AddForce(  _shipTransform.up * (bulletForce * Time.deltaTime));
            
        } 
    }
}