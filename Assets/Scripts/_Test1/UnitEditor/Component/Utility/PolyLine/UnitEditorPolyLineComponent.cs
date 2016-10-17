using System;

namespace Scripts._Test1.UnitEditor.Component.Utility.PolyLine {

	/// <summary>
	/// ユニットエディタのポリラインコンポーネント
	/// </summary>
	public abstract class UnitEditorPolyLineComponent : UnitEditorComponent {

		protected UnitEditorPolyLine polyLine;

		#region VirtualFunction

		/// <summary>
		/// 使わない。
		/// </summary>
		[Obsolete("Not using")]
		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);
		}

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Initialize(UnitEditor unitEditor, UnitEditorPolyLine polyLine) {
			base.Initialize(unitEditor);
			this.polyLine = polyLine;
		}

		#endregion
	}
}