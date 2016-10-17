using Scripts._Test1.UnitEditor.Common.Parts;
using Scripts._Test1.UnitEditor.Component.StateMachine;
using Scripts._Test1.UnitEditor.Component.UI.ShowHide.Sidemenu;
using Scripts._Test1.UnitEditor.Component.Utility.Database;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts._Test1.UnitEditor.Component.Control.Select.Parts {

	/// <summary>
	/// ユニットエディタのパーツ選択
	/// </summary>
	public class UnitEditorSelectParts : UnitEditorState {

		//コンポーネント
		private UnitEditorDatabase database;

		//定数
		private const string MENU = "PartsMenu";
		private const string PREV_MENU = "MainMenu";

		#region VirtualFunction

		public override void LateInitialize() {
			base.LateInitialize();

			//コンポーネントの取得
			database = unitEditor.database;

			//パーツコールバックの設定
			database.onPartsClick.AddListener(OnPartsClicked);

			//UIコールバックの設定
			UnitEditorUIPartsMenu menu = (UnitEditorUIPartsMenu)unitEditor.ui.sidemenu.GetUIComponent(MENU);
			menu.exitBtn.onClick.AddListener(OnExitButtonClicked);
		}

		public override void Enter() {
			base.Enter();

			//menuの表示
			unitEditor.ui.sidemenu.ShowUI(MENU);
		}

		public override void Exit() {
			base.Exit();

			//menuの非表示
			unitEditor.ui.sidemenu.ShowUI(PREV_MENU);
		}

		#endregion

		#region PartsCallback

		/// <summary>
		/// パーツオブジェクトのクリック
		/// </summary>
		private void OnPartsClicked(PartsObject partsObj) {
			//有効化
			owner.ActivateState(this.name);
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 戻るボタンのクリック
		/// </summary>
		private void OnExitButtonClicked() {
			owner.DisactivateState();
		}

		#endregion
	}
}