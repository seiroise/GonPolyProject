using UnityEngine;
using System;

namespace Scripts._Test.PolyPartsEditor.Component.Adjuster {

	/// <summary>
	/// ポリゴンパーツの削除
	/// </summary>
	public class PolyPartsDelete : PolyPartsAdjusterComponent {

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(PolyPartsEditor editor, PolyPartsAdjuster adjuster) {
			base.Initialize(editor, adjuster);

			//UIコールバックの設定
			adjustMenu.deleteBtn.onClick.RemoveListener(OnDeleteButtonClicked);
			adjustMenu.deleteBtn.onClick.AddListener(OnDeleteButtonClicked);
		}

		/// <summary>
		/// 開始
		/// </summary>
		public override void Enter() {
			base.Enter();
			//選択オブジェクトの削除
			editor.database.DeletePolyPartsObject(adjuster.GetSelected());
			//調整終了
			adjuster.Exit();
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 削除ボタンのクリック
		/// </summary>
		private void OnDeleteButtonClicked() {
			Enter();
		}

		#endregion

	}
}