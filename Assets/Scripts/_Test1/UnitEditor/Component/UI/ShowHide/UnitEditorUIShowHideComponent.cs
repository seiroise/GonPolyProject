using System;
using UnityEngine;
using Scripts._Test1.UnitEditor.Common.ShowHide;

namespace Scripts._Test1.UnitEditor.Component.UI.ShowHide {

	/// <summary>
	/// 表示非表示が可能なユニットエディタのUIコンポーネント
	/// </summary>
	public abstract class UnitEditorUIShowHideComponent : UnitEditorUIComponent, IShowHide {

		protected UnitEditorUIShowHide showHide;

		#region VirtualFunction

		/// <summary>
		/// 初期化。こっちじゃなくてUnitEditorUIShowHideを引数に取る方を使う。
		/// </summary>
		[Obsolete]
		public override void Initialize(UnitEditor unitEditor, UnitEditorUI editorUI) {
			base.Initialize(unitEditor, editorUI);
		}

		/// <summary>
		/// 初期化。こっちを使う
		/// </summary>
		public virtual void Initialize(UnitEditor unitEditor, UnitEditorUI editorUI, UnitEditorUIShowHide showHide) {
			base.Initialize(unitEditor, editorUI);
			this.showHide = showHide;
		}

		#endregion

		#region IShowHide メンバー

		public virtual void Show() {
			
		}

		public virtual void Hide() {
			
		}

		#endregion
	}
}