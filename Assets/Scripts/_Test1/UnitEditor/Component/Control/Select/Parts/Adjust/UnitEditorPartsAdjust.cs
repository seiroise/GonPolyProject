using Scripts._Test1.UnitEditor.Common.Parts;
using Scripts._Test1.UnitEditor.Component.StateMachine;
using Scripts._Test1.UnitEditor.Component.UI.ShowHide.Sidemenu;
using Scripts._Test1.UnitEditor.Component.Utility.Database;
using Scripts._Test1.UnitEditor.Component.Utility.PolyLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts._Test1.UnitEditor.Component.Control.Select.Parts.Adjust {

	/// <summary>
	/// ユニットエディタのパーツ調整
	/// </summary>
	public class UnitEditorPartsAdjust : UnitEditorState {

		//selectParts
		private UnitEditorSelectParts selectParts;
		private PartsObject select;

		//コンポーネント
		private UnitEditorPolyLine polyLine;
		private UnitEditorDatabase database;

		//ポリラインコンポーネント
		private UnitEditorPolyLineAdjuster adjuster;
		
		//定数
		private const string MENU = "PartsMenu";
		private const string LINE_COM = "Adjuster";

		#region VirtualFunction

		/// <summary>
		/// 遅延初期化
		/// </summary>
		public override void LateInitialize() {
			base.LateInitialize();

			//selectParts
			selectParts = (UnitEditorSelectParts)owner;

			//コンポーネントの取得
			polyLine = unitEditor.polyLine;
			database = unitEditor.database;
			adjuster = (UnitEditorPolyLineAdjuster)polyLine.GetState(LINE_COM);

			//UIコールバックの設定
			UnitEditorUIPartsMenu menu = (UnitEditorUIPartsMenu)unitEditor.ui.sidemenu.GetUIComponent(MENU);
			menu.adjustBtn.onClick.AddListener(OnAdjustButtonClicked);
		}

		public override void Enter() {
			base.Enter();
			
			//選択ポリゴンの取得
			select = selectParts.Select;
			select.Disable();

			//編集する頂点の設定
			adjuster.SetVertices(select.GetVertices(), true);

			//状態遷移
			polyLine.ActivateState(LINE_COM);

			//コールバック設定
			adjuster.onEndAdjust.AddListener(OnAdjustEnd);
			adjuster.onChangeVertex.AddListener(OnVertexChanged);
		}

		public override void Exit() {
			base.Exit();

			//コールバック削除(スタックオーバーフローを防ぐ)
			adjuster.onEndAdjust.RemoveListener(OnAdjustEnd);
			adjuster.onChangeVertex.RemoveListener(OnVertexChanged);

			//状態遷移
			polyLine.DisactivateState();

			//選択ポリゴンの有効化
			select.Enable();
		}
		#endregion

		#region Callback

		/// <summary>
		/// 調整終了
		/// </summary>
		private void OnAdjustEnd(List<Vector2> vertices) {
			//状態遷移
			owner.DisactivateState();
		}

		/// <summary>
		/// 頂点変更
		/// </summary>
		private void OnVertexChanged(List<Vector2> vertices) {
			if(vertices == null) return;
			//パーツの作成
			select.SetVertices(vertices);
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 調整ボタンのクリック
		/// </summary>
		private void OnAdjustButtonClicked() {
			//状態遷移
			owner.ActivateState(this.name);
		}

		#endregion
	}
}