using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    #region ### OnClick ###

    public void GoToGameScene()
    {
        SceneManager.Instance.LoadSceneAsync(GameString.IntroScene, GameString.GameScene);
    }
    
    #endregion
}
