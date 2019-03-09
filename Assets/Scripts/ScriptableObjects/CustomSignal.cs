//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu]
//public class CustomSignal : ScriptableObject
//{
//    public List<SignalListener> Listeners = new List<SignalListener>();

//    public void Raise(float parameter)
//    {
//        for (int i = Listeners.Count - 1; i >= 0; i--)
//        {
//            Listeners[i].OnSignalRaised(parameter);
//        }
//    }

//    public void RegisterListener(SignalListener listener)
//    {
//        if (!Listeners.Contains(listener))
//            Listeners.Add(listener);
//    }

//    public void DeregisterListener(SignalListener listener)
//    {
//        if (Listeners.Contains(listener))
//            Listeners.Remove(listener);
//    }
//}
