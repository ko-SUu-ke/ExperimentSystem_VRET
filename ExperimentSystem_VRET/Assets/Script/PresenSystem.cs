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
    public int themeNum; //���v�̃v���[����
    private int nowThemeCount;//���݂̃v���[����
    private int limitTime; //�������ԁ@
    public int timeSpan;�@//�v���[�����Ԑݒ�
    internal bool isSUDS;
    internal bool isRestart;
    internal bool isPresentating;

    public List<string> themeList;  //�v���[���e�[�}���X�g
    public List<string> attTextList;//ATT�̃e�L�X�g�E�������X�g
    public List<AudioClip> attAudioList;
    public int attTimeSpan;         //ATT�^�[�Q�b�g�̕\������
    public int attHidTimeSpan;    //ATT�^�[�Q�b�g�̔�\������
    private int nextShowAttTime;    //���Ƀ^�[�Q�b�g������鎞��
    private int nextHidAttTime;     //���Ƀ^�[�Q�b�g���B��鎞��

    internal AudioSource audioSource;
    private int attNum;
    internal int nowAttCount;
    internal bool playAttAudio;
    internal bool showAttTarget;

    internal List<int> SUDSScores;

    internal int status;

    internal int second; //�J�E���g�_�E���̎��ԁi�b�j
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
        presenMonitor.SetText("��ʂ��N���b�N�����ăX�^�[�g");
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

    /// <summary>
    /// ATT�̃^�[�Q�b�g����
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
    /// Att�̎��ԊǗ�
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
