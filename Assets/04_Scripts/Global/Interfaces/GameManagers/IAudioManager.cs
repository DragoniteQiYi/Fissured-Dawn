using Cysharp.Threading.Tasks;
using FissuredDawn.Data.Configs;
using System;
using UnityEngine;

namespace FissuredDawn.Global.Interfaces.GameManagers
{
    public interface IAudioManager
    {
        event Action OnMusicLoaded;

        /// <summary>
        /// ≥ı ºªØ
        /// </summary>
        void Initialize();

        /// <summary>
        /// ≤•∑≈“Ù¿÷
        /// </summary>
        /// <param name="clipName">“Ù∆µ∆¨∂Œ√˚≥∆</param>
        /// <param name="volume">“Ù¡ø (0-1)</param>
        /// <param name="loop"> «∑Ò—≠ª∑</param>
        void PlayMusic(string clipName, float volume = 1f, bool loop = true);

        /// <summary>
        /// ≤•∑≈“Ù–ß
        /// </summary>
        /// <param name="clipName">“Ù∆µ∆¨∂Œ√˚≥∆</param>
        /// <param name="volume">“Ù¡ø (0-1)</param>
        /// <param name="pitch">“Ùµ˜</param>
        void PlaySound(string clipName, float volumeMultiplier = 1, float pitch = 1,
            Transform parent = null);

        /// <summary>
        /// Õ£÷π±≥æ∞“Ù¿÷
        /// </summary>
        void StopMusic();

        /// <summary>
        /// ‘›Õ£±≥æ∞“Ù¿÷
        /// </summary>
        void PauseMusic();

        /// <summary>
        /// ª÷∏¥±≥æ∞“Ù¿÷
        /// </summary>
        void ResumeMusic();

        /// <summary>
        /// æ≤“ÙÀ˘”–“Ù∆µ
        /// </summary>
        /// <param name="mute"> «∑Òæ≤“Ù</param>
        void MuteAll(bool mute);

        /// <summary>
        /// ‘§º”‘ÿ“Ù∆µ◊ ‘¥
        /// </summary>
        /// <param name="clipName">“Ù∆µ∆¨∂Œ√˚≥∆</param>
        UniTask PreloadAudio(string clipName);

        /// <summary>
        /// Õ£÷πÀ˘”–“Ù∆µ≤•∑≈
        /// </summary>
        void StopAllSounds();
    }
}
