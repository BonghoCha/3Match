using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region ## Class ##

    public class AudioClipEx
    {
        private ClipType clipType;
        private string clipName;
        private AudioClip clip;

        public AudioClipEx(ClipType clipType, string clipName, AudioClip clip)
        {
            this.clipType = clipType;
            this.clipName = clipName;
            this.clip = clip;
        }

        #region ## Getter ##

        public ClipType GetClipType()
        {
            return this.clipType;
        }

        public string GetName()
        {
            return this.clipName;
        }
        
        public AudioClip GetClip()
        {
            return this.clip;
        }

        #endregion
    }

    #endregion

    #region ## Enum ##

    public enum ClipType
    {
        BGM,
        Effect
    }

    #endregion

    private static SoundManager _instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Util.GetSingleTon<SoundManager>("@SoundManager");
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }
    
    [SerializeField] private AudioSource _BGM;
    [SerializeField] private List<AudioSource> _Effects;

    private Dictionary<string, AudioClipEx> _clipDic = new Dictionary<string, AudioClipEx>();

    public void Play(string clipName)
    {
        if (_clipDic.TryGetValue(clipName, out var clip))
        {
            var clipType = clip.GetClipType();
            switch (clipType)
            {
                case ClipType.BGM:
                {
                    Debug.Log(clip);
                    Play(_BGM, clip.GetClip());
                    break;
                }
                case ClipType.Effect:
                {
                    var isPlayed = false;
                    foreach (var effect in _Effects)
                    {
                        if (effect.isPlaying)
                        {
                            Play(effect, clip.GetClip());

                            isPlayed = true;
                            break;
                        }
                    }

                    if (!isPlayed)
                    {
                        Play(_Effects[0], clip.GetClip());
                    }

                    break;
                }
            }
        }
        else
        {
            // TODO : 리소스 매니저로 로드하는 방식 추가 필요
            var loadedClip = Resources.Load($"Sounds/{clipName}") as AudioClip;
            
            Debug.Log(loadedClip);
            if (loadedClip != null)
            {
                var clipType = ClipType.BGM;
                if (clipName.Contains("BGM"))
                {
                    clipType = ClipType.BGM;
                }
                else
                {
                    clipType = ClipType.Effect;
                }
                _clipDic.Add(clipName, new AudioClipEx(clipType, clipName, loadedClip));
                
                Play(clipName);
            }
            else
            {
                Debug.Log("[SoundManager] not found the clip !!");    
            }
        }
    }

    public void Play(AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}