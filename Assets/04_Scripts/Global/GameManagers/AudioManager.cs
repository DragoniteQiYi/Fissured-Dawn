using Cysharp.Threading.Tasks;
using FissuredDawn.Data.Configs;
using FissuredDawn.Global.Interfaces.GameManagers;
using FissuredDawn.Shared.Constants;
using FissuredDawn.Shared.Enums;
using FissuredDawn.Toolkits;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;

/*
 *  设计规则：
 *  1.通用音效为核心资源，必须初始化
 *  2.音乐和特定音效与场景相关，按需读取
 *  3.作用域资源，用完即卸载，避免内存占用
 *  4.所有资源必须异步读取，同步加载
 */
namespace FissuredDawn.Global.GameManagers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [Header("组件引用")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private GameObject _soundPrefab;

        [Header("音频参数")]
        [SerializeField][Range(0, 1f)] private float _mainVolume;
        [SerializeField][Range(0, 1f)] private float _musicVolume;
        [SerializeField][Range(0, 1f)] private float _uiVolume;
        [SerializeField][Range(0, 1f)] private float _sfxVolume;
        [SerializeField][Range(0, 1f)] private float _ambientVolume;

        [Header("配置参数")]
        [SerializeField] private int _poolSize = 10;

        public event Action OnMusicLoaded;

        private Queue<AudioSource> _audioPool = new();
        private List<AudioSource> _activeAudioSources = new();
        private bool _audioSystemInitialized = false;

        [Inject] private readonly ISceneLoader _sceneLoader;

        /// <summary>
        /// 核心音频资源字典
        /// </summary>
        private Dictionary<string, AudioClip> _coreAudioResources = new();

        /// <summary>
        /// 核心音频配置字典
        /// </summary>
        private Dictionary<string, AudioConfig> _coreAudioConfigs = new();

        [Header("当前播放的背景音乐")]
        [SerializeField] private string _currentMusicKey;
        [SerializeField] private AudioClip _currentMusicResource;
        [SerializeField] private AudioConfig _currentMusicConfig;
        [SerializeField] private bool _isMusicPaused = true;

        private void Awake()
        {
            _musicSource = GetComponent<AudioSource>();
            _musicSource.playOnAwake = false;
            _musicSource.loop = true;

            UpdateVolumes();
        }

        private void Start()
        {
            // 预初始化音频系统
            InitializeAudioSystem();
            _sceneLoader.OnSceneLoaded += LoadSceneMusic;
            Debug.Log("[AudioManager]: 音频系统开始运行");
        }

        private void Update()
        {
            if (!_isMusicPaused
                && _currentMusicKey != null
                && _currentMusicConfig.Loop)
            {
                if (_musicSource.time >= _currentMusicResource.length)
                {
                    _musicSource.time = _currentMusicConfig.LoopStartTime;
                }
            }
        }

        public void Initialize()
        {
            UniTask.Void(async () =>
            {
                await LoadMainAudioResourcesAsync();
            });
            CreateAudioPool();
        }

        public void MuteAll(bool mute)
        {
            AudioListener.pause = mute;
            AudioListener.volume = mute ? 0 : 1;

            // 暂停所有活跃音效
            foreach (var audioSource in _activeAudioSources)
            {
                if (audioSource != null)
                {
                    if (mute) audioSource.Pause();
                    else audioSource.UnPause();
                }
            }

            if (mute) _musicSource.Pause();
            else if (!_isMusicPaused) _musicSource.UnPause();
        }

        public void PauseMusic()
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.Pause();
                _isMusicPaused = true;
            }
        }

        public void PlayMusic(string clipName, float volumeMultiplier = 1, bool loop = true)
        {
            if (_currentMusicResource == null)
            {
                Debug.LogWarning($"[AudioManager]: 音乐资源未加载: {clipName}");
                return;
            }
            if (_currentMusicKey.Equals(clipName))
            {
                return;
            }
            var config = _coreAudioConfigs[clipName];

            _currentMusicConfig = config;
            _currentMusicKey = clipName;
            _musicSource.clip = _currentMusicResource;
            _musicSource.loop = loop;
            _musicSource.volume = _mainVolume * _musicVolume * volumeMultiplier * config.BasicVolume;
            _musicSource.Play();
            _isMusicPaused = false;
        }

        public void PlaySound(string clipName, float volumeMultiplier = 1, float pitch = 1, 
            Transform parent = null)
        {
            if (!_audioSystemInitialized)
            {
                Debug.LogWarning("[AudioManager]: 音频系统未初始化，跳过播放");
                return;
            }

            if (_audioPool.Count == 0)
            {
                Debug.LogWarning("[AudioManager]: 对象池已耗尽，创建新的音效源");
                ExpandPool(5);
            }

            if (!_coreAudioResources.TryGetValue(clipName, out var clip))
            {
                Debug.LogWarning($"[AudioManager]: 音效资源未加载: {clipName}");
                return;
            }

            AudioSource audioSource = _audioPool.Dequeue();
            GameObject soundObj = audioSource.gameObject;

            // 设置父对象
            if (parent != null)
            {
                soundObj.transform.SetParent(parent);
                soundObj.transform.localPosition = Vector3.zero;
            }
            else
            {
                soundObj.transform.SetParent(transform);
                soundObj.transform.localPosition = Vector3.zero;
            }

            soundObj.SetActive(true);
            Debug.Log("[AudioManager]: 音频播放对象初始化");

            // 配置音频源
            audioSource.clip = clip;
            Debug.Log("[AudioManager]: 配置音频对象");
            audioSource.volume = _mainVolume * _sfxVolume * volumeMultiplier;
            audioSource.pitch = pitch;
            audioSource.spatialBlend = 0f; // 2D音效
            audioSource.Play();
            Debug.Log("[AudioManager]: 播放音频对象");

            // 添加到活跃列表
            _activeAudioSources.Add(audioSource);

            // 播放完后回收
            UniTask.Void(async () =>
            {
                await ReturnToPool(audioSource, clip.length);
            });
            Debug.Log("[AudioManager]: 音频对象播放完毕");
        }

        public async UniTask PreloadAudio(string clipName)
        {
            await LoadSingleAudioClipAsync(clipName, false);
        }

        public void ResumeMusic()
        {
            if (_isMusicPaused)
            {
                _musicSource.UnPause();
                _isMusicPaused = false;
            }
        }

        public void StopMusic()
        {
            _musicSource.Stop();
            _currentMusicKey = null;
            _isMusicPaused = false;
        }

        public void StopAllSounds()
        {
            foreach (var audioSource in _activeAudioSources.ToArray())
            {
                if (audioSource != null)
                {
                    audioSource.Stop();
                    audioSource.gameObject.SetActive(false);
                    audioSource.transform.SetParent(transform);
                    _audioPool.Enqueue(audioSource);
                }
            }
            _activeAudioSources.Clear();
        }

        private void InitializeAudioSystem()
        {
            // 创建一个无声的AudioSource来预热音频系统
            GameObject warmupObj = new GameObject("AudioWarmup");
            AudioSource warmupSource = warmupObj.AddComponent<AudioSource>();
            warmupSource.playOnAwake = false;
            warmupSource.volume = 0f;

            // 播放一个极短的无声片段
            AudioClip dummyClip = AudioClip.Create("dummy", 1, 1, 44100, false);
            warmupSource.clip = dummyClip;
            warmupSource.Play();

            Destroy(warmupObj, 0.1f);
            _audioSystemInitialized = true;
            Debug.Log("[AudioManager]: 音频系统预热完成");
        }

        private async UniTask LoadMainAudioResourcesAsync()
        {
            _coreAudioConfigs = await JsonHelper.LoadAsync<Dictionary<string, AudioConfig>>
                (ConfigKey.AUDIO_CONFIG);

            var tasks = new List<UniTask>();   

            foreach (var config in _coreAudioConfigs)
            {
                string key = config.Key;
                tasks.Add(LoadSingleAudioClipAsync(key, true));
            }

            // 并行加载所有音频资源
            await UniTask.WhenAll(tasks);

            Debug.Log($"[AudioManager]: 音频资源加载完成，共加载{_coreAudioResources.Count}个音频片段");
        }

        private AudioConfig GetAudioConfig(string audioKey) => _coreAudioConfigs[audioKey];

        private async UniTask LoadSingleAudioClipAsync(string key, bool isPersistent)
        {
            // 检查是否已加载
            if (_coreAudioResources.ContainsKey(key))
            {
                Debug.Log($"[AudioManager]: 音频资源已加载: {key}，无需重复加载");
                return;
            }
            // 如果当前资源是音乐类型且加载时机为初始化，跳过
            if (_coreAudioConfigs[key].GroupType == AudioGroupType.Music && isPersistent)
            {
                return;
            }

            try
            {
                var handle = Addressables.LoadAssetAsync<AudioClip>(key);
                await handle.ToUniTask();

                if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
                {
                    if (isPersistent)
                    {
                        _coreAudioResources[key] = handle.Result;
                    }
                    else
                    {
                        _currentMusicResource = handle.Result;
                    }
                    // 如果需要存储handle以便后续释放
                    // _handles[key] = handle;
                    Debug.Log($"[AudioManager]: 音频资源加载成功: {key}");
                }
                else
                {
                    Debug.LogError($"[AudioManager]: 音频资源加载失败: {key}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AudioManager]: 加载音频资源时发生异常 {key}: {e.Message}");
            }
        }

        private void LoadSceneMusic(SceneConfig sceneConfig)
        {
            if (sceneConfig == null || string.IsNullOrEmpty(sceneConfig.BGMId))
            {
                Debug.Log("[AudioManager]: 加载场景配置为空或无音乐资源ID");
                return;
            }
            if (_currentMusicKey != null)
            {
                if (_currentMusicKey.Equals(sceneConfig.BGMId))
                {
                    Debug.Log("[AudioManager]: 当前场景音乐与上一场景相同");
                    return;
                }
            }
            string musicKey = sceneConfig.BGMId;
            UniTask.Void(async () => 
            {
                await PreloadAudio(musicKey);
                Debug.Log("场景音乐读取成功");
                PlayMusic(musicKey);
            });    
        }

        #region 对象池相关

        /// <summary>
        /// 初始化音频对象池
        /// </summary>
        private void CreateAudioPool()
        {
            for (int i = 0; i < _poolSize; i++)
            {
                CreatePooledAudioSource();
            }
        }

        /// <summary>
        /// 扩展对象池
        /// </summary>
        /// <param name="count"></param>
        private void ExpandPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreatePooledAudioSource();
            }
            Debug.Log($"[AudioManager]: 对象池扩展了{count}个，当前总数: {_audioPool.Count + _activeAudioSources.Count}");
        }

        private void CreatePooledAudioSource()
        {
            GameObject soundObj = Instantiate(_soundPrefab, transform);
            soundObj.SetActive(false);
            soundObj.name = "PooledAudioSource";

            AudioSource audioSource = soundObj.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0f;

            _audioPool.Enqueue(audioSource);
        }

        private async UniTask ReturnToPool(AudioSource audioSource, float delay)
        {
            await UniTask.Delay((int)(delay * 1000));

            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.gameObject.SetActive(false);
                audioSource.transform.SetParent(transform);
                audioSource.transform.localPosition = Vector3.zero;

                _activeAudioSources.Remove(audioSource);
                _audioPool.Enqueue(audioSource);
            }
        }

        private void UpdateVolumes()
        {
            if (_musicSource != null)
            {
                _musicSource.volume = _mainVolume * _musicVolume;
            }
        }

        // 清理资源
        private void OnDestroy()
        {
            StopAllSounds();
            _coreAudioResources.Clear();
            _coreAudioConfigs.Clear();
        }

        #endregion
    }
}
