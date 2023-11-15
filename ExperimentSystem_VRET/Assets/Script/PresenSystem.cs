using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Linq;

public class PresenSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI presenMonitor;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI AttMonitor;
    public int themeNum; //合計のプレゼン数
    private int nowThemeCount;//現在のプレゼン数
    private int limitTime; //制限時間　
    public int timeSpan;　//プレゼン時間設定
    internal bool isSUDS;
    internal bool isRestart;
    internal bool isPresentating;

    public List<string> themeList;  //プレゼンテーマリスト
    public List<string> attTextList;//ATTのテキスト・音声リスト
    public List<AudioClip> attAudioList;
    public int attTimeSpan;         //ATTターゲットの表示時間
    public int attHidTimeSpan;    //ATTターゲットの非表示時間
    private int nextShowAttTime;    //次にターゲットが現れる時間
    private int nextHidAttTime;     //次にターゲットが隠れる時間

    internal AudioSource audioSource;
    private int attNum;
    internal int nowAttCount;
    internal bool playAttAudio;
    internal bool showAttTarget;

    internal List<int> SUDSScores;

    internal int status;

    internal int second; //カウントダウンの時間（秒）
    public GameObject pointer;
    internal GameObject numKeypad;
    public GameObject attMonitorObj;
    public List<GameObject> attTargets;

    void Start()
    {
        numKeypad = GameObject.Find("NumKeypad");
        
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(status == 0)
        {
            TitleScreen();
        }
        else if (status == 1)
        {
            PresenScreen();
        }
        else if(status == 2)
        {
            EndScreen();
        }
    }

    internal void TitleScreen()
    {
        presenMonitor.SetText("画面をクリックをしてスタート");
        numKeypad.SetActive(false);
        if (Input.GetKeyDown("space"))
        {
            status = 1;
        }
    }

    internal void Init()
    {
        second = -1;
        nowThemeCount = 0;
        limitTime = 0;
        SUDSScores = new List<int>();
        isSUDS = true;
        isRestart = false;
        status = 0;
        audioSource = GetComponent<AudioSource>();
        attNum = attAudioList.Count;
        nowAttCount = 0;
        playAttAudio = false;
        nextShowAttTime = attHidTimeSpan;
        nextHidAttTime = 0;
        
        foreach(GameObject attTarget in attTargets)
        {
            attTarget.SetActive(false);
        }
        attMonitorObj.SetActive(false);
    }

    internal void PresenScreen()
    {
        if (isSUDS)
        {
            SUDSScreen();
        }
        else if (isRestart)
        {
            RestartTime();
            ShowTheme();
            ShowLimitTime();
            Att();
            pointer.SetActive(false);
        }
        else
        {
            ChangeTheme();
            ShowLimitTime();
            Att();
        }
        AttTime();
        LimitTimer();
    }

    internal void EndScreen()
    {
        if (Input.GetKeyDown("space"))
        {
            status = 0;
            Init();
        }
        pointer.SetActive(true);
    }

    internal void SUDSScreen()
    {
        if (Input.GetKeyDown("space"))
        {
            SUDS suds = numKeypad.GetComponent<SUDS>();
            SUDSScores.Add(suds.SUDSScore);
            EndSUDS();
            return;
        }
        ShowSUDS();
        pointer.SetActive(true);
        numKeypad.SetActive(true);
        attMonitorObj.SetActive(false);
    }

    internal void EndSUDS()
    {
        isSUDS = false;
        isRestart = true;
        pointer.SetActive(false);
        numKeypad.SetActive(false);
        //AttMonitorObj.SetActive(true);
    }

    public void Click()
    {
        if (status == 0)
        {
            status = 1;
        }
        else if(status == 2)
        {
            status = 0;
            Init();
        }
    }

    /// <summary>
    /// 残り時間計測
    /// </summary>
    internal void LimitTimer()
    {
        second = limitTime - (int)Math.Round(Time.time);
    }

    /// <summary>
    /// 時間を再開
    /// </summary>
    internal void RestartTime()
    {
        limitTime = (int)Math.Round(Time.time);
    }

    /// <summary>
    /// 残り時間表示
    /// </summary>
    internal void ShowLimitTime()
    {
        var span = new TimeSpan(0, 0, second);
        string mmss = span.ToString(@"mm\:ss");
        timer.SetText("TIME : " + mmss);
    }

    /// <summary>
    /// プレゼンテーマの切り替え
    /// 
    /// </summary>
    internal void ChangeTheme()
    {
        if (second > 0)
        {
            return;
        }

        if (nowThemeCount == 0 || nowThemeCount == 3 || nowThemeCount == 6 || nowThemeCount == 9)
        {
            isSUDS = true;
            return;
        }
        
        ShowTheme();
    }

    /// <summary>
    /// プレゼンテーマの表示
    /// </summary>
    internal void ShowTheme()
    {
        if (second > 0)
        {
            return;
        }
        limitTime += timeSpan; //制限時間をセット
        nextShowAttTime = attHidTimeSpan;
        nextHidAttTime = 0;

        for (int count = 0; count < themeList.Count; count++)
        {
            if (count == nowThemeCount)
            {
                int _num = count + 1;
                presenMonitor.fontSize = 36;
                presenMonitor.SetText(_num + "\r\n" + themeList[count]);
            }
            else if (nowThemeCount >= themeNum) //現在のテーマ数が全テーマ数を超えたとき
            {
                status = 2;
                string _SUDSRec = null;
                for (int i = 0; i < SUDSScores.Count; i++)
                {
                    _SUDSRec = String.Concat(_SUDSRec,SUDSScores[i] + "点 ");
                }
                presenMonitor.fontSize = 26;
                presenMonitor.SetText("お疲れ様でした。\r\n ヘッドセットを外して実験者に声をかけてください。\r\n" + _SUDSRec); //プレゼンを終了
                second = 0;
            }
        }
        nowThemeCount++;
        isRestart = false;
    }

    /// <summary>
    /// SUDSの表示
    /// </summary>
    internal void ShowSUDS()
    {
        string _SUDSRec = null;
        for(int i = 0; i <SUDSScores.Count; i++)
        {
            int _num = i + 1;
            _SUDSRec = String.Concat(_SUDSRec, _num + "回目:" + SUDSScores[i] + "点 ");
        }
        presenMonitor.fontSize = 26;
        presenMonitor.SetText("今の不安について0～100で答えてください\r\n" + _SUDSRec);
    }

    /// <summary>
    /// ATTのターゲット示す
    /// </summary>
    internal void Att()
    {
        if (playAttAudio)
        {
            if (nowAttCount == attTargets.Count - 1)
            {
                nowAttCount = 0;
            }
            else
            {
                nowAttCount++;
            }
            //audioSource.PlayOneShot(attAudioList[nowAttCount]);
            playAttAudio = false;
        }
        if (showAttTarget)
        {
            //AttMonitor.SetText(attTextList[nowAttCount]);
            attTargets[nowAttCount].SetActive(true);
        }
        else
        {
            //AttMonitor.SetText(" ");
            attTargets[nowAttCount].SetActive(false);
        }
    }

    /// <summary>
    /// Attの時間管理
    /// </summary>
    internal void AttTime()
    {
        if (second == timeSpan - nextHidAttTime)
        {
            playAttAudio = false;
            showAttTarget = false;
            nextHidAttTime += attTimeSpan + attHidTimeSpan;
        }else if(second == timeSpan - nextShowAttTime){
            playAttAudio = true;
            showAttTarget = true;
            nextShowAttTime += attTimeSpan + attHidTimeSpan;
        }
    }
}
