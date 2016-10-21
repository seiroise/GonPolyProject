using Scripts._Test.PolyPartsEditor.UI.ColorEditor;
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
	public class UnitEditorSelectParts : UnitEditorStateMachine {

		//コンポーネント
		private UnitEditorDatabase database;

		//選択パーツ
		private PartsObject select;
		public PartsObject Select { get { return select; } }

		//色調整
		private ColorEditField colorEditField;

		//定数
		private const string MENU = "PartsMenu";
		private const string PREV_MENU = "MainMenu";

		#region VirtualFunction

		public override void LateInitialize() {
			base.LateInitialize();

			//コンポーネントの取得
			database = unitEditor.database;

			//ColorEditFieldにパーツの色を反映
			colorEditField = unitEditor.ui.colorEditField;

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

			//選択パーツ以外を無効化
			database.DisableParts(select);

			//ColorEditFieldにパーツの色を反映とコールバックの設定
			colorEditField.SetColor(select.polygonColor);
			colorEditField.onColorChanged.AddListener(OnColorChanged);
		}

		public override void Exit() {
			base.Exit();

			//menuの非表示
			unitEditor.ui.sidemenu.ShowUI(PREV_MENU);

			//全てのパーツを有効化
			database.EnableParts();

			//コールバックの削除
			colorEditField.onColorChanged.RemoveListener(OnColorChanged);
		}

		#endregion

		#region PartsCallback

		/// <summary>
		/// パーツオブジェクトのクリック
		/// </summary>
		private void OnPartsClicked(PartsObject partsObj) {
			//選択パーツに設定
			select = partsObj;
			//有効化
			owner.ActivateState(this.name);
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
		/// 色編集フィールド上で色の変更が起きた時
		/// </summary>
		private void OnColorChanged(Color color) {
			if (select == null) return;
			select.SetPolygonColor(color);
		}

		#endregion
	}
}