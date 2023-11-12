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
    public int themeNum; //���v�̃v���[����
    private int nowThemeCount;//���݂̃v���[����
    private int limitTime; //�������ԁ@
    public int timeSpan;�@//�v���[�����Ԑݒ�
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

    private List<string> themeList; //�v���[���e�[�}���X�g
    internal List<int> SUDSScores;

    internal int status;

    internal int second; //�J�E���g�_�E���̎��ԁi�b�j
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
        presenMonitor.SetText("��ʂ��N���b�N�����ăX�^�[�g");
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
    /// �c�莞�Ԍv��
    /// </summary>
    internal void LimitTimer()
    {
        second = limitTime - (int)Math.Round(Time.time);
    }

    /// <summary>
    /// ���Ԃ��ĊJ
    /// </summary>
    internal void RestartTime()
    {
        limitTime = (int)Math.Round(Time.time);
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
    /// �v���[���e�[�}�̐؂�ւ�
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
    /// �v���[���e�[�}�̕\��
    /// </summary>
    internal void ShowTheme()
    {
        if (second > 0)
        {
            return;
        }
        limitTime += timeSpan; //�������Ԃ��Z�b�g

        for (int count = 0; count < themeList.Count; count++)
        {
            if (count == nowThemeCount)
            {
                int _num = count + 1;
                presenMonitor.fontSize = 36;
                presenMonitor.SetText(_num + "\r\n" + themeList[count]);
            }
            else if (nowThemeCount >= themeNum) //���݂̃e�[�}�����S�e�[�}���𒴂����Ƃ�
            {
                status = 2;
                string _SUDSRec = null;
                for (int i = 0; i < SUDSScores.Count; i++)
                {
                    _SUDSRec = String.Concat(_SUDSRec,SUDSScores[i] + "�_ ");
                }
                presenMonitor.fontSize = 26;
                presenMonitor.SetText("�����l�ł����B\r\n �w�b�h�Z�b�g���O���Ď����҂ɐ��������Ă��������B\r\n" + _SUDSRec); //�v���[�����I��
                second = 0;
            }
        }
        nowThemeCount++;
        isRestart = false;
    }

    /// <summary>
    /// SUDS�̕\��
    /// </summary>
    internal void ShowSUDS()
    {
        string _SUDSRec = null;
        for(int i = 0; i <SUDSScores.Count; i++)
        {
            int _num = i + 1;
            _SUDSRec = String.Concat(_SUDSRec, _num + "���:" + SUDSScores[i] + "�_ ");
        }
        presenMonitor.fontSize = 26;
        presenMonitor.SetText("���̕s���ɂ���0�`100�œ����Ă�������\r\n" + _SUDSRec);
    }
}
