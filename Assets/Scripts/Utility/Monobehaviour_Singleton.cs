using UnityEngine;

namespace DefaultNamespace.Utility
{
    public abstract class Monobehaviour_Singleton<T> : MonoBehaviour where T : Monobehaviour_Singleton<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                
                Debug.LogWarning($"There cannot be multiple instances of {typeof(T).Name}. Duplicate was destroyed.");
                Destroy(this.gameObject); // Destroy the duplicate
                return;
                
            }

            if (Instance == null)
            {
                Instance = this as T;
            }
            

        }


        protected virtual void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

    }
}