using UnityEngine;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.UI.HideIndicate;

namespace Scripts._Test.PolyPartsEditor.UI {

	/// <summary>
	/// ポリゴンパーツ作成エディタのUI
	/// </summary>
	public class PolyPartsEditorUI : MonoBehaviour {

		[Header("SideMenu")]
		public HideIndicateUIManager sideMenu;

		[Header("Popup")]
		public HideIndicateUIManager popup;

	}
}