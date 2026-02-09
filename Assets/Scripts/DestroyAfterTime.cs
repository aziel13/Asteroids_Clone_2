using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    
    [SerializeField] private float objectLifeTime = 1f;
    private void FixedUpdate()
    {
     
        objectLifeTime-= Time.deltaTime;
            
        if (objectLifeTime <= 0f)
        {
            Destroy(gameObject);  
        }

    } 

}
