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

        public void PlayRobotClip(int index)
        {
            if (!_source || !RobotSounds[index])
                return;
            
            _source.Stop();
            _source.clip = RobotSounds[index];
            _source.Play();
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
