using UnityEngine;
using UnityEngine.Events;
using System;

namespace Scripts._Test1.UnitEditor.Component.Utility.Marker {

	/// <summary>
	/// マーカーイベント
	/// </summary>
	[Serializable]
	public class MarkerEvent : UnityEvent<GameObject> { }
}