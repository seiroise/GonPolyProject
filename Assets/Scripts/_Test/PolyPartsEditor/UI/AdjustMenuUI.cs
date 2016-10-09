using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Scripts._Test.PolyPartsEditor.UI {

	/// <summary>
	/// パーツ調整メニューのUI
	/// </summary>
	public class AdjustMenuUI : SideMenuUI {

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