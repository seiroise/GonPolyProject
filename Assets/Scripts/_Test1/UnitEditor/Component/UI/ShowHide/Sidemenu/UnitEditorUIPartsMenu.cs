using Scripts._Test1.UnitEditor.Common.Lerp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts._Test1.UnitEditor.Component.UI.ShowHide.Sidemenu {

	/// <summary>
	/// パーツ選択時に表示するメニュー
	/// </summary>
	public class UnitEditorUIPartsMenu : UnitEditorUISideMenu {

		[Header("Buttons")]
		public Button adjustBtn;	//調整ボタン
		public Button moveBtn;		//移動ボタン
		public Button rotateBtn;	//回転ボタン
		public Button mirrorBtn;	//反転ボタン
		public Button copyBtn;		//コピーボタン
		public Button deleteBtn;	//削除ボタン

	}
}