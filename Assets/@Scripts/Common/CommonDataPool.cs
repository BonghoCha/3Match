using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonDataPool : MonoBehaviour
{
    private static CommonDataPool _instance = null;
    public static CommonDataPool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Util.GetSingleTon<CommonDataPool>("@CommonDataPool");
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }
}
