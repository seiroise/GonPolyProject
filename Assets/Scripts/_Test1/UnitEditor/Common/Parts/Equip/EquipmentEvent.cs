using UnityEngine.Events;
using System;

namespace Scripts._Test1.UnitEditor.Common.Parts.Equip {

	/// <summary>
	/// 装備のイベント
	/// </summary>
	[Serializable]
	public class EquipmentEvent : UnityEvent<PartsObject, Equipment> { }
}