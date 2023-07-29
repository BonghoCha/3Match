using UnityEngine;

public class Util : MonoBehaviour
{
    public static T GetSingleTon<T>(string name) where T : Component
    {
        // 중복된 인스턴스 있는지 확인
        var instance = FindObjectOfType<T>();
        if (instance == null)
        {
            instance = new GameObject().AddComponent<T>();
            instance.name = name;    
        }
        
        return instance;
    }
}
