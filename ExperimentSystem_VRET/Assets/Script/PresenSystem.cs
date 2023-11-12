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
    public int themeNum; //合計のプレゼン数
    private int nowThemeCount;//現在のプレゼン数
    private int limitTime; //制限時間　
    public int timeSpan;　//プレゼン時間設定
    internal bool isSUDS;
    internal bool isRestart;
    internal bool isPresentating;

    public string theme1;
    public string theme2;
    public string theme3;
    public string theme4;
    public string theme5;
    public string theme6;
    public string theme7;
    public string theme8;
    public string theme9;

    private List<string> themeList; //プレゼンテーマリスト
    internal List<int> SUDSScores;

    internal int status;

    internal int second; //カウントダウンの時間（秒）
    public GameObject pointer;
    internal GameObject NumKeypad;

    void Start()
    {
        NumKeypad = GameObject.Find("NumKeypad");
        
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
        NumKeypad.SetActive(false);
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
        themeList = new List<string>() { theme1, theme2, theme3, theme4, theme5, theme6, theme7, theme8, theme9 };
        SUDSScores = new List<int>();
        presenMonitor.SetText(theme1);
        isSUDS = true;
        isRestart = false;
        status = 0;
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
            pointer.SetActive(false);
        }
        else
        {
            ChangeTheme();
            ShowLimitTime();
        }
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
            SUDS suds = NumKeypad.GetComponent<SUDS>();
            SUDSScores.Add(suds.SUDSScore);
            EndSUDS();
            return;
        }
        ShowSUDS();
        pointer.SetActive(true);
        NumKeypad.SetActive(true);
    }

    internal void EndSUDS()
    {
        isSUDS = false;
        isRestart = true;
        pointer.SetActive(false);
        NumKeypad.SetActive(false);
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
}
