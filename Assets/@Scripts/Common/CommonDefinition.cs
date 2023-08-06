using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonDefinition : MonoBehaviour
{
    [Serializable]
    public class Point
    {
        public int x;
        public int y;

        public Point() { }
        public Point(int x, int y)
        {
            SetInfo(x, y);
        }
        
        public void SetInfo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public static Point operator +(Point a) => a;
        public static Point operator -(Point a) => new Point(-a.x, -a.y);
        public static Point operator +(Point a, Point b) => new Point(a.x + b.x, a.y + b.y);
        public static Point operator -(Point a, Point b) => new Point(a.x - b.x, a.y - b.y);
    }
}
