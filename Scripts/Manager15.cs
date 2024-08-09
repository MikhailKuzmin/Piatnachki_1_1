using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.CompilerServices;



public class Manager15 : MonoBehaviour
{
    public GameObject ProtoButton; // прототип кнопки - €чейки п€тнашек
    public GameObject ProtoLevelButton; // прототип кнопки выбора уровн€
    public GameObject AudioButton;
    public GameObject MenuButton;
    public GameObject RestartButton;
    public GameObject ContinueButton;
    public Transform Sakura; // игрова€ фонова€ панель
    public Transform MenuPanel; //панель выбора уровн€
    public Transform Content;
    public Transform Map_Content;
    GameObject[,] cells = new GameObject[4, 4]; //массив €чеек п€тнашек
    public GameObject[] LevelButtons = new GameObject[22]; // ћассив кнопок выбора уровней
    public Images_16[] levels = new Images_16[22]; //массив уровней
    public Text num; // номер выбранной €чейки 
    public Text LevelNumber; // номер выбранного уровн€
    int h16, w16, hi, wj;
    public AudioClip[] audioData = new AudioClip[2];
    public GameObject WinPanel; //ѕанель победы
    bool GameStarted = false; //началась игра или нет
    //string Saves = "saves15.txt"; //сохран€ет номер последнего непройденного уровн€
    //string SaveMap = "savesmap.txt"; //сохран€ет положение п€тнашек
    public string LastLevel = "-1";
    public Text _Timer;
    public float _timer_f; 
    public int[] All_Levels_Progress = new int[22];
    public Text VictoryTimer;
    public Image VictoryStars;
    public List<Sprite> VictoryStarsImages;
    public Transform WinContent;
    float sootnosh;

