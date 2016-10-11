using UnityEngine;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.UI;
using Scripts._Test.PolyPartsEditor.Component;
using Scripts._Test.PolyPartsEditor.Database;

namespace Scripts._Test.PolyPartsEditor {

	/// <summary>
	/// ポリゴンパーツエディター
	/// </summary>
	public class PolyPartsEditor : MonoBehaviour {

		[Header("Components")]
		public PolyPartsMaker maker;
		public PolyPartsAdjuster adjuster;
		private List<PolyPartsEditorComponent> components;

		[Header("UI")]
		public PolyPartsEditorUI ui;

		[Header("DataBase")]
		public PolyPartsDatabase database;

		#region UnityEvent

		private void Start() {
			components = new List<PolyPartsEditorComponent>();
			components.Add(maker);
			components.Add(adjuster);

			for(int i = 0; i < components.Count; ++i) {
				components[i].Initialize(this);
			}
		}

		#endregion
	}
}