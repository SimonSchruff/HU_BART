using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioClip[] robotRateSoundWinGr1;
        [SerializeField] AudioClip[] robotRateSoundWinGr2;
        [SerializeField] AudioClip[] robotRateSoundWinGr3;
        [SerializeField] AudioClip[] robotRateSoundLossGr1;
        [SerializeField] AudioClip[] robotRateSoundLossGr2;
        [SerializeField] AudioClip[] robotRateSoundLossGr3;
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
        private int _groupID;
    
        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);

            _source = GetComponent<AudioSource>(); 
            _groupID = SaveManager.instance.GroupID;

        }
        
        /// <summary>
        /// Play a clip from the robot sound array;
        /// </summary>
        /// <param name="index">Index of clip in RobotSounds[]</param>
        /// <returns>Length of played clip in seconds; If source or clip is null, method returns 0;</returns>
        public float PlayRandomRobotClip()
        {
            _source.Stop();

            switch (SaveManager.instance.actualGroup)
            {
                case SaveManager.GroupInfo.Group1:
                    _source.clip = RobotSounds[0];
                    break;
                case SaveManager.GroupInfo.Group2:
                    _source.clip = RobotSounds[1];
                    break;
                case SaveManager.GroupInfo.Group3:
                    _source.clip = RobotSounds[2];
                    break;

            }
            _source.Play();

            return _source.clip.length;
        }

        public void PlaySoundByGroup(int groupID)
        {
            if (!_source)
                return;
        
            _source.Stop();

            switch (_groupID)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                
            }
            
            _source.Play();
        }

        public void PlayRobotRateSound(bool win)
        {
            switch (SaveManager.instance.actualGroup)
            {
                case SaveManager.GroupInfo.Group1:
                    _source.clip = win ? getRandomElem(robotRateSoundWinGr1, true) : getRandomElem(robotRateSoundLossGr1, false);
                    break;
                case SaveManager.GroupInfo.Group2:
                    _source.clip = win ? getRandomElem(robotRateSoundWinGr2, true) : getRandomElem(robotRateSoundLossGr2, false);
                    break;
                case SaveManager.GroupInfo.Group3:
                    _source.clip = win ? getRandomElem(robotRateSoundWinGr3, true) : getRandomElem(robotRateSoundLossGr3, false);
                    break;
            }
            _source.Play();
        }
        int _tempLost = 0;
        int _tempWon = 0;
        
        AudioClip getRandomElem (AudioClip [] inputArray, bool _lost)
        {
            if (_lost)
                _tempLost++;
            else
                _tempWon++;

            return inputArray[((_lost?_tempLost:_tempWon) % inputArray.Length)];
        }
    }
}
