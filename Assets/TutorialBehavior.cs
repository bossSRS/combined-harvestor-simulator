using CHS.Tutorial;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialBehavior : MonoBehaviour
{
    [SerializeField]
    private TMP_Text Message;
    [SerializeField]
    private Image TickMark;
    [ShowInInspector]
    public bool isCompleted;
    public UnityEvent PlayAudio;
    public UnityEvent TutorialComplete;

    public AudioClip clue;
    private TutorialManager tutorialManager;

    private void Start()
    {
        tutorialManager = FindAnyObjectByType<TutorialManager>();
        PlayAudio.AddListener(OnPlayedAudio);
    }
    // Start is called before the first frame update
    public void OnEnable()
    {
        Message = GetComponent<TMP_Text>();
        TickMark = GetComponentInChildren<Image>();
        TickMark.enabled = false;
    }
    [Button]
    public void OnTutorialCompleted()
    {
        isCompleted = true;
        TickMark.gameObject.transform.localScale = Vector3.one * 1.5f;
        TickMark.enabled = true;
        TickMark.transform.DOScale(Vector3.one, 0.75f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Message.color = Color.green;
        });
        StopAudio();
    }
    [Button]
    public void OnPlayedAudio()
    {
        if(!tutorialManager) tutorialManager = FindAnyObjectByType<TutorialManager>();
        tutorialManager.clueAudio.Stop();
        tutorialManager.clueAudio.clip = clue;
        tutorialManager.clueAudio.loop = true;
        tutorialManager.clueAudio.Play();
    }
    [Button]
    public void StopAudio()
    {
        tutorialManager.clueAudio.Stop();
    }
}
