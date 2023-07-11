using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI Text1;
    [SerializeField] public TextMeshProUGUI Text2;
    [SerializeField] public TextMeshProUGUI Text3;
    [SerializeField] public TextMeshProUGUI Text4;
    [SerializeField] public TextMeshProUGUI Text5;
    [SerializeField] public TextMeshProUGUI Text6;
    [SerializeField] public TextMeshProUGUI Text7;
    [SerializeField] public TextMeshProUGUI Text8;
    [SerializeField] public TextMeshProUGUI Text9;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float smallPause;
    [SerializeField] private float BigPause = 2f;
    private Queue<(string, float)> textsToWrite = new Queue<(string, float)>();
    // ������ Singleton ��� ���� ����� ����� ��������� � CutsceneManager ����� CutsceneManager.Instance.�����������������������������()
    public static CutsceneManager Instance;

    // ���� �� �������� �������, � ������� ���� Key � Value ������� � ���������� ����� ���������� � Dictionary "cutsceneDataBase"
    // ������ �� ��� ������ ��� ���� ��������� Dictionary �� ������������ � ����������
    [SerializeField] private List<CutsceneStruct> cutscenes = new List<CutsceneStruct>();

    // ���� ������ ���������� ��� �������� ������� �� ����� ���������, � � ���������� ����� ���������� ���� �������� �� ������
    // ��� ��� ��� Dictionary ��������� � ��������� �� ����� ��������� � ���� �� ������ ������� ��� ��� - CutsceneManager.cutsceneDataBase["���� ������ ��������"]
    public static Dictionary<string, GameObject> cutsceneDataBase = new Dictionary<string, GameObject>();

    // ������ � ���� �������� ������� ������������� � ������� ������, ���� �� ����� �������� ������ �� ������������� - ��� ����� null
    public static GameObject activeCutscene;

    private void Awake()
    {
        // ������ Singleton
        Instance = this;

        // �������� ����� ������������� ���� ������ � ����������
        InitializeCutsceneDataBase();

        // ���������� �� ���� ��������� � ��������� �� (����� ��� ������� ���� �� ����������� ��������)
        foreach (var cutscene in cutsceneDataBase)
        {
            cutscene.Value.SetActive(false);
        }
    }

    // ����� � ������� �� ��������� Dictionary cutsceneDataBase
    private void InitializeCutsceneDataBase()
    {
        // ����� ����������� �� ������ ������ ������� ���� ���� ������
        cutsceneDataBase.Clear();

        // ��������� cutsceneDataBase ������� � ���������� ������� �� ������ � ����� cutscenes
        for (int i = 0; i < cutscenes.Count; i++)
        {           
            cutsceneDataBase.Add(cutscenes[i].cutsceneKey, cutscenes[i].cutsceneObject);
        }
    }

    // ����� ��� ��������� �������� �� �����
    public void StartCutscene(string cutsceneKey)
    {
        // ���� cutsceneDataBase �� ������� �������� � cutsceneKey �� ��������� �� ���� � ������� � �� ��������� ���� ��������� �����
        if (!cutsceneDataBase.ContainsKey(cutsceneKey)) 
        {
            Debug.LogError($"�������� c ������ \"{cutsceneKey}\" ���� � cutsceneDataBase");
            return;
        } 

        // ���� ������ ������������� �������� � �� �������� ������� � ���� ������ Ũ �� �� ������ �������� ���������� ������
        if (activeCutscene != null)
        {
            if (activeCutscene == cutsceneDataBase[cutsceneKey])
            {
                return;
            }
        }

        // ����������� �������� ��������
        activeCutscene = cutsceneDataBase[cutsceneKey];

        // ��������� ��� ��������
        foreach (var cutscene in cutsceneDataBase)
        {
            cutscene.Value.SetActive(false);
        }

        // �������� �� �������� ������� ����� �������
        cutsceneDataBase[cutsceneKey].SetActive(true);
    }

    // ����� ������� ��������� ������� ��������
    public void EndCutscene()
    {
        if (activeCutscene != null)
        {
            activeCutscene.SetActive(false);
            activeCutscene = null;
        }
    }

    public void WriteText1()
    {
        textsToWrite.Enqueue(("� ����������� ������ ������,", smallPause));
        textsToWrite.Enqueue((" ���\n���� �������� �����������\n������� ���������� ������\n���������� ����,",smallPause));
        textsToWrite.Enqueue((" ��������\n��������� ����� ����� ���\n�������.\n\n",BigPause));

        //textsToWrite.Enqueue(("������ ������ �� ���,\n����������� �������,", smallPause));
        //textsToWrite.Enqueue((" ������ ���� �� ���������-������ ���������,",smallPause)); 
        //textsToWrite.Enqueue((" ��� � ������������� �����\n�������, ����������� �� ���.\n\n", BigPause));
        
        textsToWrite.Enqueue(("����� ���� �������� ����������\n������� ��� ���� ���������\n����� �� ����� �����,", smallPause));
        textsToWrite.Enqueue((" ��� ������� ������ ���������� ��������������� ����� ����.",BigPause));
        
        StartCoroutine(WriteTexts(Text1));
    }
    public void WriteText2()
    {
        textsToWrite.Enqueue(("��� ���� �����,", smallPause));
        textsToWrite.Enqueue((" ������� ����� ���������� ����� ���� ����� ��������� �������.\n\n",BigPause));

        textsToWrite.Enqueue(("�� ��� �������� ����������� ��� ��������,", smallPause));
        textsToWrite.Enqueue((" � �������� ������ ����, �������� �� �������� ��� ��� ������� �������.\n\n", BigPause));

        //textsToWrite.Enqueue(("������ ���� ������� ��� ��� ���� ��������.", smallPause));
        //textsToWrite.Enqueue((" ������ �����,", smallPause));
        //textsToWrite.Enqueue((" ����� ����� ��� ������,", smallPause));
        //textsToWrite.Enqueue((" ��������� ����", smallPause));
        //textsToWrite.Enqueue((" - ��� ��� ����������� ������� �������� �����, ������� �� �����������.\n\n", BigPause));

        //textsToWrite.Enqueue(("��� ���� ��� ���� ����,", smallPause));
        //textsToWrite.Enqueue((" ��������� ���,", smallPause));
        //textsToWrite.Enqueue((" ������� �� ��������� ������.",BigPause));
        
        StartCoroutine(WriteTexts(Text2));
    }
    
    public void WriteText3()
    {
        textsToWrite.Enqueue(("������� �����,", smallPause));
        textsToWrite.Enqueue((" � �� ����� ��� ��� ����� ���������� ������ � ������,", smallPause));
        textsToWrite.Enqueue((" ������� ���� �������� �� ������.\n\n",BigPause));

        //textsToWrite.Enqueue(("��� ��� ������.", BigPause));
        //textsToWrite.Enqueue((" ������������ ������� ��������� ������,", smallPause));
        //textsToWrite.Enqueue((" �������, ��������, �������� � �������.\n\n",BigPause));

        textsToWrite.Enqueue(("����� ����� ������� � ������ �� ���,", smallPause));
        textsToWrite.Enqueue((" ������� ������� ���,", smallPause));
        textsToWrite.Enqueue((" �� ������ ������ ��������� �� ��� ���.", BigPause));
        //textsToWrite.Enqueue((" ��� ��� ����� ���� ����.\n\n", BigPause)); , ������ ����,

        //textsToWrite.Enqueue(("��� ������ ��������� ��� ������ �������� � ���������,", smallPause));
        //textsToWrite.Enqueue((" � ��,", smallPause));
        //textsToWrite.Enqueue((" �� ������� �������� �� ���������� ���,",smallPause));
        //textsToWrite.Enqueue((" � ������� �������� � ���.\n\n",BigPause));
        
        StartCoroutine(WriteTexts(Text3));
    }
    
    public void WriteText4()
    {
        textsToWrite.Enqueue(("������ ������� ����� ��� ������ � ������ �� ����,", smallPause));
        textsToWrite.Enqueue((" ������ ��������� �� ��� �������.\n\n", BigPause));

        textsToWrite.Enqueue((" �� ����� ������,", smallPause));
        textsToWrite.Enqueue((" ����� � ��������,", smallPause));
        textsToWrite.Enqueue((" ��������� �������� ����, �������� ������ ��������������.", smallPause));
        textsToWrite.Enqueue((" ���� ��������� � ������� ����,", smallPause));
        textsToWrite.Enqueue((" ������ ����� ����������� ������,", smallPause));
        textsToWrite.Enqueue((" �������� ����� ������������.\n\n",BigPause));
        
        
        textsToWrite.Enqueue(("�� �������� ������� ��������� ������ �� ���� �� ����.", smallPause));
        textsToWrite.Enqueue((" ������ ��� ������� �� ������ � ���������,", smallPause));
        textsToWrite.Enqueue((" � ��������� ����� ����� �����.\n\n",BigPause));

        StartCoroutine(WriteTexts(Text4));
    }
    
    public void WriteText5()
    {
        textsToWrite.Enqueue(("�����, ����������� �� ���� ������� ������,", smallPause));
        textsToWrite.Enqueue((" � ����� ������ ��������.", smallPause));
        textsToWrite.Enqueue((" ��� ���� �������, �������� �����,", smallPause));
        textsToWrite.Enqueue(("  ���������� ����� � ������ ���� � ����, ���������� � ������.\n\n",BigPause));

        textsToWrite.Enqueue(("��� ������ �� ������������� �����,", smallPause));
        textsToWrite.Enqueue((" ������� ���� ������� ������ ������ ������, ����� �������� ������������� ������,", smallPause));
        textsToWrite.Enqueue((" ���� ���������� ������ ������.", smallPause));
        textsToWrite.Enqueue((" ����� ����� �� ����� �� ����,", smallPause));
        textsToWrite.Enqueue((" ��� ���, ���� ������� � ������ � ������� �����.\n\n",BigPause));


        textsToWrite.Enqueue(("� ��� ��������� ������ �������� ���������� �������� �������.", smallPause));
        textsToWrite.Enqueue((" �� ����� �����, ��� ��� ���, ������� ��� ������ ����� ������� ��� ����� ���������� � ��������,", smallPause));
        textsToWrite.Enqueue((" � ���� ��������� ��� ��������.\n\n",BigPause));

        StartCoroutine(WriteTexts(Text5));
    }
    
    public void WriteText6()
    {
        textsToWrite.Enqueue(("� ��� ������ ��������� ����� � ����,", smallPause));
        textsToWrite.Enqueue((" ������ ������ � ������� �����, ����� �� ������� �� ���� ����� ����� � �����.\n\n",BigPause));

        textsToWrite.Enqueue(("�� ��� ������ ������ � ��������, �� ���� ����� ������.", smallPause));
        textsToWrite.Enqueue((" ���, ��� �� ���� � �����, ���� ������� � ���� ����.\n\n",BigPause));


        textsToWrite.Enqueue(("����� ���� ���������������, � �������� ����� ����� ��������, ������� �� ����� ������ �������,", smallPause));
        textsToWrite.Enqueue((" ����� ��� ��� ����� �� ��� �� �����.", smallPause));
        textsToWrite.Enqueue((" �� ������ �������� ������� � ����� ������,", smallPause));
        textsToWrite.Enqueue((" � ��� ��������� ���� ������� �� ������ � ������.\n\n",BigPause));

        StartCoroutine(WriteTexts(Text6));
    }
    
    public void WriteText7()
    {
        textsToWrite.Enqueue(("���������� � ���, ����� ��������� ������ �������-����.", smallPause));
        textsToWrite.Enqueue((" ��������, �� ������� ������, ��� ��� �������� �������.", smallPause));
        textsToWrite.Enqueue((" ����������� ����������� � ���� �����, ������� ������� �������� � ����� ���� �������:\n\n",BigPause));

        textsToWrite.Enqueue(("\"���� �����, ��� �������, ������ � �������� ���\",", smallPause));
        textsToWrite.Enqueue((" - ������� �������� ��, ����� �� ������������� �����.\n\n",BigPause));
        
        textsToWrite.Enqueue(("\"��� �����, ��� ����� ����, ����� ������� � ������� ��� �� ����, ���� � ����.", smallPause));
        textsToWrite.Enqueue((" �� �� ������ ����� � �������� ����, ��� �������� �� ��� ����������.\"\n\n",BigPause));
        
        textsToWrite.Enqueue(("������� ����������� ����� � ���, ����� ��������� � ��������� ����� ��������� ���, ���� �� ����� ������ ���� �����.\n\n",BigPause));

        StartCoroutine(WriteTexts(Text7));
    }
    public void WriteText8()
    {
        textsToWrite.Enqueue(("� ���������� �� � �����������...",BigPause));
        StartCoroutine(WriteTexts(Text8));
    }
    
    public void WriteText9()
    {
        textsToWrite.Enqueue(("� ������ ���� ��������!",BigPause));
        StartCoroutine(WriteTexts(Text9));
    }

    
    IEnumerator WriteTexts(TextMeshProUGUI text)
    {
        while (textsToWrite.Count > 0)
        {
            var tuple = textsToWrite.Dequeue();
            yield return StartCoroutine(WriteText(tuple.Item1, text, tuple.Item2));
        }
    }
    
    IEnumerator WriteText(string textToWrite, TextMeshProUGUI text, float wait)
    {
        foreach (char c in textToWrite)
        {
            text.text += c;
            yield return new WaitForSeconds(speed); 
        }
        yield return new WaitForSeconds(wait); 
    }
    public void NextScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}

// ��������� ������� ��� �����, ����� ����� ����������� ��� �������� � Key � Value � Dictionary cutsceneDataBase
[System.Serializable]
public struct CutsceneStruct
{
    public string cutsceneKey;
    public GameObject cutsceneObject;
}