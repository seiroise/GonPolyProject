using UnityEngine;
using Scripts._Test.PolyPartsEditor.Common;
using Seiro.Scripts.Graphics.PolyLine2D;

namespace Scripts._Test.PolyPartsEditor.Component.Adjuster {

	/// <summary>
	/// ポリゴンパーツエディタのパーツコピー
	/// </summary>
	class PolyPartsCopy : PolyPartsAdjusterComponent {

		public PolyLine2DEditor polyLineEditor;


		private bool active = false;
		private PolyPartsObject selected;

		private PolyPartsObject noticePolyObj;	//予告用ポリゴンオブジェクト

		#region UnityEvent

		private void Update() {
			if(active) {
				UpdateNotice();
				InputChcek();
			}
		}

		#endregion

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(PolyPartsEditor editor, PolyPartsAdjuster adjuster) {
			base.Initialize(editor, adjuster);

			//コールバックの設定
			adjustMenu.copyBtn.onClick.RemoveListener(OnCopyButtonClicked);
			adjustMenu.copyBtn.onClick.AddListener(OnCopyButtonClicked);
		}

		/// <summary>
		/// 開始
		/// </summary>
		public override void Enter() {
			base.Enter();
			selected = adjuster.GetSelected();

			active = true;
		}

		/// <summary>
		/// 終了
		/// </summary>
		public override void Exit() {
			base.Exit();

			active = false;
		}

		#endregion

		#region Function

		/// <summary>
		/// 予告の更新
		/// </summary>
		private void UpdateNotice() {
			
		}

		/// <summary>
		/// 入力確認
		/// </summary>
		private void InputChcek() {
			if(Input.GetMouseButtonDown(0)) {
				//座標確認
				Vector2 point;
				if(polyLineEditor.GetMousePoint(out point)) {
					//スナップ確認
					Vector2 snapPoint;
					if(polyLineEditor.supporter.Snap())

				}
			}
		}

		#endregion

		#region UICalback

		/// <summary>
		/// 複製ボタンのクリック
		/// </summary>
		private void OnCopyButtonClicked() {
			Enter();
		}

		#endregion

	}
}