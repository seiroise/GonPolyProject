using UnityEngine;
using UnityEngine.Events;
using System;

namespace Scripts._Test.PolyPartsEditor.UI.ColorEditor {

	/// <summary>
	/// ColorEditFieldで使うイベント
	/// </summary>
	[Serializable]
	class ColorEditorEvent : UnityEvent<Color> { }
}