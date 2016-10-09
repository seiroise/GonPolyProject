using UnityEngine;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.UI;

namespace Scripts._Test.PolyPartsEditor.Component {

	/// <summary>
	/// ポリゴンパーツエディター
	/// </summary>
	public class PolyPartsEditor : MonoBehaviour {

		[Header("Component")]
		public PolyPartsMaker maker;

		private List<PolyPartsEditorComponent> coms;

		[Header("UI")]
		public MainMenuUI mainMenuUI;
		public AdjustMenuUI adjustMenuUI;

		[Header("DataBase")]
		public PolyPartsDatabase database;

		#region UnityEvent

		private void Start() {
			coms = new List<PolyPartsEditorComponent>();

			coms.Add(maker);

			for(int i = 0; i < coms.Count; ++i) {
				coms[i].Initialoze(this);
			}
		}

		#endregion
	}
}