

using UnityEngine;

namespace EventSystem
{
    public static class ParabolaEvents
    {
        public static readonly RequestMakeParabolaEvent RequestMakeParabolaEvent = new RequestMakeParabolaEvent();
        public static readonly ParabolaChangeEvent ParabolaChangeEvent = new ParabolaChangeEvent();
        public static readonly ShootParabolaEvent ShootParabolaEvent = new ShootParabolaEvent();
    }

    public class RequestMakeParabolaEvent : GameEvent
    {
        public Vector3 startPos;
        public Vector3 endPos;
        
        public RequestMakeParabolaEvent Initializer(Vector3 start, Vector3 end)
        {
            startPos = start;
            endPos = end;
            return this;
        }
    }

    public class ParabolaChangeEvent : GameEvent
    {
        public Vector3[] points;
        public bool canEnd;
        
        public ParabolaChangeEvent Initializer(Vector3[] arr, bool canEnd)
        {
            points = arr;
            this.canEnd = canEnd;
            return this;
        }
    }
    
    public class ShootParabolaEvent : GameEvent
    {
        public Vector3[] points;
        
        public ShootParabolaEvent Initializer(Vector3[] arr)
        {
            points = arr;
            return this;
        }
    }
}