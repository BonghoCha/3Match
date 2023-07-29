using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonDefinition : MonoBehaviour
{
    [Serializable]
    public class Point
    {
        public int x { get; set; }
        public int y { get; set; }

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
    }
}
