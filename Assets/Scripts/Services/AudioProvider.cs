using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Services
{
    public class AudioProvider
    {
        private const int POOL_SIZE = 2;

        private readonly AudioSource musicSource;

        private readonly Pool pool;

        private readonly AudioSource sfxSourcePrefab;

        public AudioProvider (
            AudioSource musicSource,
            [Inject(Id = "SFX")] AudioSource sfxSourcePrefab,
            Pool pool,
            Transform effectsSourcePoolContainer)
        {
            this.musicSource = musicSource;
            this.sfxSourcePrefab = sfxSourcePrefab;
            this.pool = pool;

            pool.AddPool(sfxSourcePrefab, POOL_SIZE, effectsSourcePoolContainer);
        }

        public void PlayMusic (AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PauseMusic () => musicSource.Pause();

        public void ResumeMusic () => musicSource.Play();

        public void PlayOneShot (AudioClip clip, Vector3? position = null, float volume = 1f)
        {
            SetupAudioSource(position, out AudioSource source, volume);

            source.clip = clip;
            source.Play();
        }

        private void SetupAudioSource (Vector3? position, out AudioSource source, float volume = 1)
        {
            source = pool.GetObject<AudioSource>(sfxSourcePrefab);

            if (position != null)
            {
                source.transform.position = position.Value;
                source.spatialBlend = 1;
            }
            else
            {
                source.spatialBlend = 0;
            }

            source.volume = volume;
        }
    }
}