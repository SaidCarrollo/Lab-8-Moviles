//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "GameEvent", menuName = "Game Events/Game Event")]
//public class GameEvent : ScriptableObject
//{
//    private List<GameEventListener> listeners = new List<GameEventListener>();

//    public void Raise()
//    {
//        for (int i = listeners.Count - 1; i >= 0; i--)
//        {
//            listeners[i].OnEventRaised();
//        }
//    }

//    public void RegisterListener(GameEventListener listener)
//    {
//        if (!listeners.Contains(listener))
//            listeners.Add(listener);
//    }

//    public void UnregisterListener(GameEventListener listener)
//    {
//        if (listeners.Contains(listener))
//            listeners.Remove(listener);
//    }
//}

//[CreateAssetMenu(fileName = "GameEvent<T>", menuName = "Game Events/Game Event Generic")]
//public class GameEvent<T> : ScriptableObject
//{
//    private List<GameEventListener<T>> listeners = new List<GameEventListener<T>>();

//    public void Raise(T parameter)
//    {
//        for (int i = listeners.Count - 1; i >= 0; i--)
//        {
//            listeners[i].OnEventRaised(parameter);
//        }
//    }

//    public void RegisterListener(GameEventListener<T> listener)
//    {
//        if (!listeners.Contains(listener))
//            listeners.Add(listener);
//    }

//    public void UnregisterListener(GameEventListener<T> listener)
//    {
//        if (listeners.Contains(listener))
//            listeners.Remove(listener);
//    }
//}
