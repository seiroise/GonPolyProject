using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace Scripts._Test1.UnitEditor.Component.Utility.PolyLine {

	/// <summary>
	/// ポリライン用イベント
	/// </summary>
	[Serializable]
	public class PolyLineEvent : UnityEvent<List<Vector2>> { }
}