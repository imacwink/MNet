using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Common
{
	public class STEventTriggerListener : UnityEngine.EventSystems.EventTrigger
	{
		public delegate void VoidDelegate(GameObject go, PointerEventData eventData);
		public VoidDelegate onClick;
		public VoidDelegate onDown;
		public VoidDelegate onEnter;
		public VoidDelegate onExit;
		public VoidDelegate onUp;
		public VoidDelegate onSelect;
		public VoidDelegate onUpdateSelect;

		static public STEventTriggerListener Get(GameObject go)
		{
			STEventTriggerListener listener = go.GetComponent<STEventTriggerListener>();
			if (listener == null) listener = go.AddComponent<STEventTriggerListener>();
			return listener;
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			if (onClick != null) onClick(gameObject, eventData);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (onDown != null) onDown(gameObject, eventData);
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			if (onEnter != null) onEnter(gameObject, eventData);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			if (onExit != null) onExit(gameObject, eventData);
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			if (onUp != null) onUp(gameObject, eventData);
		}

		public override void OnSelect(BaseEventData eventData)
		{
			if (onSelect != null) onSelect(gameObject, null);
		}

		public override void OnUpdateSelected(BaseEventData eventData)
		{
			if (onUpdateSelect != null) onUpdateSelect(gameObject, null);
		}
	}
}