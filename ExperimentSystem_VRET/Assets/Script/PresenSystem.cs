using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class PresenSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI presenMonitor;
    public TextMeshProUGUI timer;
    public int themeNum; //合計のプレゼン数
    private int nowThemeCount;//現在のプレゼン数
    private int limitTime; //制限時間　
    public int timeSpan;　//プレゼン時間設定
    private bool isSUDS;
    private bool isRestart;

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

    internal bool isPresentating;
    internal int status;

    internal int second; //カウントダウンの時間（秒）
    public GameObject pointer;


    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

        if(status == 0)
        {
            presenMonitor.SetText("Spaceでスタート");
            if (Input.GetKeyDown("space"))
            {
                status = 1;
            }
        }
        else if (status == 1)
        {
            if (isSUDS)
            {
                if (Input.GetKeyDown("space"))
                {
                    isSUDS = false;
                    isRestart = true;
                }
                ShowSUDS();
                pointer.SetActive(true);
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
        else if(status == 2)
        {
            if (Input.GetKeyDown("space"))
            {
                status = 0;
                Init();
            }
            pointer.SetActive(true);
        }
    }

    public void Click()
    {
        if (status == 0)
        {
            status = 1;
        }
        else if(status == 1)
        {
            if (isSUDS)
            {
                isSUDS = false;
                isRestart = true;
            }
        }
        else if(status == 2)
        {
            status = 0;
            Init();
        }
    }

    internal void Init()
    {
        second = -1;
        nowThemeCount = 0;
        limitTime = 0;
        themeList = new List<string>() { theme1, theme2, theme3, theme4, theme5, theme6, theme7, theme8, theme9 };
        presenMonitor.SetText(theme1);
        isSUDS = true;
        isRestart = false;
        status = 0;
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
    /// プレゼンテーマの表示
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
                presenMonitor.SetText(_num + "\r\n" + themeList[count]);
            }
            else if (nowThemeCount >= themeNum) //現在のテーマ数が全テーマ数を超えたとき
            {
                status = 2;
                presenMonitor.SetText("お疲れ様でした。\r\n ヘッドセットを外して実験者に声をかけてください"); //プレゼンを終了
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
        presenMonitor.SetText("今の不安について1～100で答えてください");
    }

}
