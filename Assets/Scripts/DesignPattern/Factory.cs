using System;
using UnityEngine;

namespace DesignPattern
{
    public class Factory<T> : MonoBehaviour
        where T : Factory<T>
    {
        static public T New()
        {
            var gameObject = new GameObject(typeof(T).ToString());
            var component = gameObject.AddComponent<T>();
            return component;
        }

        static public T New(string prefabPath)
        {
            var gameObject = Instantiate(Resources.Load(prefabPath)) as GameObject;
            if (gameObject == null)
                throw new NullReferenceException();

            var component = gameObject.GetComponent<T>();
            if (component == null)
                Destroy(gameObject);

            return component;
        }
    }
}