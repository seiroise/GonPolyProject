using System;

namespace Scripts._Test1.UnitEditor.Component.UI {

	/// <summary>
	/// ユニットエディタのUIコンポーネント
	/// </summary>
	public abstract class UnitEditorUIComponent : UnitEditorComponent {

		protected UnitEditorUI editorUI;

		#region VirtualFunction

		/// <summary>
		/// 初期化。こっちは使わずUnitEditorUIを引数に取る方を使うこと
		/// </summary>
		[Obsolete("Not using")]
		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);
		}

		/// <summary>
		/// 初期化。こっちを使う
		/// </summary>
		public virtual void Initialize(UnitEditor unitEditor, UnitEditorUI editorUI) {
			base.Initialize(unitEditor);
			this.editorUI = editorUI;
		}

		#endregion
	}
}