    // Start is called before the first frame update
    void Start()
    {
        //MapSizeUpdater();
        sootnosh = (Screen.width / Screen.height) * Screen.width;
        //if (File.Exists(Saves))
        //{
        //    LastLevel = File.ReadAllText(Saves);
        //    LevelNumber.text = LastLevel;
        //}
        if (PlayerPrefs.HasKey("last_level_set"))
        {
            LastLevel = PlayerPrefs.GetString("last_level_set");
            LevelNumber.text = LastLevel;
        }
        if(PlayerPrefs.HasKey("audio_set"))
        {
            if(PlayerPrefs.GetInt("audio_set") == 1)
            {
                GetComponent<AudioSource>().enabled = false;
            }
            if (PlayerPrefs.GetInt("audio_set") == 0)
            {
                GetComponent<AudioSource>().enabled = true;
            }
            AudioActive();
        }
        Create_Menu();
        
        if (PlayerPrefs.HasKey("_all_levels_stars"))
        {
            string _all_levels_progress_str = PlayerPrefs.GetString("_all_levels_stars");
            Debug.Log(_all_levels_progress_str);
            //string _all_levels_progress_str = PlayerPrefs.GetString("_all_levels_stars");
            Debug.Log(_all_levels_progress_str);
            string[] _all_levels_stars = _all_levels_progress_str.Split("_");
            for (int i = 0; i < All_Levels_Progress.Length; i++)
            {
                _all_levels_stars[i] = _all_levels_stars[i].Replace(" ", "");
                All_Levels_Progress[i] = System.Convert.ToInt32(_all_levels_stars[i]);
                if(All_Levels_Progress[i] == -1)
                {
                    LevelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    if (All_Levels_Progress[i] == 0)
                    {
                        LevelButtons[i].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("zero_stars");
                        LevelButtons[i].transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else if (All_Levels_Progress[i] == 1)
                    {
                        LevelButtons[i].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("one_stars");
                        LevelButtons[i].transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else if (All_Levels_Progress[i] == 2)
                    {
                        LevelButtons[i].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("two_stars");
                        LevelButtons[i].transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else if (All_Levels_Progress[i] == 3)
                    {
                        LevelButtons[i].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("three_stars");
                        LevelButtons[i].transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < All_Levels_Progress.Length; i++)
            {
                All_Levels_Progress[i] = -1;
            }
        }
        if(PlayerPrefs.HasKey("ContinueButton"))
        {
            if (PlayerPrefs.GetInt("ContinueButton") == 0)
            {
                ContinueButton.gameObject.SetActive(false);
            }
            else
            {
                ContinueButton.gameObject.SetActive(true);
            }
        }
        MapSizeUpdater();

        //Rand_move();
    }
    //
    // Update is called once per frame
    void Update()
    {
        float new_sootn = (Screen.width / Screen.height) * Screen.width;
        if (new_sootn!= sootnosh)
        {
            MapSizeUpdater();
            sootnosh = new_sootn;
        }
        if (GameStarted)
        {
            Recreate();
            Victory();
            if(!WinPanel.activeSelf)
            {
                _timer_f += 1 * Time.deltaTime;
                _Timer.text = System.Math.Floor(_timer_f / 60).ToString() + ":" + System.Math.Floor(_timer_f % 60).ToString();
            }
           
        }
    }

    public void MapSizeUpdater()
    {
        
        if (Screen.width / Screen.height < 1.2f && Screen.width / Screen.height > 0.8f)
        {
            WinContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2((Screen.height / 6) * 3.29f, Screen.height / 6);
            Map_Content.transform.parent.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height * 0.8f, Screen.height * 0.8f);
            Map_Content.GetComponent<GridLayoutGroup>().cellSize = new Vector2((Screen.height * 0.8f - 30) / 4, (Screen.height * 0.8f - 30) / 4);
            MenuButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 10, Screen.width / 10);
            RestartButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 10, Screen.width / 10);
            AudioButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 8, Screen.height / 10);
            Content.transform.parent.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -Screen.height / 10);
            Content.GetComponent<GridLayoutGroup>().constraintCount = 2;
            Content.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Screen.width / 3, Screen.width / 3);
        }
        else if (Screen.width < Screen.height) //высокое портретное
        {
            WinContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2((Screen.width * 0.7f), (Screen.width * 0.7f) / 3.29f);
            Map_Content.transform.parent.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.9f, Screen.width * 0.9f);
            Map_Content.GetComponent<GridLayoutGroup>().cellSize = new Vector2((Screen.width * 0.9f-30) / 4, (Screen.width * 0.9f - 30) / 4);
            MenuButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 10, Screen.width / 10);
            RestartButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 10, Screen.width / 10);
            AudioButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 5, Screen.width / 8);
            Content.transform.parent.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -Screen.width / 8);
            Content.GetComponent<GridLayoutGroup>().constraintCount = 2;
            Content.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Screen.width / 2.5f, Screen.width / 2.5f);
        }

        else if (Screen.width > Screen.height) //широкое альбомное
        {
            WinContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2((Screen.height / 6) * 3.29f, Screen.height / 6);
            Map_Content.transform.parent.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height*0.9f, Screen.height * 0.9f);
            Map_Content.GetComponent<GridLayoutGroup>().cellSize = new Vector2((Screen.height * 0.9f-30) / 4, (Screen.height * 0.9f - 30) / 4);
            MenuButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 8, Screen.height / 8);
            RestartButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 8, Screen.height / 8);
            AudioButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 7, Screen.height / 7);
            Content.transform.parent.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -Screen.height / 7);
            Content.GetComponent<GridLayoutGroup>().constraintCount = 3;
            Content.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Screen.width / 3, Screen.width / 3);
        }
        for (int i = 0; i < All_Levels_Progress.Length; i++)
        {
            LevelButtons[i].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(Content.GetComponent<GridLayoutGroup>().cellSize.x*0.9f, (Content.GetComponent<GridLayoutGroup>().cellSize.y * 0.9f)/ 3.29f);
        }









        }

    public void Create_Menu()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            LevelButtons[i] = Instantiate(ProtoLevelButton);
            LevelButtons[i].transform.SetParent(Content);
            LevelButtons[i].transform.localPosition = new Vector3((i % 2) * 430 - 220, (i / 2) * (-420) + 550, 1);
            LevelButtons[i].transform.localScale = new Vector3(1, 1, 1);
            //levels[i].LevelLogo = Resources.Load<Sprite>("Images//Logos" + (i+1).ToString());
            //LevelButtons[i].GetComponent<Image>().sprite = levels[i].LevelLogo;
            LevelButtons[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Logos/" + (i + 1).ToString());
            LevelButtons[i].GetComponentInChildren<Text>().text = i.ToString();
            if (i.ToString() == LastLevel)
            {
                ContinueButton.transform.SetParent(LevelButtons[i].transform);
                ContinueButton.transform.localPosition = new Vector3(0, 0, 1);
            }
        }

    }

    public void Create_Map()
    {
        MenuPanel.gameObject.SetActive(false);
        LastLevel = LevelNumber.text;
        PlayerPrefs.SetString("last_level_set", LastLevel);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                //основна€ карта
                cells[i, j] = Instantiate(ProtoButton);
               
                cells[i, j].transform.SetParent(Map_Content);
                cells[i, j].transform.localPosition = Vector3.zero;
                cells[i, j].transform.localScale = new Vector3(1, 1, 1);
                cells[i, j].GetComponentInChildren<Text>().text = (i * 4 + j + 1).ToString();
                levels[System.Convert.ToInt32(LevelNumber.text)].details[i * 4 + j] = Resources.Load<Sprite>("Images/"+(System.Convert.ToInt32(LevelNumber.text)+1).ToString() + "/"+ (i * 4 + j + 1).ToString());
                cells[i, j].GetComponent<Image>().sprite = levels[System.Convert.ToInt32(LevelNumber.text)].details[i * 4 + j];
            }
            GameStarted = true;
            _timer_f = 0;
        }
    }
    public void Recreate()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (cells[i, j].GetComponentInChildren<Text>().text == "16")
                {
                    cells[i, j].GetComponentInChildren<Text>().color = new Vector4(1.5f, 0.1f, 0f, 0.0f);
                    cells[i, j].GetComponent<Image>().color = new Vector4(1.0f, 0.2f, 0f, 0.0f);
                    cells[i, j].GetComponent<Button>().interactable = false;
                }
                else
                {
                    cells[i, j].GetComponentInChildren<Text>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                    cells[i, j].GetComponent<Image>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                    cells[i, j].GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    public void move()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (cells[i, j].GetComponentInChildren<Text>().text == "16")
                {
                    h16 = i;
                    w16 = j;
                }
                if (cells[i, j].GetComponentInChildren<Text>().text == num.GetComponent<Text>().text)
                {
                    hi = i;
                    wj = j;
                }
            }
        }
        Sprite iTrans = cells[hi, wj].GetComponent<Image>().sprite;
        if ((h16 == hi && (w16 == wj + 1 || w16 == wj - 1)) || (w16 == wj && (h16 == hi + 1 || h16 == hi - 1)))
        {
            string trans = cells[h16, w16].GetComponentInChildren<Text>().text;
            cells[h16, w16].GetComponentInChildren<Text>().text = num.GetComponent<Text>().text;
            cells[hi, wj].GetComponentInChildren<Text>().text = trans;
            cells[hi, wj].GetComponent<Image>().sprite = cells[h16, w16].GetComponent<Image>().sprite;
            cells[h16, w16].GetComponent<Image>().sprite = iTrans;
            ProtoButton.GetComponent<AudioSource>().clip = audioData[0];
            ProtoButton.GetComponent<AudioSource>().Play(0);
            string map_set = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    map_set += cells[i, j].GetComponentInChildren<Text>().text + "_";
                }
            }
            PlayerPrefs.SetString("SaveMapSet", map_set);
            ContinueButton.gameObject.SetActive(true);
            //StreamWriter SvMap = new StreamWriter(SaveMap);
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        SvMap.Write(cells[i, j].GetComponentInChildren<Text>().text + " ");
            //    }
            //}
            //SvMap.Close();
        }
        else
        {
            ProtoButton.GetComponent<AudioSource>().clip = audioData[1];
            ProtoButton.GetComponent<AudioSource>().Play(0);
        }
        //StreamWriter Sv = new StreamWriter(Saves);
        //Sv.Write(LevelNumber.text);
        //Sv.Close();

        PlayerPrefs.SetFloat("TimerSet", _timer_f);
        PlayerPrefs.SetInt("ContinueButton", 1);

    }
    public void Rand_move()
    {
        System.Random rnd = new System.Random();
        for (int r = 0; r < 1000; r++)
        {
            hi = rnd.Next(0, 4);
            wj = rnd.Next(0, 4);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (cells[i, j].GetComponentInChildren<Text>().text == "16")
                    {
                        h16 = i;
                        w16 = j;
                    }
                }
            }
            Sprite iTrans = cells[hi, wj].GetComponent<Image>().sprite;
            if ((h16 == hi && (w16 == wj + 1 || w16 == wj - 1)) || (w16 == wj && (h16 == hi + 1 || h16 == hi - 1)))
            {
                string trans = cells[h16, w16].GetComponentInChildren<Text>().text;
                cells[h16, w16].GetComponentInChildren<Text>().text = cells[hi, wj].GetComponentInChildren<Text>().text;
                cells[hi, wj].GetComponentInChildren<Text>().text = trans;
                cells[hi, wj].GetComponent<Image>().sprite = cells[h16, w16].GetComponent<Image>().sprite;
                cells[h16, w16].GetComponent<Image>().sprite = iTrans;
            }

        }

    }
    public void Last_move() //метод, который воспроизводит карту п€тнашек, как в недопройденном уровне
    {
        string LastMap;
        LastMap = PlayerPrefs.GetString("SaveMapSet");
        //StreamReader Sr = new StreamReader(SaveMap);
        //LastMap = Sr.ReadToEnd();
        //Sr.Close();
        string[] ConG = LastMap.Split('_');
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                ConG[i * 4 + j] = ConG[i * 4 + j].Replace(" ", "");
                cells[i, j].GetComponentInChildren<Text>().text = ConG[i * 4 + j];
                cells[i, j].GetComponent<Image>().sprite = levels[System.Convert.ToInt32(LastLevel)].details[System.Convert.ToInt32(ConG[i * 4 + j]) - 1];
            }
            GameStarted = true;
        }
        _timer_f = PlayerPrefs.GetFloat("TimerSet");
    }
    public void Victory()
    {

        if (cells[0, 0].GetComponentInChildren<Text>().text == 1.ToString() &&
            cells[0, 1].GetComponentInChildren<Text>().text == 2.ToString() &&
            cells[0, 2].GetComponentInChildren<Text>().text == 3.ToString() &&
            cells[0, 3].GetComponentInChildren<Text>().text == 4.ToString() &&
            cells[1, 0].GetComponentInChildren<Text>().text == 5.ToString() &&
            cells[1, 1].GetComponentInChildren<Text>().text == 6.ToString() &&
            cells[1, 2].GetComponentInChildren<Text>().text == 7.ToString() &&
            cells[1, 3].GetComponentInChildren<Text>().text == 8.ToString() &&
            cells[2, 0].GetComponentInChildren<Text>().text == 9.ToString() &&
            cells[2, 1].GetComponentInChildren<Text>().text == 10.ToString() &&
            cells[2, 2].GetComponentInChildren<Text>().text == 11.ToString() &&
            cells[2, 3].GetComponentInChildren<Text>().text == 12.ToString() &&
            cells[3, 0].GetComponentInChildren<Text>().text == 13.ToString() &&
            cells[3, 1].GetComponentInChildren<Text>().text == 14.ToString() &&
            cells[3, 2].GetComponentInChildren<Text>().text == 15.ToString())
        {
            ContinueButton.gameObject.SetActive(false);
            PlayerPrefs.SetInt("ContinueButton", 0);

            VictoryTimer.text = "¬аше ¬рем€: " + System.Math.Floor(_timer_f / 60).ToString() + ":" + System.Math.Floor(_timer_f % 60).ToString();
            
            
           
            GetComponent<AudioSource>().Stop();
            if (_timer_f < 40.0f)
            {
                All_Levels_Progress[System.Convert.ToInt32(LastLevel)] = 3;
                LevelButtons[System.Convert.ToInt32(LastLevel)].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("three_stars");
                LevelButtons[System.Convert.ToInt32(LastLevel)].transform.GetChild(1).gameObject.SetActive(true);
                VictoryStars.sprite = VictoryStarsImages[3];
            }
            else if(_timer_f < 60.0f)
            {
                All_Levels_Progress[System.Convert.ToInt32(LastLevel)] = 2;
                LevelButtons[System.Convert.ToInt32(LastLevel)].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("two_stars");
                LevelButtons[System.Convert.ToInt32(LastLevel)].transform.GetChild(1).gameObject.SetActive(true);
                VictoryStars.sprite = VictoryStarsImages[2];
            }
            else if (_timer_f < 80.0f)
            {
                All_Levels_Progress[System.Convert.ToInt32(LastLevel)] = 1;
                LevelButtons[System.Convert.ToInt32(LastLevel)].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("one_stars");
                LevelButtons[System.Convert.ToInt32(LastLevel)].transform.GetChild(1).gameObject.SetActive(true);
                VictoryStars.sprite = VictoryStarsImages[1];
            }
            else if (_timer_f >= 80.0f)
            {
                All_Levels_Progress[System.Convert.ToInt32(LastLevel)] = 0;
                LevelButtons[System.Convert.ToInt32(LastLevel)].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("zero_stars");
                LevelButtons[System.Convert.ToInt32(LastLevel)].transform.GetChild(1).gameObject.SetActive(true);
                VictoryStars.sprite = VictoryStarsImages[0];
            }
            WinPanel.SetActive(true);
            string AllStars = "";
            Debug.Log(AllStars);
            for (int i = 0; i < All_Levels_Progress.Length; i++)
            {
                AllStars += All_Levels_Progress[i].ToString() + "_"; 
            }
            PlayerPrefs.SetString("_all_levels_stars", AllStars);
            Debug.Log(AllStars);

        }

    }
    public void Restart()
    {
        Rand_move();
        WinPanel.SetActive(false);
        _timer_f = 0;
        GetComponent<AudioSource>().Play();
    }

    public void Menu_Restart()
    {
        GameStarted = false;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Destroy(cells[i, j]);
            }
        }
        MenuPanel.gameObject.SetActive(true);
        for (int i = 0; i < levels.Length; i++)
        {
            if (i.ToString() == LastLevel)
            {
                ContinueButton.transform.SetParent(LevelButtons[i].transform);
                ContinueButton.transform.localPosition = new Vector3(0, 0, 1);
            }
        }
        WinPanel.SetActive(false);
        GetComponent<AudioSource>().Play();

    }
    public void AudioActive()
    {
        if (GetComponent<AudioSource>().enabled == false)
        {
            GetComponent<AudioSource>().enabled = true;
            ProtoButton.GetComponent<AudioSource>().enabled = true;
            MenuButton.GetComponent<AudioSource>().enabled = true;
            RestartButton.GetComponent<AudioSource>().enabled = true;
            WinPanel.GetComponent<AudioSource>().enabled = true;
            AudioButton.GetComponentInChildren<Text>().text = "ћузыка";
            
            PlayerPrefs.SetInt("audio_set", 1);
            
        }
        else
        {
            GetComponent<AudioSource>().enabled = false;
            ProtoButton.GetComponent<AudioSource>().enabled = false;
            MenuButton.GetComponent<AudioSource>().enabled = false;
            RestartButton.GetComponent<AudioSource>().enabled = false;
            WinPanel.GetComponent<AudioSource>().enabled = false;
            AudioButton.GetComponentInChildren<Text>().text = "ћузыка выкл";
            PlayerPrefs.SetInt("audio_set", 0);
        }

    }
    [System.Serializable]
    public class Images_16 //класс уровн€
    {
        public Sprite LevelLogo;
        public Sprite[] details = new Sprite[16];
    }
    //public void Create_SaveMap()
    //{
    //    string LastMap;
    //    StreamReader Sr = new StreamReader(SaveMap);
    //    LastMap = Sr.ReadToEnd();
    //    Sr.Close();
    //    string[] ConG = LastMap.Split('\n');
    //    MenuPanel.gameObject.SetActive(false);
    //    for (int i = 0; i < 4; i++)
    //    {
    //        for (int j = 0; j < 4; j++)
    //        {
    //            //основна€ карта
    //            cells[i, j] = Instantiate(ProtoButton);
    //            cells[i, j].transform.SetParent(Sakura);
    //            cells[i, j].transform.localPosition = new Vector3(j * 205 - 307, i * (-205) + 307, 1);
    //            cells[i, j].transform.localScale = new Vector3(1, 1, 1);
    //            ConG[i * 4 + j] = ConG[i * 4 + j].Replace(" ", "");
    //            cells[i, j].GetComponentInChildren<Text>().text = ConG[i * 4 + j];
    //            cells[i, j].GetComponent<Image>().sprite = levels[System.Convert.ToInt32(LastLevel)].details[System.Convert.ToInt32(ConG[i * 4 + j]) - 1];
    //        }
    //        GameStarted = true;
    //    }
    //    StreamWriter Sv = new StreamWriter(Saves);
    //    Sv.Write(LevelNumber.text);
    //    Sv.Close();
    //}
    //public void DetailClick_Audio()
    //{
    //    ProtoButton.GetComponent<AudioSource>().Play(0);
    //}
}
