using Scripts._Test.PolyPartsEditor.Common;
using Scripts._Test.PolyPartsEditor.UI.ColorEditor;
using Scripts._Test1.UnitEditor.Common.Parts;
using Scripts._Test1.UnitEditor.Component.StateMachine;
using Scripts._Test1.UnitEditor.Component.UI.ShowHide.Sidemenu;
using Scripts._Test1.UnitEditor.Component.Utility.Database;
using Scripts._Test1.UnitEditor.Component.Utility.PolyLine;
using Seiro.Scripts.Graphics.PolyLine2D;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts._Test1.UnitEditor.Component.Control.Make {
	
	/// <summary>
	/// ユニットエディタの作成操作
	/// </summary>
	class UnitEditorControlMake : UnitEditorState {

		//コンポーネント
		private UnitEditorPolyLine polyLine;
		private UnitEditorDatabase database;

		//ポリラインコンポーネント
		private UnitEditorPolyLineMaker maker;
		
		//UI
		private ColorEditField colorEditField;

		//定数
		private const string MENU = "MainMenu";
		private const string LINE_COM = "Maker";

		#region VirtualFunction

		public override void LateInitialize() {
			base.LateInitialize();

			//コンポーネントの取得
			polyLine = unitEditor.polyLine;
			database = unitEditor.database;
			maker = (UnitEditorPolyLineMaker)polyLine.GetState(LINE_COM);

			//UIコールバックの設定
			UnitEditorUIMainMenu mainmenu = (UnitEditorUIMainMenu)unitEditor.ui.sidemenu.GetUIComponent(MENU);
			mainmenu.makeBtn.onClick.AddListener(OnMakeBtnClicked);

			//ColorEditFieldの取得
			colorEditField = unitEditor.ui.colorEditField;
		}

		public override void Enter() {
			base.Enter();
			//状態遷移
			polyLine.ActivateState(LINE_COM);
			//コールバック追加
			maker.onMakeEnd.AddListener(OnMakeEnd);

			//全てのパーツを無効化
			database.DisableParts();
		}

		public override void Exit() {
			base.Enter();
			//状態遷移
			polyLine.DisactivateState();
			//コールバック削除
			maker.onMakeEnd.RemoveListener(OnMakeEnd);

			//全てのパーツを有効化
			database.EnableParts();
		}

		#endregion

		#region Callback

		/// <summary>
		/// 作成終了
		/// </summary>
		private void OnMakeEnd(List<Vector2> vertices) {
			if (vertices != null) {
				//パーツの作成
				PartsObject polyObj = database.InstantiateParts(vertices, colorEditField.GetColor());
				//無効化
				polyObj.Disable();
			}
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 作成ボタンのクリック
		/// </summary>
		private void OnMakeBtnClicked() {
			//状態遷移
			owner.ActivateState(this.name);
		}

		#endregion

	}
}