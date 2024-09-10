using UnityEngine;

namespace Thanabardi.Generic.Utility
{
    public abstract class MonoSingleton : MonoBehaviour
    {
        public abstract void Destroy();
    }

    public class MonoSingleton<T> : MonoSingleton where T : MonoBehaviour
    {
        private static bool _isDestroyed = false;
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                // check is the instance intentionally destroyed
                if (_isDestroyed)
                {
                    return null;
                }

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogWarning($"has duplicated MonoSingleton at type {typeof(T)}");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject go = new();
                        _instance = go.AddComponent<T>();
                        go.name = _instance.GetType().Name;
                    }
                }

                return _instance;
            }

            private set { _instance = value; }
        }

        public virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this);
            _isDestroyed = false;
        }

        public virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _isDestroyed = true;
            }
        }

        public override void Destroy()
        {
            _instance = null;
            Destroy(gameObject);
        }
    }
}