using UnityEngine;
using UnityEngine.UI;
using Scripts._Test1.UnitEditor.Common.Lerp;

namespace Scripts._Test1.UnitEditor.Component.UI.ShowHide.Sidemenu {

	/// <summary>
	/// デフォルトで表示するメニュー
	/// </summary>
	public class UnitEditorUIMainMenu : UnitEditorUISideMenu {

		[Header("Buttons")]
		public Button selectBtn;	//選択ボタン
		public Button makeBtn;		//作成ボタン

	}
}