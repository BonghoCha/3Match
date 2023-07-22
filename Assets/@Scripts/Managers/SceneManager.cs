using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private static SceneManager _instance = null;
    public static SceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Util.GetSingleTon<SceneManager>("@SceneManager");
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private float _timeout = 10f;
    
    public void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name, mode);
    }

    private Coroutine _coLoadSceneAsync = null;
    public void LoadSceneAsync(string prev, string next, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        if (_coLoadSceneAsync != null)
        {
            StopCoroutine(_coLoadSceneAsync);
        }
        _coLoadSceneAsync = StartCoroutine(LoadSceneAsync(prev, next));
    }

    private IEnumerator LoadSceneAsync(string prev, string next)
    {
        var nextOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(next);
        nextOperation.allowSceneActivation = false;

        var timeout = 0f;
        while (timeout < _timeout)
        {
            timeout += Time.deltaTime;

            Debug.Log($"{next} Loaded : {nextOperation.progress}...");
            if (nextOperation.progress >= 0.9f)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        
        // 타임아웃
        if (timeout >= _timeout)
        {
            Debug.LogWarning("LoadScene Timeout !!");
            yield break;
        }

        // 다음 씬 활성화
        nextOperation.allowSceneActivation = true;

        // 이전 씬 비활성화
         UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(prev);
        
        // TODO : 로드/언로드 로직 개선 필요
    }
}
