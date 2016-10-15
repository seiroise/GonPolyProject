using Scripts._Test.PolyPartsEditor.Common;
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
	class UnitEditorControlMake : UnitEditorControlComponent {

		private UnitEditorPolyLine polyLine;
		private UnitEditorDatabase database;

		private UnitEditorPolyLineMaker maker;

		//定数
		private const string MENU = "Mainmenu";
		private const string MAKE_COM = "Maker";

		#region VirtualFunction

		public override void LateInitialize() {
			base.LateInitialize();
			//componentの取得
			polyLine = unitEditor.polyLine;
			maker = (UnitEditorPolyLineMaker)polyLine.GetStateComponent(MAKE_COM);
			database = unitEditor.database;
			//menuの取得
			UnitEditorUIMainmenu mainmenu = (UnitEditorUIMainmenu)unitEditor.ui.sidemenu.GetUIComponent(MENU);
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
			editorController.ActivateControlComponent(this.name);
		}

		/// <summary>
		/// 作成終了
		/// </summary>
		private void MakeEnd(List<Vector2> vertices) {
			if (vertices != null) {
				PolyPartsObject polyObj = database.InstantiatePolyObj(vertices);
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