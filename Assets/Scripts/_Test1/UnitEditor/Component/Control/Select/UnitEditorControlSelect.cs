using System.Collections.Generic;
using Scripts._Test1.UnitEditor.Component.UI.ShowHide.Sidemenu;
using Scripts._Test1.UnitEditor.Component.Utility.Database;
using UnityEngine;
using Scripts._Test1.UnitEditor.Component.StateMachine;

namespace Scripts._Test1.UnitEditor.Component.Control.Select {

	/// <summary>
	/// ユニットエディタの選択操作
	/// </summary>
	public class UnitEditorControlSelect : UnitEditorStateMachine {

		//コンポーネント
		private UnitEditorDatabase database;

		//定数
		private const string MENU = "MainMenu";

		#region VirtualFunction

		/// <summary>
		/// 遅延初期化
		/// </summary>
		public override void LateInitialize() {
			base.LateInitialize();

			//コンポーネントの取得
			database = unitEditor.database;

			//menuの取得
			UnitEditorUIMainmenu mainmenu = (UnitEditorUIMainmenu)unitEditor.ui.sidemenu.GetUIComponent(MENU);

			//UIコールバック追加
			mainmenu.selectBtn.onClick.AddListener(OnSelectBtnClicked);

		}

		/// <summary>
		/// 開始
		/// </summary>
		public override void Enter() {
			base.Enter();
			
			//全てのパーツを有効化
			database.EnablePolygons();
		}

		/// <summary>
		/// 終了
		/// </summary>
		public override void Exit() {
			base.Exit();

			//全てのパーツを無効化
			database.DisablePolygons();
		}

		#endregion


		#region UICallback

		//選択ボタンのクリック
		private void OnSelectBtnClicked() {
			//有効化
			owner.ActivateState(this.name);
		}

		#endregion

	}
}