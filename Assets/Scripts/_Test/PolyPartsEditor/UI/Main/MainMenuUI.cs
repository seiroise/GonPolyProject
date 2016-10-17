using UnityEngine;
using UnityEngine.UI;
using Scripts._Test.PolyPartsEditor.UI.HideIndicate;
using Scripts._Test.PolyPartsEditor.Common;

namespace Scripts._Test.PolyPartsEditor.UI.Main {
	
	/// <summary>
	/// メインメニューのUI
	/// </summary>
	public class MainMenuUI : HideIndicateUI {

		[Header("Buttons")]
		public Button makeBtn;
		public Button saveBtn;
		public Button undoBtn;
		public Button redoBtn;
		public Button equipClearBtn;
		public Button partsClearBtn;
		public Button gridBtn;
		public Button testBtn;
		public Button configBtn;

		public Button exitBtn;

	}
}