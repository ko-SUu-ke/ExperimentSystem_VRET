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
    public int themeNum; //���v�̃v���[����
    private int nowThemeCount;//���݂̃v���[����
    private int limitTime; //�������ԁ@
    public int timeSpan;�@//�v���[�����Ԑݒ�

    public string theme1;
    public string theme2;
    public string theme3;
    public string theme4;
    public string theme5;
    public string theme6;
    public string theme7;
    public string theme8;
    public string theme9;

    private List<string> themeList; //�v���[���e�[�}���X�g

    internal bool isPresentating;

    internal int second; //�J�E���g�_�E���̎��ԁi�b�j



    void Start()
    {
        second = -1;
        nowThemeCount = 0;
        limitTime = timeSpan;
        themeList = new List<string>() { theme1, theme2, theme3, theme4, theme5, theme6, theme7, theme8, theme9 };
        presenMonitor.SetText(theme1);
        isPresentating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPresentating)
        {
            LimitTimer();
            ShowLimitTime();
            ShowTheme();
        }
    }

    /// <summary>
    /// �c�莞�Ԍv��
    /// </summary>
    internal void LimitTimer()
    {
        second = limitTime - (int)Math.Round(Time.time);
    }

    /// <summary>
    /// �c�莞�ԕ\��
    /// </summary>
    internal void ShowLimitTime()
    {
        var span = new TimeSpan(0, 0, second);
        string mmss = span.ToString(@"mm\:ss");
        timer.SetText("TIME : " + mmss);
    }

    /// <summary>
    /// �v���[���e�[�}�̕\��
    /// </summary>
    internal void ShowTheme()
    {
        if (second > 0)
        {
            return;
        }

        nowThemeCount++;
        limitTime += timeSpan;

        for (int count = 0; count < themeList.Count; count++)
        {
            if (count == nowThemeCount)
            {
                int _num = count + 1;
                presenMonitor.SetText(_num + themeList[count]);
            }
            else if (nowThemeCount >= themeNum) //���݂̃e�[�}�����S�e�[�}���𒴂����Ƃ�
            {
                isPresentating = false; //�v���[�����I��
                presenMonitor.SetText("�I��");
            }
        }
    }
}
