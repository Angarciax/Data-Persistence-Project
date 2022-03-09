using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    const int HoFSize = 5;
    public static GameManager Instance;
    public int ballSize = -1; // 0=small, 1=medium, 2=large
    public string playerName;
    public int highScore;
    public HoF gameHoF;
    public HoFPerson[] dataHoF = new HoFPerson[HoFSize];
    private TextMeshProUGUI[] screenEntriesHoF = new TextMeshProUGUI[HoFSize];
    const string saveFilename = "Brickanoid_HoF.json";

    [Serializable] public struct HoFPerson
    {
        public int score;
        public string name;
    }

    [Serializable] public class HoF
    { // Oh! My... Serilizable won't accept an array
        public HoFPerson high1;
        public HoFPerson high2;
        public HoFPerson high3;
        public HoFPerson high4;
        public HoFPerson high5;
    }

    private void Awake()
    {
        // Just this instance
        if( Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // 
        Instance = this;
        DontDestroyOnLoad(gameObject);
        gameHoF = new HoF() ;
        LoadHoF();
    }

       // Start is called before the first frame update
    void Start()
    {
        // Paint HoF
        PaintHoF();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            StartMainScene( 0 );
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            StartMainScene( 1 );
        }
        else if(Input.GetKeyDown(KeyCode.F3))
        {
            StartMainScene( 2 );
        }
    }

    public void PaintHoF()
    {
        // Search again TextMeshProUGUI[] screenEntriesHoF. They got destroyed every scene reload
        screenEntriesHoF[0] = GameObject.Find("HoFPlayer1").GetComponent<TextMeshProUGUI>();
        screenEntriesHoF[1] = GameObject.Find("HoFPlayer2").GetComponent<TextMeshProUGUI>();
        screenEntriesHoF[2] = GameObject.Find("HoFPlayer3").GetComponent<TextMeshProUGUI>();
        screenEntriesHoF[3] = GameObject.Find("HoFPlayer4").GetComponent<TextMeshProUGUI>();
        screenEntriesHoF[4] = GameObject.Find("HoFPlayer5").GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < HoFSize; i++)
            {
                screenEntriesHoF[i].text = NumberText(dataHoF[i].score) + " ... " + dataHoF[i].name;
            }        
    }

    public void StartMainScene( int bSize )
    {
        ballSize = bSize ;
        playerName = GameObject.Find("PlayerName").GetComponent<TMP_InputField>().text;
        SceneManager.LoadScene("main");
    }

    public void LoadHoF()
    {
        string filePath = Application.persistentDataPath + "/" + saveFilename; 
        if (!File.Exists(filePath))
        {
            // No existe fichero. Crear HoF a mano
            gameHoF.high1.name = "Gandalf"; gameHoF.high1.score = 5;
            gameHoF.high2.name = "Bilbo"; gameHoF.high2.score = 4;
            gameHoF.high3.name = "Gollum"; gameHoF.high3.score = 3;
            gameHoF.high4.name = "Sauron"; gameHoF.high4.score = 2;
            gameHoF.high5.name = "Thorin"; gameHoF.high5.score = 1;
        }
        else
        { // Load & Deserialize
            string textJson = File.ReadAllText(filePath);
            gameHoF  = JsonUtility.FromJson<HoF>(textJson);
        }
        DataHoF2Table();
        highScore = gameHoF.high1.score ;
    }
    
    public void SaveHoF()
    {
        // Copy to serializable data, Serialize and save
        Table2DataHoF();
        string filePath = Application.persistentDataPath + "/" + saveFilename;
        string textJson = JsonUtility.ToJson( gameHoF );
        File.WriteAllText(filePath, textJson);
    }

    void DataHoF2Table()
    {
        dataHoF[0].score = gameHoF.high1.score; dataHoF[0].name = gameHoF.high1.name;
        dataHoF[1].score = gameHoF.high2.score; dataHoF[1].name = gameHoF.high2.name;
        dataHoF[2].score = gameHoF.high3.score; dataHoF[2].name = gameHoF.high3.name;
        dataHoF[3].score = gameHoF.high4.score; dataHoF[3].name = gameHoF.high4.name;
        dataHoF[4].score = gameHoF.high5.score; dataHoF[4].name = gameHoF.high5.name;
    }

    void Table2DataHoF()
    {
        gameHoF.high1.score = dataHoF[0].score; gameHoF.high1.name = dataHoF[0].name;
        gameHoF.high2.score = dataHoF[1].score; gameHoF.high2.name = dataHoF[1].name;
        gameHoF.high3.score = dataHoF[2].score; gameHoF.high3.name = dataHoF[2].name;
        gameHoF.high4.score = dataHoF[3].score; gameHoF.high4.name = dataHoF[3].name;
        gameHoF.high5.score = dataHoF[4].score; gameHoF.high5.name = dataHoF[4].name;
    }
    public bool UpdateHoF( string name, int score)
    {
        // Buscar si entra en HoF
        int HoFPlace = 0;
        while (HoFPlace < HoFSize && dataHoF[HoFPlace].score >= score ) HoFPlace++;
        if( HoFPlace < HoFSize ) // SI -> Entra en HoF
        {
            // Hacerle hueco
            for ( int position = HoFSize -1 ; position > HoFPlace; position--)
            {
                dataHoF[position].score = dataHoF[position - 1].score;
                dataHoF[position].name = dataHoF[position - 1].name;
            }
            dataHoF[HoFPlace].name = name;
            dataHoF[HoFPlace].score = score;
            // y grabar
            SaveHoF();
            if (score > highScore)
                highScore = score;
            return true;
        }
        return false;
    }
    string NumberText( int score)
    { // Return 3 digits for an integer ("005" instead of "5" or "  5")
        string text = "000";
        text += score;
        text = text.Substring(text.Length - 3, 3);
        return text;
    }

}
