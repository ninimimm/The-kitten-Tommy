using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButtons : MonoBehaviour
{
    [SerializeField] private int countButtons;
    [SerializeField] private string code;
    [SerializeField] public GameObject door;
    public StringBuilder keys = new ();
    public Button[] buttons;
    [SerializeField] private float timeToWait;
    public float timer;
    private ManageButtonsData _data;
    private Animator _doorAnimator;
    private BoxCollider2D _doorBoxCollider2D;
    private int index = -1;

    private void Start()
    {
        _data = SavingSystem<ManageButtons, ManageButtonsData>.Load($"{gameObject.name}.data");
        _doorAnimator = door.GetComponent<Animator>();
        _doorBoxCollider2D = door.GetComponent<BoxCollider2D>();
        if (SceneManager.GetActiveScene().name == "Jungle")
            Save();
        if (!MainMenu.dictSave.ContainsKey(gameObject.name))
        {
            MainMenu.dictSave.Add(gameObject.name,MainMenu.index);
            MainMenu.index ++;
        }
        if (MainMenu.isStarts[MainMenu.dictSave[gameObject.name]])
        {
            Save();
            MainMenu.isStarts[MainMenu.dictSave[gameObject.name]] = false;
        }
        Load();
    }

    void Update()
    {
        if (keys.Length == countButtons)
        {
            if (code.Contains(keys.ToString()))
            {
                _doorAnimator.SetBool("opened",true);
                _doorBoxCollider2D.enabled = false;
            }
            else if (timer < -0.4)
            {
                timer = timeToWait;
                foreach (var button in buttons)
                    button.isUnpressSoundPlayed = false;
                foreach (var button in buttons)
                {
                    button.boxCol1.enabled = true;
                    button.boxCol2.enabled = false;
                }
            }
        }
        
        if (timer < 0 && timer > -0.2)
        {
            foreach (var button in buttons)
                button.state = Button.MovementState.Stay;
            keys = new StringBuilder("");
        }
        timer -= Time.deltaTime;
    }
    
    public void Save()
    {
        if (this != null) 
            SavingSystem<ManageButtons,ManageButtonsData>.Save(this, $"{gameObject.name}.data");
    }


    public void Load()
    {
        _data = SavingSystem<ManageButtons, ManageButtonsData>.Load($"{gameObject.name}.data");
        _doorAnimator.SetBool("opened",_data.animatorState);
        _doorBoxCollider2D.enabled = _data.colliderState;
    }
}