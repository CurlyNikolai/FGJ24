using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class CurlyTerminal2 : MonoBehaviour
{
    [SerializeField] private int maxHistory = 5;
    [SerializeField] private TextMeshProUGUI msgEntry;
    [SerializeField] private VerticalLayoutGroup msgHistory;
    [SerializeField] private TMP_InputField tmpInput;

    private RectTransform root;

    private void Awake()
    {
        root = GetComponent<RectTransform>();
        CommandAttribute.InitCommandMap();
        InitTerminalUI();
    }

    private void InitTerminalUI()
    {
        tmpInput.onSubmit.AddListener(msg =>
        {

        if (!string.IsNullOrEmpty(msg))
        {
                string cmd = "";
                string[] parameters = null;
                if (msg[0] == '/')
                {
                    var cmds = msg.Substring(1, msg.Length - 1).Split(" ");
                    cmd = cmds[0];
                    parameters = cmds.Skip(1).ToArray();
                    msg = $"> {msg}";
                }

                AddMsgEntry(msg);
                
                tmpInput.text = "";
                tmpInput.ActivateInputField();
                
                if (CommandAttribute.commandMap.ContainsKey(cmd))
                {
                    try 
                    {
                        var m = CommandAttribute.commandMap[cmd];

                        if (m.GetParameters().Length != parameters.Length) 
                            throw new ArgumentException("Argument list does not match with method");

                        m.Invoke(this, parameters);
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"Unable to run command {cmd}: " + e.Message);
                    }
                }
            }
        });
    }

    private void AddMsgEntry(string msg)
    {
        var newEntry = Instantiate(msgEntry, msgHistory.transform, false);
        newEntry.text = msg;
        newEntry.gameObject.SetActive(true);

        if (msgHistory.transform.childCount > maxHistory)
            Destroy(msgHistory.transform.GetChild(1).gameObject);

    }

    private void ClearMsgHistory()
    {
        for (int i = 1; i < msgHistory.transform.childCount; i++)
        {
            Destroy(msgHistory.transform.GetChild(i).gameObject);
        }
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleNewLogMessage;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleNewLogMessage;
    }

    private void HandleNewLogMessage(string logMsg, string stackTrace, LogType type)
    {
        AddMsgEntry(logMsg);
    }

    [Command]
    public void Hello()
    {
        Debug.Log("Hello World!");
    }

    [Command]
    public void Clear()
    {
        ClearMsgHistory();
    }
}
