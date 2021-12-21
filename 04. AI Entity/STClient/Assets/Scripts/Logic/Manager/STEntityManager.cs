using UnityEngine;
using System.Collections.Generic;
using Common;
using Protocol;

namespace Manager
{
    public class STEntityManager : STMonoSingleton<STEntityManager>
    {
        public string mLocalEntityID { get; set; }
        public Dictionary<string, GameObject> mEntityDic { get; set; }

        public void Init()
        {
            mLocalEntityID = "";
            mEntityDic = new Dictionary<string, GameObject>();
        }

        public Dictionary<string, GameObject> AllEntities()
        {
            return mEntityDic;
        }

        public void CreateEntity(string strRoot, STSpawnEntityPacket packet)
        {
            Debug.Log("CreateEntity Enity ID : " + packet.ID);

            GameObject entityObj = (GameObject)Resources.Load("Entity");
            Vector3 position = new Vector3(packet.X, packet.Y, packet.Z);
            Quaternion rotation = new Quaternion();

            GameObject entityInstance = Instantiate(entityObj, position, rotation);

            GameObject entityRoot = GameObject.Find(strRoot + "/GameRoot/EntityRoot");
            if (null != entityRoot)
            {
                entityInstance.transform.parent = entityRoot.transform;
            }

            mEntityDic.Add(packet.ID, entityInstance);

            Transform cameraTrans = entityInstance.transform.Find("Main Camera");

            if (packet.ID == mLocalEntityID)
            {
                entityInstance.AddComponent<STController>();
                entityInstance.transform.name = "LocalEntity";
                if (null != cameraTrans)
                {
                    cameraTrans.gameObject.SetActive(true);
                }
            }
            else
            {
                entityInstance.AddComponent<STMovement>();
                entityInstance.transform.name = packet.ID;
                if (null != cameraTrans)
                {
                    cameraTrans.gameObject.SetActive(false);
                }
            }

            EntityChange();
        }

        public void UpdateEntity(STEntityPositionPacket packet)
        {
            if (mEntityDic.ContainsKey(packet.ID))
            {
                GameObject obj = mEntityDic[packet.ID];
                if (null != obj)
                {
                    STMovement stMovement = obj.GetComponent<STMovement>();
                    if (null != stMovement)
                    {
                        stMovement.SetMovePosition(new Vector3(packet.X, packet.Y, packet.Z));
                    }
                }
            }

            EntityChange();
        }

        public void RemoveEntity(string strPacketID)
        {
            if (mEntityDic.ContainsKey(strPacketID))
            {
                GameObject obj = mEntityDic[strPacketID];
                if (null != obj)
                    MonoBehaviour.Destroy(obj);

                mEntityDic.Remove(strPacketID);
            }

            EntityChange();
        }

        private void EntityChange()
        {
            STGlobalEventNotify.GetInstance().SetEvent((int)STGlobalEventDef.EVENT_CMD_UPDATE_OBS_ENTITY, null);
        }
    }
}