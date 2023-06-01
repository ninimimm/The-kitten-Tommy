using System.Text;
using UnityEngine;

public class ManageButtons : MonoBehaviour
{
    [SerializeField] public GameObject door;
    public StringBuilder keys = new ();
    public Button[] buttons;
    [SerializeField] private float timeToWait;
    public float timer;
    private ManageButtonsData _data;
    private Animator _doorAnimator;
    private BoxCollider2D _doorBoxCollider2D;

    private void Start()
    {
        _doorAnimator = door.GetComponent<Animator>();
        _doorBoxCollider2D = door.GetComponent<BoxCollider2D>();
        if (!ManageButtonsData.Start.Contains(gameObject.name))
        {
            ManageButtonsData.Start.Add(gameObject.name);
            Save();
        }
        Load();
    }

    void Update()
    {
        if (keys.Length == 5)
        {
            if (keys.ToString() == "41523")
            {
                _doorAnimator.SetBool("opened",true);
                _doorBoxCollider2D.enabled = false;
            }
            else if (timer < -0.4)
            {
                timer = timeToWait;
                foreach (var button in buttons)
                    button.isUnpressSoundPlayed = false;
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
        SavingSystem<ManageButtons,ManageButtonsData>.Save(this, $"{gameObject.name}.data");
    }


    public void Load()
    {
        _data = SavingSystem<ManageButtons, ManageButtonsData>.Load($"{gameObject.name}.data");
        _doorAnimator.SetBool("opened",_data.animatorState);
        _doorBoxCollider2D.enabled = _data.colliderState;
    }
}
