using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    private bool _isSoundOn = true;
    private bool _isMusicOn = true;

    [System.Serializable]
    public class Sound
    {
        public GameEnum.ESound tag;
        public GameObject prefab;
        public int size;
    }

    public List<Sound> sounds;
    private Dictionary<GameEnum.ESound, Queue<GameObject>> soundsDictionary = new Dictionary<GameEnum.ESound, Queue<GameObject>>();

    [System.Serializable]
    public class Music
    {
        public GameEnum.EMusic tag;
        public GameObject prefab;
        public int size = 1;
    }

    public List<Music> musics;
    private Dictionary<GameEnum.EMusic, Queue<GameObject>> musicsDictionary = new Dictionary<GameEnum.EMusic, Queue<GameObject>>();

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        InitializeSounds();
        InitializeMusics();
        LoadSettings();
        PlayMusicForCurrentScene();

        // Lắng nghe khi scene load xong
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeSounds()
    {
        foreach (var sound in sounds)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < sound.size; i++)
            {
                GameObject obj = Instantiate(sound.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            soundsDictionary.Add(sound.tag, objectPool);
        }
    }

    private void InitializeMusics()
    {
        foreach (var music in musics)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < music.size; i++)
            {
                GameObject obj = Instantiate(music.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            musicsDictionary.Add(music.tag, objectPool);
        }
    }

    public void PlaySound(GameEnum.ESound tag)
    {
        if (!_isSoundOn) return;

        if (!soundsDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Sound {tag} not found!");
            return;
        }

        var pool = soundsDictionary[tag];
        GameObject obj = GetFromPool(pool, tag.ToString());
        if (obj != null)
        {
            AudioSource audio = obj.GetComponent<AudioSource>();
            audio.Play();
            StartCoroutine(ReturnToPool(obj, audio.clip.length, pool));
        }
    }

    public void PlayMusic(GameEnum.EMusic tag)
    {
        if (!_isMusicOn) return;

        if (!musicsDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Music {tag} not found!");
            return;
        }

        var pool = musicsDictionary[tag];
        GameObject obj = GetFromPool(pool, tag.ToString());
        if (obj != null)
        {
            AudioSource audio = obj.GetComponent<AudioSource>();
            audio.loop = true;
            audio.Play();
        }
    }

    public void StopAllMusic()
    {
        foreach (var queue in musicsDictionary.Values)
        {
            foreach (var obj in queue)
            {
                if (obj.activeSelf)
                {
                    AudioSource audio = obj.GetComponent<AudioSource>();
                    audio.Stop();
                    obj.SetActive(false);
                }
            }
        }
    }

    private GameObject GetFromPool(Queue<GameObject> pool, string name)
    {
        foreach (var obj in pool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        Debug.LogWarning($"Pool for {name} is empty, creating new...");
        return null;
    }

    private IEnumerator ReturnToPool(GameObject obj, float delay, Queue<GameObject> pool)
    {
        yield return new WaitForSecondsRealtime(delay);
        obj.SetActive(false);
    }

    // Hàm bật/tắt Music
    public void ToggleMusic()
    {
        _isMusicOn = !_isMusicOn;
        PlayerPrefs.SetInt("MusicSetting", _isMusicOn ? 1 : 0);

        if (_isMusicOn)
        {
            PlayMusicForCurrentScene();
            Debug.Log("Music On");
        }
        else
        {
            StopAllMusic();
            Debug.Log("Music Off");
        }

        PlayerPrefs.Save();
    }

    // Hàm bật/tắt Sound
    public void ToggleSound()
    {
        _isSoundOn = !_isSoundOn;
        PlayerPrefs.SetInt("SoundSetting", _isSoundOn ? 1 : 0);

        if (_isSoundOn)
        {
            Debug.Log("Sound On");
        }
        else
        {
            Debug.Log("Sound Off");
        }

        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        _isMusicOn = PlayerPrefs.GetInt("MusicSetting", 1) == 1;
        _isSoundOn = PlayerPrefs.GetInt("SoundSetting", 1) == 1;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_isMusicOn)
        {
            StopAllMusic();
            PlayMusicForCurrentScene();
        }
    }

    private void PlayMusicForCurrentScene()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex == 0)
        {
            PlayMusic(GameEnum.EMusic.MenuBackground);
        }
        else if (buildIndex == 1)
        {
            PlayMusic(GameEnum.EMusic.GameplayBackground);
        }
    }
}
