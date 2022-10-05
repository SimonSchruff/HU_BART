using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Linq;


namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager instance;

        // Specifies where to save csv file; Editor saves in project folder and android on persistent path;
        [Serializable] public enum SaveLocation
        {
            Editor,
            Android
        }
        public SaveLocation SelectedSaveLocation = SaveLocation.Editor; 

        public string PlayerID = "test_id";
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
        }

        // Create Title Line for CSV File
        private string CreateCSVTitle()
        {
            string returnTitle = "ID";
            string[] balloonDataTitles = new string[_balloonAmount];
            
            for (int i = 0; i < _balloonAmount; i++) {
                balloonDataTitles[i] = $",{i}_Type,{i}_Earned,{i}_NumberOfPumps,{i}_TotalEarned,{i}_DidCashIn,{i}_DidSecondCashIn";
                returnTitle += balloonDataTitles[i]; 
            }
            
            return returnTitle;
        }

        // Create Data Entry Line for CSV File
        private string CreateCSVLine()
        {
            string content = PlayerID + "_" + _startTime.Month +"_"+ _startTime.Day +"_"+ + _startTime.Hour +"_"+ _startTime.Minute;
            string[] contentData = new string[_balloonAmount];

            for (int i = 0; i < _balloonData.Count; i++) {
                contentData[i] = $",{_balloonData[i].balloonType},{_balloonData[i].earned},{_balloonData[i].numberOfPumps},{_balloonData[i].totalEarned},{_balloonData[i].didCashIn},{_balloonData[i].didSecondCashIn}";
                content += contentData[i]; 
            }

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
        
    }
}