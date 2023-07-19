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
    // Создаём Singleton для того чтобы легко обращатся к CutsceneManager через CutsceneManager.Instance.ПубличныйМетодКоторыйВамНужен()
    public static CutsceneManager Instance;

    // Лист из структур катсцен, в которых есть Key и Value которые в дальнейшем будут заполнятся в Dictionary "cutsceneDataBase"
    // Делаем мы это потому что даже публичный Dictionary не отображается в инспекторе
    [SerializeField] private List<CutsceneStruct> cutscenes = new List<CutsceneStruct>();

    // База данных содержащая все катсцены которые мы будем создавать, и в дальнейшем можем вытягивать наши катсцены по ключам
    // Так как наш Dictionary публичный и статичный мы можем обращатся к нему из любого скрипта вот так - CutsceneManager.cutsceneDataBase["Ключ нужной катсцены"]
    public static Dictionary<string, GameObject> cutsceneDataBase = new Dictionary<string, GameObject>();

    // Хранит в себе катсцену которая проигрывается в текущий момент, если ни одной катсцены сейчас не проигрывается - она равна null
    public static GameObject activeCutscene;

    private void Awake()
    {
        // Создаём Singleton
        Instance = this;

        // Вызываем метод инициализации базы данных с катсценами
        InitializeCutsceneDataBase();

        // Проходимся по всем катсценам и выключаем их (чтобы при запуске игры не запускались катсцены)
        foreach (var cutscene in cutsceneDataBase)
        {
            cutscene.Value.SetActive(false);
        }
    }

    // Метод в котором мы заполняем Dictionary cutsceneDataBase
    private void InitializeCutsceneDataBase()
    {
        // Перед заполнением на всякий случай очищаем нашу базу данных
        cutsceneDataBase.Clear();

        // Заполняем cutsceneDataBase ключами и значениями которые мы укажем в листе cutscenes
        for (int i = 0; i < cutscenes.Count; i++)
        {           
            cutsceneDataBase.Add(cutscenes[i].cutsceneKey, cutscenes[i].cutsceneObject);
        }
    }

    // Метод для включения катсцены по ключу
    public void StartCutscene(string cutsceneKey)
    {
        // Если cutsceneDataBase не содежит катсцены с cutsceneKey то упоминаем об этом в консоли и не выполняем весь остальной метод
        if (!cutsceneDataBase.ContainsKey(cutsceneKey)) 
        {
            Debug.LogError($"Катсцены c ключом \"{cutsceneKey}\" нету в cutsceneDataBase");
            return;
        } 

        // Если сейчас проигрывается катсцена и мы пытаемся вызвать в этот момент ЕЁ ЖЕ то просто обрываем выполнение метода
        if (activeCutscene != null)
        {
            if (activeCutscene == cutsceneDataBase[cutsceneKey])
            {
                return;
            }
        }

        // Присваиваем активную катсцену
        activeCutscene = cutsceneDataBase[cutsceneKey];

        // Выключаем все катсцены
        foreach (var cutscene in cutsceneDataBase)
        {
            cutscene.Value.SetActive(false);
        }

        // Включаем ту катсцену которую хотим вызвать
        cutsceneDataBase[cutsceneKey].SetActive(true);
    }

    // Метод который выключает текущую катсцену
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
        textsToWrite.Enqueue(("В бесконечном океане песков,", smallPause));
        textsToWrite.Enqueue((" где\nдюны величаво возвышаются\nподобно бескрайним волнам\nрыжеватого моря,",smallPause));
        textsToWrite.Enqueue((" скромное\nсемейство котов нашло своё\nубежище.\n\n",BigPause));

        //textsToWrite.Enqueue(("Тонкая шерсть их тел,\nсогреваемых солнцем,", smallPause));
        //textsToWrite.Enqueue((" играла теми же золотисто-рыжими оттенками,",smallPause)); 
        //textsToWrite.Enqueue((" что и блистательные пески\nпустыни, окутывающие их дом.\n\n", BigPause));
        
        textsToWrite.Enqueue(("Среди этих пушистых обитателей\nпустыни был один маленький\nкотик по имени Томми,", smallPause));
        textsToWrite.Enqueue((" чья история\nтолько разворачивается перед нами.",BigPause));
        
        StartCoroutine(WriteTexts(Text1));
    }
    public void WriteText2()
    {
        textsToWrite.Enqueue(("Это было место,", smallPause));
        textsToWrite.Enqueue((" которое Томми\nнесомненно любил всем своим\nмаленьким сердцем.\n\n",BigPause));

        textsToWrite.Enqueue(("Он был поглощен окружающими его\nчудесами,", smallPause));
        textsToWrite.Enqueue((" и проводил долгие часы,\nнаблюдая за парящими над его головой птицами.\n\n", BigPause));

        //textsToWrite.Enqueue(("Каждый звук природы был для него мелодией.", smallPause));
        //textsToWrite.Enqueue((" Шелест ветра,", smallPause));
        //textsToWrite.Enqueue((" шорох песка под лапами,", smallPause));
        //textsToWrite.Enqueue((" щебетание птиц", smallPause));
        //textsToWrite.Enqueue((" - все это формировало великую симфонию жизни, которой он наслаждался.\n\n", BigPause));

        //textsToWrite.Enqueue(("Это было для него чудо,", smallPause));
        //textsToWrite.Enqueue((" волшебный мир,", smallPause));
        //textsToWrite.Enqueue((" который он стремился понять.",BigPause));
        
        StartCoroutine(WriteTexts(Text2));
    }
    
    public void WriteText3()
    {
        textsToWrite.Enqueue(("Однажды,", smallPause));
        textsToWrite.Enqueue((" Томми обратил свое внимание на подхваченный ветром листик,", smallPause));
        textsToWrite.Enqueue((" который\nтанцевал в воздухе.\n\n",BigPause));

        //textsToWrite.Enqueue(("Это был листик.", BigPause));
        //textsToWrite.Enqueue((" подхваченный игривым пустынным ветром,", smallPause));
        //textsToWrite.Enqueue((" который, казалось, танцевал в воздухе.\n\n",BigPause));

        textsToWrite.Enqueue(("Он начал прыгать и бегать за ним,", smallPause));
        textsToWrite.Enqueue((" но листик всегда ускользал от его лап,", smallPause));
        textsToWrite.Enqueue((" словно зная, что это всего лишь игра.", BigPause));
        //textsToWrite.Enqueue((" что это всего лишь игра.\n\n", BigPause)); , словно зная,

        //textsToWrite.Enqueue(("Эта забава наполнила его сердце радостью и восторгом,", smallPause));
        //textsToWrite.Enqueue((" и он,", smallPause));
        //textsToWrite.Enqueue((" не обращая внимания на окружающий мир,",smallPause));
        //textsToWrite.Enqueue((" с головой окунулся в нее.\n\n",BigPause));
        
        StartCoroutine(WriteTexts(Text3));
    }
    
    public void WriteText4()
    {
        textsToWrite.Enqueue(("Листок увлекал Томми все дальше и\nдальше от дома,", smallPause));
        textsToWrite.Enqueue((" пока небо не разорвалось громким звуком.\n\n", BigPause));

        textsToWrite.Enqueue(("Молния, яркая и страшная прорезала\nнебесный свод,", smallPause));
        textsToWrite.Enqueue((" наполнив воздух\nэлектричеством.\n\n", BigPause));
        textsToWrite.Enqueue(("Томми замер, ", smallPause));
        textsToWrite.Enqueue((" внезапно осознав,", smallPause));
        textsToWrite.Enqueue((" насколько далеко он ушел от дома.\n\n",BigPause));
        
        
        //textsToWrite.Enqueue(("Он внезапно осознал насколько далеко он ушел от дома.", smallPause));
        //textsToWrite.Enqueue((" Сердце его замерло от страха и изумления,", smallPause));
        //textsToWrite.Enqueue((" а маленький хвост встал дыбом.\n\n",BigPause));

        StartCoroutine(WriteTexts(Text4));
    }
    
    public void WriteText5()
    {
        textsToWrite.Enqueue(("Томми обернулся на звук громкой молнии.", smallPause));
        textsToWrite.Enqueue(("\nЭто было торнадо,", smallPause));
        textsToWrite.Enqueue((" взметавшее песок и облака пыли в небо, исчезающие в высоте.\n\n",BigPause));

        textsToWrite.Enqueue(("Его ничего не подозревающая семья была\nподхвачена мощным вихрем,", smallPause));
        textsToWrite.Enqueue((" поднята в воздух и унесена прочь.\n\n",BigPause));

        
        textsToWrite.Enqueue(("Мир, который еще минуту назад казался ему таким безопасным и знакомым,", smallPause));
        textsToWrite.Enqueue((" в одно\nмгновение был разрушен.\n\n",BigPause));

        StartCoroutine(WriteTexts(Text5));
    }
    
    public void WriteText6()
    {
        textsToWrite.Enqueue(("В его глазах отражался страх и ужас,", smallPause));
        textsToWrite.Enqueue((" и сердце билось в бешеном ритме.\n\n",BigPause));

        textsToWrite.Enqueue(("Это было как злой сон,", smallPause));
        textsToWrite.Enqueue((" слишком ужасный, чтобы быть реальностью.", smallPause));
        textsToWrite.Enqueue((" Он не верил\nсвоим глазам.", smallPause));
        textsToWrite.Enqueue((" Все, что он знал и любил,\nбыло унесено в этой буре.\n\n",BigPause));


        //textsToWrite.Enqueue(("Когда буря утихомиривалась, а песчаный вихрь начал исчезать, оставив за собой только пустыню,", smallPause));
        //textsToWrite.Enqueue((" Томми все еще стоял на том же месте.", smallPause));
        //textsToWrite.Enqueue((" Он ощущал огромную пустоту в своем сердце,", smallPause));
        //textsToWrite.Enqueue((" и его маленькое тело дрожало от страха и холода.\n\n",BigPause));

        StartCoroutine(WriteTexts(Text6));
    }
    
    public void WriteText7()
    {
        textsToWrite.Enqueue(("Ворвавшись в дом, Томми обнаружил\nсвоего дедушку-кота.\n\n", BigPause));

        textsToWrite.Enqueue(("\"Твою семью, мой мальчик, унесло в\nоблачный мир\",", smallPause));
        textsToWrite.Enqueue((" - грустно произнес он, глядя на ошеломленного Томми.\n\n",BigPause));
        
        textsToWrite.Enqueue(("\"Это место, где живет Брут, самый злобный и опасный пес из всех, кого я знал.", smallPause));
        textsToWrite.Enqueue((" Он не\nтерпит котов и угрожает всем, кто попадает на его территорию.\"\n\n",BigPause));

        StartCoroutine(WriteTexts(Text7));
    }
    public void WriteText8()
    {
        textsToWrite.Enqueue(("И он отправился на поиски своей семьи...",BigPause));
        StartCoroutine(WriteTexts(Text8));
    }
    
    public void WriteText9()
    {
        textsToWrite.Enqueue(("В добрый путь приятель!",BigPause));
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



// Структура катсцен для листа, чтобы потом присваивать эти значения к Key и Value в Dictionary cutsceneDataBase
[System.Serializable]
public struct CutsceneStruct
{
    public string cutsceneKey;
    public GameObject cutsceneObject;
}