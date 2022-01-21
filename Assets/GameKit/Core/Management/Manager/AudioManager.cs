using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameKit;
namespace GameKit
{
    public class AudioManager : SingletonBase<AudioManager>
    {
        private AudioSource globalMusicSource = null;
        public float musicVolume = 1;
        public float soundVolume = 1;
        private GameObject GlobalAudio = null;
        private List<AudioSource> soundList = new List<AudioSource>();

        public AudioManager()
        {
            MonoManager.instance.AddUpdateListener(Update);
        }

        private void Update()
        {
            for (int i = soundList.Count - 1; i >= 0; --i)
            {
                if (!soundList[i].isPlaying)
                {
                    this.StopSound(soundList[i]);
                }
            }
        }
        public void PlayMusic(string name)
        {
            if (globalMusicSource != null)
            {
                globalMusicSource.Play();
            }
            else
            {
                GameObject obj = new GameObject();
                obj.name = "GlobalMusic";
                globalMusicSource = obj.AddComponent<AudioSource>();
                ResourceManager.instance.LoadAsync<AudioClip>("Audio/GlobalMusicSource/" + name, (clip) =>
                {
                    globalMusicSource.clip = clip;
                    globalMusicSource.loop = true;
                    globalMusicSource.volume = musicVolume;
                    globalMusicSource.Play();
                });
            }
        }

        public void PlaySound(string name, UnityAction<AudioSource> callback)
        {
            if (globalMusicSource != null)
            {
                globalMusicSource.Play();
            }
            else
            {
                GlobalAudio = new GameObject();
                GlobalAudio.name = "Sound";
                ResourceManager.instance.LoadAsync<AudioClip>("Audio/Sound/" + name, (clip) =>
                {
                    AudioSource source = GlobalAudio.AddComponent<AudioSource>();
                    source.clip = clip;
                    source.volume = soundVolume;
                    source.Play();
                    soundList.Add(source);
                    if (callback != null)
                        callback(source);
                });
            }

        }

        public void ChangeMusicVolume(float v)
        {
            musicVolume = v;
            if (globalMusicSource == null)
                return;
            globalMusicSource.volume = musicVolume;
        }

        public void ChangeSoundVolume(float v)
        {
            soundVolume = v;
            for (int i = 0; i < soundList.Count; ++i)
                soundList[i].volume = v;
        }

        public void ChangeMasterVolume(float v)
        {
            ChangeSoundVolume(v);
            ChangeMusicVolume(v);
        }
        public void PauseGlobalMusic()
        {
            if (globalMusicSource == null)
                return;
            globalMusicSource.Pause();
        }
        public void StopGlobalMusic()
        {
            if (globalMusicSource == null)
                return;
            globalMusicSource.Stop();
        }

        public void StopSound(AudioSource source)
        {
            if (soundList.Contains(source))
            {
                soundList.Remove(source);
                source.Stop();
                GameObject.Destroy(source);
            }
        }

        public void RegisterSound(AudioSource source)
        {
            if (!soundList.Contains(source))
                soundList.Add(source);
        }

        public void RegisterMusic(AudioSource source)
        {
            if (globalMusicSource != null && !globalMusicSource.isPlaying)
                globalMusicSource = source;
        }

        public List<AudioSource> GetSoundList()
        {
            return soundList;
        }
    }

}
