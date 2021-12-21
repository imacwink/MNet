using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
	public enum STGlobalEventDef
	{
		EVENT_GLOBAL_BEGIN = 0,
		EVENT_CMD_UPDATE_OBS_ENTITY, /*刷新OB展示*/
		EVENT_GLOBAL_END,
	};

	public delegate void STEventNotify(int iEventID, System.Object args);
	public struct STEventElement
	{
		public event STEventNotify mEvent;

		public void SetEvent(int iEventID, System.Object args)
		{
			if (mEvent != null)
			{
				mEvent(iEventID, args);
			}
		}

		public void Release()
		{
			if (mEvent != null)
			{
				mEvent = null;
			}
		}
	};

	public class STGlobalEventNotify : STMonoSingleton<STGlobalEventNotify> 
	{
		private STEventElement[] mEventServerArray = new STEventElement[(int)STGlobalEventDef.EVENT_GLOBAL_END];

		public STGlobalEventNotify SubscribeEvent(int iEventID, STEventNotify fun)
		{
			mEventServerArray[iEventID].mEvent += fun;

			return this;
		}

		public STGlobalEventNotify UnSubscribeEvent(int iEventID, STEventNotify fun)
		{
			mEventServerArray[iEventID].mEvent -= fun;

			return this;
		}

		public void SetEvent(int iEventID, System.Object args)
		{
			mEventServerArray[iEventID].SetEvent(iEventID, args);
		}

		public void OnDestroy()
		{
			for (int i = 0; i < mEventServerArray.Length; i++)
			{
				mEventServerArray[i].Release();
			}
		}
	}
}