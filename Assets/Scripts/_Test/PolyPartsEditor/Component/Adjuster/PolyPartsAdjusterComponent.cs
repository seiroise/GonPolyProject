using Scripts._Test.PolyPartsEditor.UI.Adjuster;

namespace Scripts._Test.PolyPartsEditor.Component.Adjuster {

	public abstract class PolyPartsAdjusterComponent : PolyPartsEditorComponent {
	
		protected PolyPartsAdjuster adjuster;
		protected AdjustMenuUI adjustMenu;

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Initialize(PolyPartsEditor editor, PolyPartsAdjuster adjuster) {
			this.Initialize(editor);
			this.adjuster = adjuster;
			this.adjustMenu = adjuster.AdjustMenu;
		}

		#endregion
	}
}