namespace FissuredDawn.Global.Interfaces.GameManagers
{
    public interface IAudioManager
    {
        /// <summary>
        /// “Ù¿÷“Ù¡ø
        /// </summary>
        float MusicVolume { get; set; }

        /// <summary>
        /// “Ù–ß“Ù¡ø
        /// </summary>
        float SoundVolume { get; set; }

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
        void PlaySound(string clipName, float volume = 1f, float pitch = 1f);

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
        void PreloadAudio(string clipName);
    }
}
