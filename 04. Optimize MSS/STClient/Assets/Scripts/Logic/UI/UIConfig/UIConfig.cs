using UnityEngine;
using UnityEngine.UI;
using StateMachine;

public class UIConfig : MonoBehaviour
{
    public Button mStartServer;
    public Button mStartClient;

    void Start()
    {
        mStartServer.onClick.AddListener(OnStartServerEvent);
        mStartClient.onClick.AddListener(OnStartClientEvent);
    }

    private void OnStartServerEvent()
    {
        STStateMachine.ChangeState(STStateServer.s_stateName);
    }

    private void OnStartClientEvent()
    {
        STStateMachine.ChangeState(STStateClient.s_stateName);
    }
}
