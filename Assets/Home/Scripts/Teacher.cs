using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Teacher : MonoBehaviour
{
    [SerializeField] private Boosts _boosts;
    [SerializeField] private GameObject cat;
    [SerializeField] private CatSprite catSprite;
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private TextMeshProUGUI textIcon;
    [SerializeField] private Image textBox;
    [SerializeField] private float typingSpeed;
    [SerializeField] private TextMeshProUGUI teacherText;
    [SerializeField] private TextMeshProUGUI helpText;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 movingVector;
    [SerializeField] private Vector2 jumpVector;
    [SerializeField] private GameObject knifeTarget;
    [SerializeField] private GameObject clawsTarget;
    [SerializeField] private LayerMask knifeLayer;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private Hyena hyenaForKill;
    [SerializeField] private Hyena hyenaForWind;
    [TextArea(3, 10)]
    [SerializeField] private string[] textForTeacher;
    private Animator _animator;
    private bool _isHook;
    private bool _end;
    private bool _isHello;
    private bool _isClawsAttack;
    private bool _stopTyping;
    private bool _useHookHint;
    private bool _useKnifeHint;
    private bool _useClawsHint;
    private bool _useKillAndExperienceHint;
    private bool _useWindHint;
    private bool _useEndHint;
    private bool _wellDone;
    private bool _knifeTraining;
    private bool _clawsTraining;
    private bool _killAndExperienceTraining;
    private bool _windTraining;
    private bool startWindTraining;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _attackColliders;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _isHello = true;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (_end)
        {
            if (transform.position.x <= 40f) transform.position += speed * 2 * Time.deltaTime * movingVector;
            else _animator.SetInteger("state", 0);
            if (Input.GetKeyDown(KeyCode.E) && _useEndHint) _stopTyping = true;
            EndTraining();
            if (Input.GetKeyDown(KeyCode.Return)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (_windTraining)
        {
            if (!startWindTraining)
            {
                _boosts.energyCount = 5;
                _boosts.boostsText[0].text = "x" + 5;
                _boosts.fishCount = 5;
                _boosts.boostsText[1].text = "x" + 5;
                _boosts.waterCount = 5;
                _boosts.boostsText[2].text = "x" + 5;
                startWindTraining = true;
            }
            if (transform.position.x <= 33f) transform.position += speed * 2 * Time.deltaTime * movingVector;
            else _animator.SetInteger("state", 0);
            if (catSprite.XP < 10)
            {
                catSprite.XP = 10;
                catSprite.greenBar.SetHealth(10);
            }
            if (Input.GetKeyDown(KeyCode.E) && _useWindHint) _stopTyping = true;
            if (Input.GetKeyDown(KeyCode.Return) && _useWindHint)
            {
                StopAllCoroutines();
                _wellDone = false;
                _stopTyping = false;
                StartCoroutine(TypeSentence(textForTeacher[5], teacherText));
            }
            WindTraining();
            if (hyenaForWind.stan)
            {
                _end = true;
                StopAllCoroutines();
                textBox.enabled = false;
                helpText.enabled = false;
                teacherText.text = "";
                _animator.SetInteger("state", 2);
                helpText.text = "E - пропустить/дальше";
            }
        }
        else if (_killAndExperienceTraining)
        {
            if (transform.position.x <= 26f) transform.position += speed * 2 * Time.deltaTime * movingVector;
            else _animator.SetInteger("state", 0);
            if (Input.GetKeyDown(KeyCode.E) && _useKillAndExperienceHint) _stopTyping = true;
            if (Input.GetKeyDown(KeyCode.Return) && _useKillAndExperienceHint)
            {
                StopAllCoroutines();
                _wellDone = false;
                _stopTyping = false;
                StartCoroutine(TypeSentence(textForTeacher[4], teacherText));
            }
            KillAndExpperienceTraining();
            if (hyenaForKill.stateHyena == Hyena.MovementState.death)
            {
                _windTraining = true;
                StopAllCoroutines();
                textBox.enabled = false;
                helpText.enabled = false;
                teacherText.text = "";
                _animator.SetInteger("state", 2);
            }
        }
        else if (_clawsTraining)
        {
            if (transform.position.x <= 21f) transform.position += speed * 2 * Time.deltaTime * movingVector;
            else _animator.SetInteger("state", 0);
            if (Input.GetKeyDown(KeyCode.E) && _useClawsHint) _stopTyping = true;
            if (Input.GetKeyDown(KeyCode.Return) && _useClawsHint)
            {
                StopAllCoroutines();
                _wellDone = false;
                _stopTyping = false;
                StartCoroutine(TypeSentence(textForTeacher[3], teacherText));
            }
            ClawsTraining();
            _attackColliders = Physics2D.OverlapCircle(clawsTarget.transform.position, 0.1f, catLayer);
            if (_attackColliders && Input.GetKeyDown(KeyCode.W))
            {
                _killAndExperienceTraining = true;
                StopAllCoroutines();
                textBox.enabled = false;
                helpText.enabled = false;
                teacherText.text = "";
                _animator.SetInteger("state", 2);
            }
        }
        else if (_knifeTraining)
        {
            if (transform.position.x <= 12f) transform.position += speed * Time.deltaTime * movingVector;
            else _animator.SetInteger("state", 0);
            if (Input.GetKeyDown(KeyCode.E) && _useKnifeHint) _stopTyping = true;
            if (Input.GetKeyDown(KeyCode.Return) && _useKnifeHint)
            {
                StopAllCoroutines();
                _wellDone = false;
                _stopTyping = false;
                StartCoroutine(TypeSentence(textForTeacher[2], teacherText));
            }
            KnifeTraining();
            _attackColliders = Physics2D.OverlapCircle(knifeTarget.transform.position + new Vector3(-0.07f, 0.12f, 0f), 0.1f, knifeLayer);
            if (_attackColliders)
            {
                _clawsTraining = true;
                StopAllCoroutines();
                textBox.enabled = false;
                helpText.enabled = false;
                teacherText.text = "";
                _animator.SetInteger("state", 2);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return) && _useHookHint)
            {
                StopAllCoroutines();
                _isHello = true;
                _isHook = false;
                _stopTyping = false;
                StartCoroutine(TypeSentence(textForTeacher[0], teacherText));
            }
            if (Input.GetKeyDown(KeyCode.E) && _useHookHint)
            {
                if (_wellDone && _isHello)
                {
                    _wellDone = false;
                    _isHello = !_isHello;
                    _isHook = !_isHook;
                }
                else _stopTyping = true;
            }
            if (_isHook)
            {
                StartCoroutine(TypeSentence(textForTeacher[1], teacherText));
                _isHook = false;
            }
            else if (_isHello) JumpTraining();
            if (catSprite.isGround && cat.transform.position.x > 8)
            {
                _knifeTraining = true;
                StopAllCoroutines();
                textBox.enabled = false;
                teacherText.text = "";
                helpText.enabled = false;
                _animator.SetInteger("state", 3);
                _rigidbody2D.AddForce(jumpVector, ForceMode2D.Impulse);
            }
        }
    }

    private void JumpTraining()
    {
        if (!_useHookHint)
        {
            if (Vector3.Distance(cat.transform.position, transform.position) < 1f)
            {
                icon.enabled = true;
                textIcon.enabled = true;
            }
            else
            {
                icon.enabled = false;
                textIcon.enabled = false;
            }
        }
        if (icon.enabled && Input.GetKeyDown(KeyCode.E))
        {
            _useHookHint = true;
            icon.enabled = false;
            textIcon.enabled = false;
            textBox.enabled = true;
            helpText.enabled = true;
            StartCoroutine(TypeSentence(textForTeacher[0], teacherText));
        }
    }
    
    IEnumerator TypeSentence(string sentence, TextMeshProUGUI textComponent)
    {
        _wellDone = false;
        _stopTyping = false;
        textComponent.text = "";
        foreach (var letter in sentence)
        {
            textComponent.text += letter;
            if (_stopTyping)
            {
                textComponent.text = sentence;
                _stopTyping = false;
                break;
            }
            yield return new WaitForSeconds(typingSpeed);
        }
        _wellDone = true;
    }

    private void KnifeTraining()
    {
        if (!_useKnifeHint)
        {
            if (Vector3.Distance(cat.transform.position, transform.position) < 1f && _animator.GetInteger("state") == 0)
            {
                icon.enabled = true;
                textIcon.enabled = true;
            }
            else
            {
                icon.enabled = false;
                textIcon.enabled = false;
            }
        }
        if (icon.enabled && Input.GetKeyDown(KeyCode.E))
        {
            _useKnifeHint = true;
            icon.enabled = false;
            textIcon.enabled = false;
            textBox.enabled = true;
            helpText.enabled = true;
            StartCoroutine(TypeSentence(textForTeacher[2], teacherText));
        }
    }

    private void ClawsTraining()
    {
        if (!_useClawsHint)
        {
            if (Vector3.Distance(cat.transform.position, transform.position) < 1f && _animator.GetInteger("state") == 0)
            {
                icon.enabled = true;
                textIcon.enabled = true;
            }
            else
            {
                icon.enabled = false;
                textIcon.enabled = false;
            }
        }
        if (icon.enabled && Input.GetKeyDown(KeyCode.E))
        {
            _useClawsHint = true;
            icon.enabled = false;
            textIcon.enabled = false;
            textBox.enabled = true;
            helpText.enabled = true;
            StartCoroutine(TypeSentence(textForTeacher[3], teacherText));
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(knifeTarget.transform.position + new Vector3(-0.07f, 0.12f, 0f), 0.1f);
    }
    
    private void KillAndExpperienceTraining()
    {
        if (!_useKillAndExperienceHint)
        {
            if (Vector3.Distance(cat.transform.position, transform.position) < 1f && _animator.GetInteger("state") == 0)
            {
                icon.enabled = true;
                textIcon.enabled = true;
            }
            else
            {
                icon.enabled = false;
                textIcon.enabled = false;
            }
        }
        if (icon.enabled && Input.GetKeyDown(KeyCode.E))
        {
            _useKillAndExperienceHint = true;
            icon.enabled = false;
            textIcon.enabled = false;
            textBox.enabled = true;
            helpText.enabled = true;
            StartCoroutine(TypeSentence(textForTeacher[4], teacherText));
        }
    }
    
    private void WindTraining()
    {
        if (!_useWindHint)
        {
            if (Vector3.Distance(cat.transform.position, transform.position) < 1f && _animator.GetInteger("state") == 0)
            {
                icon.enabled = true;
                textIcon.enabled = true;
            }
            else
            {
                icon.enabled = false;
                textIcon.enabled = false;
            }
        }
        if (icon.enabled && Input.GetKeyDown(KeyCode.E))
        {
            _useWindHint = true;
            icon.enabled = false;
            textIcon.enabled = false;
            textBox.enabled = true;
            helpText.enabled = true;
            StartCoroutine(TypeSentence(textForTeacher[5], teacherText));
        }
    }
    
    private void EndTraining()
    {
        if (!_useEndHint)
        {
            if (Vector3.Distance(cat.transform.position, transform.position) < 1f && _animator.GetInteger("state") == 0)
            {
                icon.enabled = true;
                textIcon.enabled = true;
            }
            else
            {
                icon.enabled = false;
                textIcon.enabled = false;
            }
        }
        if (icon.enabled && Input.GetKeyDown(KeyCode.E))
        {
            _useEndHint = true;
            icon.enabled = false;
            textIcon.enabled = false;
            textBox.enabled = true;
            helpText.enabled = true;
            StartCoroutine(TypeSentence(textForTeacher[6], teacherText));
        }
    }
}
