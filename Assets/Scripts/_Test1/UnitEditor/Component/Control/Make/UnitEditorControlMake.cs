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
		private const string MAKE_COM = "Maker";

		#region VirtualFunction

		public override void LateInitialize() {
			base.LateInitialize();

			//コンポーネントの取得
			polyLine = unitEditor.polyLine;
			database = unitEditor.database;
			maker = (UnitEditorPolyLineMaker)polyLine.GetStateComponent(MAKE_COM);
			
			//menuの取得
			UnitEditorUIMainmenu mainmenu = (UnitEditorUIMainmenu)unitEditor.ui.sidemenu.GetUIComponent(MENU);
			
			//ColorEditFieldの取得
			colorEditField = unitEditor.ui.colorEditField;
			
			//UIコールバックの設定
			mainmenu.makeBtn.onClick.AddListener(OnMakeBtnClicked);
		}

		public override void Enter() {
			base.Enter();
			//状態遷移
			polyLine.ActivateStateComponent(MAKE_COM);
			//コールバック追加
			maker.onMakeEnd.AddListener(OnMakeEnd);
		}

		public override void Exit() {
			base.Enter();
			//状態遷移
			polyLine.DisactivateStateComponent();
			//コールバック削除
			maker.onMakeEnd.RemoveListener(OnMakeEnd);
		}

		#endregion

		#region Function

		/// <summary>
		/// 作成開始
		/// </summary>
		private void MakeStart() {
			//状態遷移
			owner.ActivateState(this.name);
		}

		/// <summary>
		/// 作成終了
		/// </summary>
		private void MakeEnd(List<Vector2> vertices) {
			if (vertices != null) {
				//パーツの作成
				PartsObject polyObj = database.InstantiatePolyObj(vertices, colorEditField.GetColor());
				//無効化
				polyObj.Disable();
			}
		}

		#endregion

		#region Callback

		/// <summary>
		/// 作成終了
		/// </summary>
		private void OnMakeEnd(List<Vector2> vertices) {
			MakeEnd(vertices);
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 作成ボタンのクリック
		/// </summary>
		private void OnMakeBtnClicked() {
			MakeStart();
		}

		#endregion

	}
}