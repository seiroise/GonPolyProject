using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Scripts._Test.PolyPartsEditor.UI.HideIndicate;

namespace Scripts._Test.PolyPartsEditor.UI.Adjuster {

	/// <summary>
	/// パーツ調整メニューのUI
	/// </summary>
	public class AdjustMenuUI : HideIndicateUI {

		[Header("Buttons")]
		public Button vertexAdjustBtn;
		public Button moveBtn;
		public Button copyBtn;
		public Button colorBtn;
		public Button rotateBtn;
		public Button mirrorBtn;
		public Button deleteBtn;

		public Button exitBtn;
	}
}