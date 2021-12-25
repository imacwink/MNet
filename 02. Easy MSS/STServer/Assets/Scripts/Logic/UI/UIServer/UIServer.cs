using UnityEngine;
using UnityEngine.UI;
using StateMachine;
using Server;

public class UIServer : MonoBehaviour
{
    [Header("Server Settings")]
    public int mMaximumConnections = 128;
    public string mServerIP = "127.0.0.1";
    public int mPort = 6337;
    public string mServerName = "MNetSimple";

    [Header("UI Settings")]
    public Button mBack;

    private STServer mServer;

    void Start()
    {
        mBack.onClick.AddListener(OnBackEvent);

        Time.fixedDeltaTime = 1f / 60; // 后续会设置到 UI 中进行动态配配置;

        mServer = new STServer(mMaximumConnections, mPort, mServerIP, mServerName);
        mServer.StartServer();
    }

    private void OnBackEvent()
    {
        if (mServer != null)
        {
            mServer.Shutdown(mServerName);
            mServer = null;
        }

        STStateMachine.ChangeState(STStateConfig.s_stateName);
    }

    private void FixedUpdate()
    {
        if (mServer != null)
        {
            mServer.ProcessServerListen();
        }
    }

    private void OnDestroy()
    {
        if (mServer != null)
        {
            mServer.Shutdown(mServerName);
            mServer = null;
        }
    }
}
