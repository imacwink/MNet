using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using behaviac;

namespace Manager
{
    public class STBehaviacAIManager : STMonoSingleton<STBehaviacAIManager>
    {
         private List<behaviac.Agent> mAgentList = new List<behaviac.Agent>();

         private static string ExportedFilePath
        {
            get
            {
                string relativePath = "/External/behaviac/ai/exported";

                if (Application.platform == RuntimePlatform.WindowsEditor) {
                    return Application.dataPath + relativePath;
                }
                else if (Application.platform == RuntimePlatform.WindowsPlayer) {
                    return Application.dataPath + relativePath;
                }
                else {
                    return "Assets" + relativePath;
                }
            }
        }

         public void Init()
         {
            InitBehavic();
         }

        private bool InitBehavic()
        {
            behaviac.Workspace.Instance.FilePath = ExportedFilePath;
            behaviac.Workspace.Instance.FileFormat = behaviac.Workspace.EFileFormat.EFF_xml;
            return true;
        }

        public void RegisterAgent(behaviac.Agent agent)
        {
            mAgentList.Add(agent);
        }

        public void Clear()
        {
            for (int i = 0; i < mAgentList.Count; i++)
            {
                if(mAgentList[i] != null)
                {
                    Object.Destroy(mAgentList[i].gameObject);
                }
            }

            mAgentList.Clear();
        }

        public void ExecuteOneFrame()
        {
            for (int i = 0; i < mAgentList.Count; )
            {
                if (!mAgentList[i].IsActive() || mAgentList[i] == null)
                {
                    i++;
                    continue;
                }

                var ret = mAgentList[i].btexec();
                if (ret != behaviac.EBTStatus.BT_RUNNING)
                {
                    mAgentList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }
}
