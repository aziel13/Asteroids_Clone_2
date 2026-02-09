using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Wrap : MonoBehaviour
    {
        
        private Camera gameCamera;
        
        private Vector3 viewBottomLeft => gameCamera.ViewportToWorldPoint(Vector3.zero);
        private Vector3 viewTopRight => gameCamera.ViewportToWorldPoint(Vector3.one);
        
        private float yoffset = 1;
        private float xoffset = 1;


        private void Awake()
        {

            if (this.gameCamera == null)
            { 
                gameCamera = GameManager.Instance.mainCamera;
            }

        }


        private void Update()
        {
            Vector3 viewPortPosition = Camera.main.WorldToViewportPoint(transform.position);
            
            Vector3 moveAdjustment = Vector3.zero;

            if (viewPortPosition.x < 0)
            {
                moveAdjustment.x +=  yoffset;

            } else 
            
            if (viewPortPosition.x > 1)
            {
                moveAdjustment.x -=  xoffset;
                
            } else 

            if (viewPortPosition.y < 0)
            {
                moveAdjustment.y +=  yoffset;

            } else
            
            if (viewPortPosition.y > xoffset)
            {
                moveAdjustment.y -=  yoffset;
            }

            if ( viewPortPosition.x < 0 || viewPortPosition.x > 1 || viewPortPosition.y < 0 ||viewPortPosition.y > 1)
            {
                transform.position = Camera.main.ViewportToWorldPoint( viewPortPosition + moveAdjustment);
            }

           
            
        }
    }
}