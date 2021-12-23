using UnityEngine;
using UnityEngine.UI;
using StateMachine;
using Server;
using Manager;

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

        Application.targetFrameRate = 30; // 后续会设置到 UI 中进行动态配配置;

        STEntityManager.GetInstance().Init();
        STBehaviacAIManager.GetInstance().Init();

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
            STBehaviacAIManager.GetInstance().ExecuteOneFrame();
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
