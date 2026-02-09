using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace.Utility
{
    public class ImmortalSingletonMonobehaviour<T> : Monobehaviour_Singleton<T> where T: Monobehaviour_Singleton<T> 
    {

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                
                DontDestroyOnLoad(gameObject);
                
            }
        }

    }
}