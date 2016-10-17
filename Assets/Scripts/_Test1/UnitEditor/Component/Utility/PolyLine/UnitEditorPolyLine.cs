using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Seiro.Scripts.Utility;
using Scripts._Test1.UnitEditor.Component.StateMachine;

namespace Scripts._Test1.UnitEditor.Component.Utility.PolyLine {

	/// <summary>
	/// ユニットエディタ用ポリライン描画
	/// </summary>
	public class UnitEditorPolyLine : UnitEditorStateMachine {

		[Header("Utility")]
		public UnitEditorPolyLineRenderer renderer;
		public UnitEditorPolyLineSnapper snapper;

		[Header("Input")]
		public EventSystem uiEventSystem;
		public Camera cam;

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(UnitEditor unitEditor, UnitEditorStateMachine owner) {
			base.Initialize(unitEditor, owner);

			//ユーティリティの初期化
			renderer.Initialize(unitEditor);
			snapper.Initialize(unitEditor);
		}

		/// <summary>
		/// 遅延初期化
		/// </summary>
		public override void LateInitialize() {
			base.LateInitialize();

			//ユーティリティの遅延初期化
			renderer.LateInitialize();
			snapper.LateInitialize();
		}

		#endregion

		#region Function

		/// <summary>
		/// マウス座標の取得。指定したEventStstemに被る場合はfalseを返す
		/// </summary>
		public bool GetMousePoint(out Vector2 point) {
			point = FuncBox.GetMousePosition(cam);
			if(!uiEventSystem) {
				return true;
			} else if(uiEventSystem.IsPointerOverGameObject()) {
				return false;
			} else {
				return true;
			}
		}

		#endregion
	}
}