using System;
using Scripts._Test1.UnitEditor.Common.Parts.Equip;
using Scripts._Test1.UnitEditor.Component.StateMachine;
using Scripts._Test1.UnitEditor.Component.UI.ShowHide.Sidemenu;
using Scripts._Test1.UnitEditor.Component.Utility.Database;
using Scripts._Test1.UnitEditor.Common.Parts;

namespace Scripts._Test1.UnitEditor.Component.Control.Select.Launcher {

	/// <summary>
	/// ユニットエディタのランチャー選択
	/// </summary>
	public class UnitEditorSelectLauncher : UnitEditorState {

		//コンポーネント
		private UnitEditorDatabase database;

		//選択ランチャー
		private Equipment select;
		public Equipment Select { get { return select; } }

		//定数
		private const string MENU = "LauncherMenu";
		private const string PREV_MENU = "MainMenu";

		#region VirtualFunction

		/// <summary>
		/// 遅延初期化
		/// </summary>
		public override void LateInitialize() {
			base.LateInitialize();

			//コンポーネントの取得
			database = unitEditor.database;

			//マーカーコールバックの設定
			database.onLauncherClick.AddListener(OnLauncherClicked);

			//UIコールバックの設定
			UnitEditorUILauncherMenu menu = (UnitEditorUILauncherMenu)unitEditor.ui.sidemenu.GetUIComponent(MENU);
			menu.exitBtn.onClick.AddListener(OnExitButtonClicked);
		}

		public override void Enter() {
			base.Enter();

			//メニューの表示
			unitEditor.ui.sidemenu.ShowUI(MENU);
		}

		public override void Exit() {
			base.Exit();

			//メニューの非表示
			unitEditor.ui.sidemenu.ShowUI(PREV_MENU);
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 戻るボタンのクリック
		/// </summary>
		private void OnExitButtonClicked() {
			//無効化
			owner.DisactivateState();
		}

		/// <summary>
		/// ランチャーマーカーのクリック
		/// </summary>
		private void OnLauncherClicked(PartsObject partsObj, Equipment equipment) {
			select = equipment;
			//有効化
			owner.ActivateState(this.name);
		}

		#endregion
	}
}