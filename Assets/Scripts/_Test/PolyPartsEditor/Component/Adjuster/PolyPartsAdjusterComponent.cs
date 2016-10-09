using Scripts._Test.PolyPartsEditor.UI;

namespace Scripts._Test.PolyPartsEditor.Component.Adjuster {

	public abstract class PolyPartsAdjusterComponent : PolyPartsEditorComponent {
	
		protected PolyPartsAdjuster adjuster;
		protected AdjustMenuUI adjustMenu;

		public virtual void Initialize(PolyPartsEditor editor, PolyPartsAdjuster adjuster) {
			this.Initialize(editor);
			this.adjuster = adjuster;
			this.adjustMenu = adjuster.AdjustMenu;
		}
	}
}