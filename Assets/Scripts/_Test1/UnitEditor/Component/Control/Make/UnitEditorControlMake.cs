using UnityEngine;
using Scripts._Test1.UnitEditor.Component.UI.ShowHide.Sidemenu;
using Seiro.Scripts.Graphics.PolyLine2D;

namespace Scripts._Test1.UnitEditor.Component.Control.Make {
	
	/// <summary>
	/// ユニットエディタの作成操作
	/// </summary>
	class UnitEditorControlMake : UnitEditorControlComponent {

		[Header("PolyLine")]
		public PolyLine2DEditor polyLine;

		//定数
		private const string MENU = "Mainmenu";

		#region VirtualFunction

		public override void LateInitialize() {
			base.LateInitialize();
			//menuの取得
			UnitEditorUIMainmenu mainmenu = (UnitEditorUIMainmenu)unitEditor.ui.sidemenu.GetUIComponent(MENU);
			//UIコールバックの設定
			mainmenu.makeBtn.onClick.AddListener(OnMakeBtnClicked);
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 作成ボタンのクリック
		/// </summary>
		private void OnMakeBtnClicked() {
			polyLine.EnableMaker();
		}

		#endregion

	}
}