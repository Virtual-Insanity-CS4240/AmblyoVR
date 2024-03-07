using Delegates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : SimpleSingleton<NPC> 
{
    [SerializeField] private TMP_Text m_NPCText;
    [SerializeField] private float m_SecondsBetweenDialogues = 3;
    private string[] m_DialogueLines = { "Test 1", "Test 2", "Test 3" };

    private bool m_IsInDialogue = false;

    public static VoidEvent StartDialogue;
    public static VoidEvent EndDialogue;

    private float m_Timer;

    protected override void HandleAwake()
    {
        m_Timer = 0;
        SingletonReady();
    }

    private void Update()
    {
        if (m_IsInDialogue)
        {
            m_Timer += Time.deltaTime;

            int index = Mathf.CeilToInt(m_Timer) / (int) m_SecondsBetweenDialogues;

            if (index >= m_DialogueLines.Length)
            {
                m_Timer = 0;
                index = 0;
            }

            m_NPCText.text = m_DialogueLines[index];
        }
    }

    private void OnEnable()
    {
        StartDialogue += HandleStartDialogue;
        EndDialogue += HandleEndDialogue;
    }

    private void OnDisable()
    {
        StartDialogue -= HandleStartDialogue;
        EndDialogue -= HandleEndDialogue;
    }

    private void HandleStartDialogue()
    {
        if (!m_IsInDialogue)
        {
            m_IsInDialogue = true;
            m_NPCText.gameObject.SetActive(true);
        }
    }

    private void HandleEndDialogue()
    {
        if (m_IsInDialogue)
        {
            m_IsInDialogue = false;            
            m_NPCText.text = "";
            m_NPCText.gameObject.SetActive(false);
            m_Timer = 0;
        }        
    }
}
