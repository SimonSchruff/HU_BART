using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Linq;
using TMPro;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager instance;

        // Specifies where to save csv file; Editor saves in project folder and android on persistent path;
        public List<Dictionary<string, string>> allSaveData = new List<Dictionary<string, string>>();

        [Serializable] public enum SaveLocation
        {
            Editor,
            Android
        }
        [Serializable]
        public enum GroupInfo
        {
            Group1,
            Group2,
            Group3
        }
        public SaveLocation SelectedSaveLocation = SaveLocation.Editor;
        public GroupInfo actualGroup = GroupInfo.Group1;

        public string PlayerID = "test_id";
        public int GroupID = 1;
        public string URL = "https://marki.fun/PHP/dataFL.php";
        
        private string _fileLoc = ""; 
        private string _fileName = ""; 
        
        private System.DateTime _startTime;
        
        [Serializable] public struct BalloonData {
            public GameManager.BalloonType balloonType;
            public int balloonNumber;
            public float earned;
            public int numberOfPumps;
            public float totalEarned;
            public bool didCashIn;
            public bool didSecondCashIn;
        }
        private List<BalloonData> _balloonData = new List<BalloonData>();

        // Private Vars
        private int _balloonAmount; 

        
        void Awake() 
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
            
            DontDestroyOnLoad(gameObject);
        }

        public void checkIfVPContinue (int group = 1){
            GroupID = group;
            switch (group)
            {
                 case 1:
                    actualGroup = GroupInfo.Group1;
                    break;
                case 2:
                    actualGroup = GroupInfo.Group2;
                    break;
                case 3:
                    actualGroup = GroupInfo.Group3;
                    break;
            }

            Debug.Log(_startTime.ToString());
                    
                    
            string input = FindObjectOfType<TMP_InputField>().text; //Check if VP input is okay and go to next level
            PlayerID = input;
            Dictionary<string, string> _tempSave = new Dictionary<string, string>();
            _tempSave.Add("playerID", PlayerID);
            _tempSave.Add("group", actualGroup.ToString());
            _tempSave.Add("startTime", _startTime.ToString());

            allSaveData.Add(_tempSave);

            if (input.Length > 0){
                LevelManager.lm.nextLevel();
            }
        }

        private void Start() 
        {
            _startTime = System.DateTime.Now;

           // var newDir = Directory.CreateDirectory(Application.persistentDataPath + "/HU_BART_Data");
           switch (SelectedSaveLocation)
           {
               case SaveLocation.Editor:
                   _fileLoc = Application.dataPath + "/SaveData/";
                   break; 
               case SaveLocation.Android:
                   _fileLoc = Application.persistentDataPath + "/";
                   break;
           }

           string date = $"{System.DateTime.Now.Day}_{System.DateTime.Now.Month}";
           _fileName = $"/HU_BART_TestData_{date}.csv";
        }
        
        public void StartPostCoroutine()
        {
            StartCoroutine(PostData());
        }
        
        /// <summary>
        /// Add Balloon Data to save managers list for saving later;
        /// </summary>
        /// <param name="data">Struct with all relevant information for current balloon;</param>
        public void SaveBalloonData(BalloonData data)
        {
            _balloonData.Add(data);

            Dictionary<string, string> _tempDic = new Dictionary<string, string>();             // Add Dictionary to all Save from Balloon Task
            _tempDic.Add("ballonNum" + _balloonData.Count, data.balloonNumber.ToString());
            _tempDic.Add("ballonType" + _balloonData.Count, data.balloonType.ToString());
            _tempDic.Add("numOfPumps" + _balloonData.Count, data.numberOfPumps.ToString());
            _tempDic.Add("didCashIn" + _balloonData.Count, data.didCashIn.ToString());
            _tempDic.Add("didSecondCashIn" + _balloonData.Count, data.didSecondCashIn.ToString());
            _tempDic.Add("earned" + _balloonData.Count, data.earned.ToString());
            _tempDic.Add("totalEarned" + _balloonData.Count, data.totalEarned.ToString());

            allSaveData.Add(_tempDic);
        }
        
        /// <summary>
        /// Save id of test person, for later saving
        /// </summary>
        public void SaveID(string id)
        {
            PlayerID = id;
        }

        // Create Title Line for CSV File
        private string CreateCSVTitle()
        {
            string returnTitle = "";
            bool first = true;

            foreach (var dic in allSaveData)
            {
                foreach (var elem in dic)
                {
                    returnTitle += (first?"":",") + elem.Key;
                    first = false;
                }
            }

          /*  string[] balloonDataTitles = new string[_balloonAmount];
            
            for (int i = 0; i < _balloonAmount; i++) {
                balloonDataTitles[i] = $",{i}_Type,{i}_Earned,{i}_NumberOfPumps,{i}_TotalEarned,{i}_DidCashIn,{i}_DidSecondCashIn";
                returnTitle += balloonDataTitles[i]; 
            }
            */
            return returnTitle;
        }

        // Create Data Entry Line for CSV File
        private string CreateCSVLine()
        {
            string content = "";

            bool first = true;

            foreach (var dic in allSaveData)
            {
                foreach (var elem in dic)
                {
                    content += (first ? "" : ",") + elem.Value;
                    first = false;
                }
            }

            /*  string content = PlayerID + "_" + _startTime.Month +"_"+ _startTime.Day +"_"+ + _startTime.Hour +"_"+ _startTime.Minute + "," + GroupID;
              string[] contentData = new string[_balloonAmount];

              for (int i = 0; i < _balloonData.Count; i++) {
                  contentData[i] = $",{_balloonData[i].balloonType},{_balloonData[i].earned},{_balloonData[i].numberOfPumps},{_balloonData[i].totalEarned},{_balloonData[i].didCashIn},{_balloonData[i].didSecondCashIn}";
                  content += contentData[i]; 
              }

            */
            return content; 
        }

        /// <summary>
        /// Create Csv File at specified location;
        /// If file already exists only write content and add it to existing file;
        /// WARNING: If save vars are changed, title and content line might not match!
        /// </summary>
        public void WriteCSVFile()
        {
            TextWriter textWriter; 
            _balloonAmount = GameManager.instance.Balloons.Count;

            // Write CSV Title Line
            if (!File.Exists(_fileLoc + _fileName))
            {
                textWriter = new StreamWriter(_fileLoc + _fileName, false);
                textWriter.WriteLine(CreateCSVTitle());
                textWriter.Close();
            }
            
            // Write CSV Content
            textWriter = new StreamWriter(_fileLoc + _fileName, true);
            textWriter.WriteLine(CreateCSVLine());
            textWriter.Close();
        }


        // Post Data to online database
        private IEnumerator PostData()
        {
            _balloonAmount = GameManager.instance.Balloons.Count;

            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            
            formData.Add(new MultipartFormDataSection("id", PlayerID + "_" + DateTime.Now.Day + "/" + DateTime.Now.Month + "_" + DateTime.Now.Hour + ":" + DateTime.Now.Minute));

            TimeSpan timeSpent = System.DateTime.Now - _startTime;
            formData.Add(new MultipartFormDataSection("startTime", _startTime.ToString()));
            formData.Add(new MultipartFormDataSection("timeSpent", timeSpent.ToString()));
            

            for (int i = 0; i < _balloonData.Count; i++)
            {
                var data = _balloonData[i]; 
                
                formData.Add(new MultipartFormDataSection("b" + i + "_balloonType", data.balloonType.ToString()));
                formData.Add(new MultipartFormDataSection("b" + i + "_didCashIn", data.didCashIn.ToString()));
                formData.Add(new MultipartFormDataSection("b" + i + "_earned", data.earned.ToString()));
                formData.Add(new MultipartFormDataSection("b" + i + "_totalEarned", data.totalEarned.ToString()));
                formData.Add(new MultipartFormDataSection("b" + i + "_earned", data.numberOfPumps.ToString()));
            }



            using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, formData))
            {
                // Hopefully allow for https transfer
                webRequest.SetRequestHeader("Access-Control-Allow-Origin", "*");
                webRequest.certificateHandler = new BybassHTTPSCertificate();

                yield return webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                        StartCoroutine(retrySendData());
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        StartCoroutine(retrySendData());
                        Debug.LogError("PostDataRequest Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        StartCoroutine(retrySendData());
                        Debug.LogError("PostDataRequest HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("PostDataRequest Response: " + webRequest.downloadHandler.text);
                        break;
                }
            }
        }

        IEnumerator retrySendData ()
        {
            //GameObject tempError = Instantiate();
            yield return new WaitForSeconds(1);
            //Destroy(tempError);
            StartCoroutine(PostData());
            
        }

        public void finishSavingAndCloseProgram()
        {
            foreach (var dic in allSaveData)
            {
                foreach (var dicEntry in dic)
                {
                    Debug.Log(dicEntry.Value + "_" + dicEntry.Key);
                }
            }
        }
        
    }
}