using UnityEngine.Events;
using System;

namespace Scripts._Test1.UnitEditor.Common.Parts {

	/// <summary>
	/// パーツイベント
	/// </summary>
	[Serializable]
	public class PartsEvent : UnityEvent<PartsObject> { }
}