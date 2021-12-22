using UnityEngine;

namespace behaviac
{
    public class ExampleAgent : Agent
    {
        // behaviac name
        public string btName = "ExampleAgent";

        // xml(export file) path
        public string xmlPath = "Assets/Resources/";

        private EBTStatus curStatus = EBTStatus.BT_RUNNING;

        public int intValue = 0;

        void Awake()
        {
            initB();
            initA();
        }

        private void initB()
        {
            Workspace.Instance.FilePath = Application.streamingAssetsPath;
            Workspace.Instance.FileFormat = Workspace.EFileFormat.EFF_xml;

            RegisterInstanceName<ExampleAgent>("ExampleAgent");
        }

        private bool initA()
        {
            bool bRet = this.btload(btName);
            if (!bRet)
                UnityEngine.Debug.LogError("Behavior tree data load failed! " + btName);
            else
                this.btsetcurrent(btName);

            return bRet;
        }

        public void Excute()
        {
            curStatus = this.btexec();
        }

        public EBTStatus PrintInfo()
        {
            print(intValue);
            return EBTStatus.BT_SUCCESS;
        }
    }
}