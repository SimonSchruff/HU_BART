using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Serializable]
        public enum BalloonType {
            standard,
            oneMore
        }

        [Serializable]
        public struct BalloonObject
        {
            public int MaxPumps;
            public BalloonType Type;
        }

        [Header("Balloons")] 
        public List<BalloonObject> Balloons = new List<BalloonObject>();

        [Header("Settings")]
        public float AmountEarnedPerPump = 0.25f;
        public float BalloonScaleIncrement = 0.1f;


        [Header("UI References")]
        public List<GameObject> Screens = new List<GameObject>(3);
        [Serializable] public struct inGameHUD {
            public RectTransform BalloonSprite;
            public TextMeshProUGUI BalloonNumberText;
            public TextMeshProUGUI CurrentEarnedText;
            public TextMeshProUGUI NumberOfPumpsText;
            public TextMeshProUGUI TotalEarnedText;
            public GameObject OneMoreText;
            public ParticleSystem CoinParticleSystem;
            public ParticleSystem BalloonExplodeParticleSystem;

        }
        public inGameHUD InGameHUD = new inGameHUD();

        // Private Vars
        private bool _didCashIn;
        private bool _firstCashIn = true;
        private bool _didSecondCashIn;
        private int _balloonAmount;
        private int _currentBalloon;
        private int _currentNumberOfPumps; 
        private float _currentEarned;
        private float _totalEarned;
        private float _initialBalloonScaleValue;

        private bool _isLastTrial;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);

            if (!InGameHUD.BalloonSprite || !InGameHUD.BalloonNumberText || !InGameHUD.CurrentEarnedText || !InGameHUD.TotalEarnedText || !InGameHUD.NumberOfPumpsText || !InGameHUD.CoinParticleSystem || !InGameHUD.BalloonExplodeParticleSystem ) {
                Debug.LogError("References Missing in  GameManager!");
                return;
            }
        }

        private void Start()
        {
            _initialBalloonScaleValue = InGameHUD.BalloonSprite.localScale.x;
            _balloonAmount = Balloons.Count;
            UpdateUIText();
        }

        private void ContinueToNextBalloon()
        {
            SaveTrial();
            ClearCurrentData();

            _currentBalloon++;
        
            UpdateUIText();
            ResetUIBalloon();
        
            if (_currentBalloon >= _balloonAmount)
            {
                // Last trial
                FragebogenManager.instance.NextQuestion();
            }
        }
    
    
        // Save the data for one trial and add it to save data list; 
        private void SaveTrial()
        {
            SaveManager.BalloonData data = new SaveManager.BalloonData();
            data.balloonNumber = _currentBalloon;
            data.balloonType = Balloons[_currentBalloon].Type;
            data.numberOfPumps = _currentNumberOfPumps;
            data.didCashIn = _didCashIn;
            data.didSecondCashIn = _didSecondCashIn;
            data.earned = _currentEarned;
            data.totalEarned = _totalEarned;

            SaveManager.instance.SaveBalloonData(data);
        }
    
        // Delete old trial data
        private void ClearCurrentData()
        {
            _didCashIn = false;
            _didSecondCashIn = false;
            _currentEarned = 0.0f;
            _currentNumberOfPumps = 0;
        }

        private void UpdateUIText()
        {
            InGameHUD.BalloonNumberText.text = $"Ballon Nummer: {_currentBalloon + 1}/{_balloonAmount}";
            InGameHUD.CurrentEarnedText.text = String.Format("Momentan verdient:  {0:0.00}", _currentEarned);
            InGameHUD.NumberOfPumpsText.text = String.Format("Aufblas Anzahl: {0:0}", _currentNumberOfPumps);
            InGameHUD.TotalEarnedText.text = String.Format("Gesamt verdient: {0:0.00}", _totalEarned);
        
            if(InGameHUD.OneMoreText.activeInHierarchy)
                InGameHUD.OneMoreText.SetActive(false);
        }

        private void ScaleUIBalloon()
        {
            Vector3 newScale = new Vector3(BalloonScaleIncrement, BalloonScaleIncrement, BalloonScaleIncrement);
            InGameHUD.BalloonSprite.localScale += newScale;
        }
    
        private void ResetUIBalloon()
        {
            Vector3 initialScale = new Vector3(_initialBalloonScaleValue, _initialBalloonScaleValue, _initialBalloonScaleValue);
            InGameHUD.BalloonSprite.localScale = initialScale;
        }

        private void PlayCoinFXAtMousePos()
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = new Vector3(worldPos.x, worldPos.y, -1f); // Z-Value does not get correctly set-up in game; 

            InGameHUD.CoinParticleSystem.transform.position = targetPos;
            InGameHUD.CoinParticleSystem.Play();
        }

        #region BUTTON_EVENTS
        public void InflateBalloon()
        {
            _currentNumberOfPumps++;
            
            _currentEarned += AmountEarnedPerPump;
            _totalEarned += AmountEarnedPerPump;
            
            UpdateUIText();
            ScaleUIBalloon();

            // ONE MORE GAME-MODE: 
            // - After first cash- in, next inflate balloon click will automatically trigger cash in
            if (!_firstCashIn)
            {
                CashIn();
                return;
            }

            // Check if MaxPumps of current balloon has been reached
            if (_currentNumberOfPumps >= Balloons[_currentBalloon].MaxPumps)
            {
                // Balloon explodes
                
                // TODO: Particle System Sprites normals are sometimes flipped -> Not all particles visible
                InGameHUD.BalloonExplodeParticleSystem.Play();
                
                _didCashIn = false;
                _totalEarned -= _currentEarned;
                _currentEarned = 0.0f;
                
                //SoundManager.instance.PlayClip(SoundManager.sound.pop);
                ContinueToNextBalloon();
                return;
            }

            //SoundManager.instance.PlayClip(SoundManager.sound.inflate);
        }
        
        public void CashIn()
        {
            switch (Balloons[_currentBalloon].Type)
            {
                case BalloonType.standard:
                    _didCashIn = true;
                    PlayCoinFXAtMousePos();
                    ContinueToNextBalloon();
                    break;
                case BalloonType.oneMore:
                    
                    if (_firstCashIn)
                    {
                        // Play Robot Sound: ONE MORE!
                        InGameHUD.OneMoreText.SetActive(true);
                        SoundManager.instance.PlayRobotClip(UnityEngine.Random.Range(0,2));
                        
                        // Ensure Balloon has enough pumps left
                        var balloon = Balloons[_currentBalloon];
                        balloon.MaxPumps = _currentNumberOfPumps + 2;
                        Balloons[_currentBalloon] = balloon;
                        
                        _firstCashIn = false;
                    }
                    else
                    {
                        // Save Vars
                        _didCashIn = true;
                        _didSecondCashIn = true;
                        
                        // Game Relevant
                        _firstCashIn = true;
                        
                        InGameHUD.OneMoreText.SetActive(false);
                        
                        PlayCoinFXAtMousePos();
                        ContinueToNextBalloon();
                    }
                    break;
            }
        }
        #endregion
    }
}
