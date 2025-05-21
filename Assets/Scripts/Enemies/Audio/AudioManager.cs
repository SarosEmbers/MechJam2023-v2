using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public Sound nowIntro, nowLooping, nowPlaying, nowChill, nowTran;
    public List<Sound> soundLibraryComplete;
    public Sound[] sounds; //Depopulated
    public Sound onPause;

    Sound loopingSound;

    private static float soundTimer = 0f;

    public string pausedSongLoop, pausedSongIntro;
    public float pausedTime;
    public int pausedTimeSamples;
    public bool inLoop = false;
    [System.Serializable]
    public class NPCTalkSounds 
    {
        public List<Sound> sounds;
        public List<string> matchNames;
        public NPCTalkSounds(List<Sound> sounds, List<string> matchNames)
        {
            this.sounds = sounds;
            this.matchNames = matchNames;
        }
    }
    public List<NPCTalkSounds> npcSounds;
    [System.Serializable]
    public class soundType
    {
        public string soundTypeName;
        public Sound[] sound;
        public soundType(string soundTypeName, Sound[] sound)
        {
            this.soundTypeName = soundTypeName;
            this.sound = sound;
        }
    }
    public List<soundType> soundCategories;
    public static AudioManager _Instance;
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        } 
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i <soundCategories.Count; i++)
        {
            foreach(Sound s in soundCategories[i].sound)
            {
                soundLibraryComplete.Add(s);
            }
        }

        sounds = soundLibraryComplete.ToArray();
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.outputAudioMixerGroup = s.mixerGroup;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = 1;
            s.source.rolloffMode = AudioRolloffMode.Linear;
            s.source.dopplerLevel = 0;
        }
    }
    private void OnDisable()
    {

    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "landingScene")
        {
            PlaySong("Title");
        }
    }
    public void Update()
    {
        if(soundTimer > 0)
        {
            soundTimer -= Time.fixedDeltaTime;
            Debug.Log("SOUND TIMER: " + soundTimer);
        }


    /*
        if (Input.GetKeyDown(KeyCode.N))
        {
            AudioManager._Instance.PlaySong("Boss 1");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager._Instance.pauseSong(true);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            AudioManager._Instance.ResumeSong("Boss 1");
        }
    */
    }
    public void Play(string name)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Play();
        }
        catch
        {
            Debug.Log("Sound: '" + name + "' not found!");
        }
    }
    public void PlayRand(string name, int howMany) //goal is to deprecate this by changing Sound "clip" into "clips[]" and have other Play() functions all randomize inherently
    {
        float f = UnityEngine.Random.Range(0.0f, howMany);
        int soundToPlay = Convert.ToInt32(f);
        Debug.Log(name + " " + soundToPlay);

        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name + soundToPlay);
            //s.pitch = UnityEngine.Random.Range(0.1f, 3f);
            s.source.Play();
        }
        catch
        {
            Debug.Log("Sound: '" + name + "' not found!");
        }
    }
    //AudioManager._Instance.Play("[YOUR SOUND HERE]");
    //AudioManager._Instance.PlayRandPitch("[YOUR SOUND HERE]");
    public void PlayRandPitch(string name, float minRand, float maxRand)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.pitch = UnityEngine.Random.Range(0.85f, 1.15f);
            s.source.spatialBlend = 1.0f;
            s.source.spread = 0;
            s.source.maxDistance = 30.0f;
            s.source.minDistance = 30.0f;
            s.source.rolloffMode = AudioRolloffMode.Linear;
            //Debug.Log("SOUND: " + s.source.pitch);

            s.source.Play();
        }
        catch
        {
            Debug.Log("Sound: '" + name + "' not found!");
        }
    }
    public void PlayAtSelectPoint(string name, Transform playPoint)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.pitch = UnityEngine.Random.Range(0.85f, 1.15f);
            s.source.maxDistance = 60.0f;
            s.source.minDistance = 30.0f;
            s.source.rolloffMode = AudioRolloffMode.Linear;
            //s.source.Play();
            AudioSource.PlayClipAtPoint(s.clip, playPoint.position, s.volume);
        }
        catch
        {
            Debug.Log("Sound: '" + name + "' not found!");
        }
    }
    public void PlayLooping(string name, bool startLoop)
    {
        if (startLoop)
        {
            try
            {
                Sound s = Array.Find(sounds, sound => sound.name == name);

                s.source.Play();

                loopingSound = s;
            }
            catch
            {
                Debug.Log("Sound: '" + name + "' not found!");
            }
        }
        else if (!startLoop && loopingSound != null)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (loopingSound.name == s.name)
            {
                loopingSound.source.Stop();
            }
        }
    }

    public void PlaySong(string name)
    {
        if (nowIntro.source != null)
        {
            nowIntro.source.Stop();
        }
        if (nowLooping.source != null)
        {
            nowLooping.source.Stop();
        }
        if (nowPlaying.source != null)
        {
            nowPlaying.source.Stop();
        }

        try
        {
            Sound i = Array.Find(sounds, sound => sound.name == name);
            Sound l = Array.Find(sounds, sound => sound.name == name + " LOOP");
            //Sound c = Array.Find(sounds, sound => sound.name == name + " CHILL");
            //Sound t = Array.Find(sounds, sound => sound.name == name + " TRAN");

            nowIntro = i;

            nowPlaying = nowIntro;

            i.source.volume = 1;
            i.source.spatialBlend = 0;
            //double introStart = AudioSettings.dspTime + 0.5f;

            i.source.Play();

            //double introLength = introStart + i.clip.length;
            //double floatToDouble = i.clip.length;

            if (l.clip != null)
            {
                nowLooping = l;
                //l.source.PlayScheduled(AudioSettings.dspTime + i.clip.length + 1.5f);
                l.source.PlayScheduled(AudioSettings.dspTime + i.clip.length);
                //Debug.Log("SOUND: " + i.source.clip.samples + " || " + i.source.clip.length + " || " + i.clip.length);

                //l.source.PlayScheduled(AudioSettings.dspTime + i.clip.samples);
                //https://forum.unity.com/threads/audiosource-playscheduled-plays-too-early.387279/
            }
            if (l.source.isPlaying == true)
            {
                nowPlaying = nowLooping;
            }
            /*
            if (c.clip != null)
            {
                c.source.PlayScheduled(AudioSettings.dspTime + i.clip.length);
                c.source.volume = 0.0f;
            }
            if (t.clip != null)
            {
                t.source.PlayScheduled(AudioSettings.dspTime + i.clip.length);
                t.source.volume = 0.0f;
            }
            */
            //Debug.Log("MUSIC INTRO: " + i.clip.name);
            //Debug.Log("MUSIC LOOP: " + l.clip.name);
            //Debug.Log("MUSIC CHILL: " + c.clip.name);
            //Debug.Log("MUSIC TRAN: " + t.clip.name);
        }
        catch
        {
            Debug.Log("Sound: '" + name + "' not found!");
        }
    }
 
    public void pauseSong(bool fadeOut)
    {
        if (nowIntro.source.isPlaying == true)
        {
            pausedTime = nowIntro.source.time;
            pausedTimeSamples = nowIntro.source.timeSamples;

            pausedSongIntro = nowIntro.clip.name;

            if (fadeOut)
            {
                AudioManager._Instance.FadeInSong("S I L E N C E", 1.0f);
            }
            else
            {
                nowIntro.source.Stop();
            }

            inLoop = false;
        }
        else if (nowLooping.source.isPlaying == true)
        {
            pausedTime = nowLooping.source.time;
            pausedTimeSamples = nowLooping.source.timeSamples;

            pausedSongLoop = nowLooping.clip.name;
            if (fadeOut)
            {
                AudioManager._Instance.FadeInSong("S I L E N C E", 1.0f);
            }
            else
            {
                nowLooping.source.Stop();
            }
            inLoop = true;
        }
    }

    public void ResumeSong(string name)
    {
        if (nowIntro.source != null)
        {
            nowIntro.source.Stop();
        }
        if (nowLooping.source != null)
        {
            nowLooping.source.Stop();
        }
        try
        {
            if(!inLoop)
            {
                Sound i = Array.Find(sounds, sound => sound.name == name);
                Sound l = Array.Find(sounds, sound => sound.name == name + " LOOP");

                Debug.Log("RESUME: " + pausedSongIntro);

                i.source.volume = 1;

                i.source.time = pausedTime;

                i.source.Play();

                if (l.clip != null)
                {
                    nowIntro = i;

                    nowPlaying = nowIntro;

                    nowLooping = l;
                    l.source.PlayScheduled(AudioSettings.dspTime + i.clip.length - pausedTime);
                }
                if (l.source.isPlaying == true)
                {
                    nowPlaying = nowLooping;
                    inLoop = true;
                }
            }
            else if (inLoop)
            {
                Sound l = Array.Find(sounds, sound => sound.name == name + " LOOP");

                nowLooping = l;

                nowPlaying = nowLooping;

                l.source.volume = 1;

                l.source.time = pausedTime;

                l.source.Play();
            }
        }
        catch
        {
            Debug.Log("Sound: '" + name + "' not found!");
        }
    }

    /*
    public void resumeSong()
    {
        if (nowIntro.isPlaying == true)
        {
            nowIntro.Stop();

            if (pausedSongIntro != null)
            {
                pausedSongIntro.time = pausedTime;
                //pausedSongIntro.Play();

                AudioManager._Instance.PlaySong(pausedSongIntro.name);

                if (nowLooping != null)
                {
                    nowLooping.PlayScheduled(AudioSettings.dspTime + pausedSongIntro.clip.length - pausedTime);
                }
            }
        }
        else if (nowLooping.isPlaying == true)
        {
            nowLooping.Stop();

            if (pausedSongLoop != null)
            {
                pausedSongLoop.time = pausedTime;
                pausedSongLoop.Play();

                pausedSongLoop = nowLooping;
            }
        }

        Debug.Log("PAUSE: " + nowPlaying.clip.name);
        Debug.Log("PAUSE: " + pausedSongIntro.name);

    }
    */

    public void FadeInSong(string fadeInTrack, float timeToFade)
    {
        StartCoroutine(CrossfadeTracks(fadeInTrack, timeToFade));
    }

    private IEnumerator CrossfadeTracks(string fadeInTrack, float timeToFade)
    {
        Sound g = Array.Find(sounds, sound => sound.name == fadeInTrack);

        Debug.Log("FADE IN: " + g.name);

        float timeElapsed = 0;

        while(timeElapsed < timeToFade)
        {
            if (g.source != null)
            {
                g.source.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            }

            if (inLoop)
            {
                nowLooping.source.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            }
            else if (nowPlaying.clip != null)
            {
                nowPlaying.source.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            }
            else if (nowPlaying.clip == null)
            {
                timeElapsed += Time.deltaTime;
                yield return timeElapsed;
            }

            timeElapsed += Time.deltaTime;
            yield return timeElapsed;
        }
    }
    //            AudioManager._Instance.Play("[YOUR SOUND HERE]");
}
