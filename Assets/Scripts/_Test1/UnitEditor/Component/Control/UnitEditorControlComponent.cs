using UnityEngine;
using System;
using Seiro.Scripts.FSM;

namespace Scripts._Test1.UnitEditor.Component.Control {

	/// <summary>
	/// ユニットエディタの操作コンポーネント
	/// </summary>
	public abstract class UnitEditorControlComponent : UnitEditorStateComponent {

		protected UnitEditorControl editorController;

		#region VirtualFunction

		/// <summary>
		/// 初期化。こっちは使わずUnitEditorControllerを引数に取る方を使うこと
		/// </summary>
		[Obsolete("Not using")]
		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);
		}

		/// <summary>
		/// 初期化。こっちを使う
		/// </summary>
		public virtual void Initialize(UnitEditor unitEditor, UnitEditorControl editorController) {
			base.Initialize(unitEditor);
			this.editorController = editorController;
		}

		#endregion
	}
}
