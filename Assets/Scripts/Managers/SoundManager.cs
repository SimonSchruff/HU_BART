using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
    
    

        public enum sound {
            inflate,
            pop
        }



        [Header("Clips")] 
        public List<AudioClip> RobotSounds = new List<AudioClip>(); 
        public AudioClip InflateSound;
        public AudioClip PopSound;
        
        
        private AudioSource _source;
    
        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);

            _source = GetComponent<AudioSource>(); 
        }
        
        /// <summary>
        /// Play a clip from the robot sound array;
        /// </summary>
        /// <param name="index">Index of clip in RobotSounds[]</param>
        /// <returns>Length of played clip in seconds; If source or clip is null, method returns 0;</returns>
        public float PlayRobotClip(int index)
        {
            if (!_source || !RobotSounds[index])
                return 0.0f;
            
            _source.Stop();
            _source.clip = RobotSounds[index];
            _source.Play();

            return _source.clip.length;
        }

        public void PlayClip(sound soundToPlay)
        {
            if (!_source || !InflateSound || !PopSound)
                return;
        
            _source.Stop();

            switch (soundToPlay)
            {
                case sound.inflate:
                    _source.clip = InflateSound;
                    break;
                case sound.pop:
                    _source.clip = PopSound;
                    break;
            }
        
            _source.Play();
        }
    }
}
