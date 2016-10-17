using UnityEngine;

namespace Scripts._Test.PolyPartsEditor.Component {

	/// <summary>
	/// ポリゴンパーツ編集エディタのコンポーネント
	/// </summary>
	public abstract class PolyPartsEditorComponent : MonoBehaviour {

		protected PolyPartsEditor editor;

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Initialize(PolyPartsEditor editor) {
			this.editor = editor;
		}

		/// <summary>
		/// 開始
		/// </summary>
		public virtual void Enter() {

		}

		/// <summary>
		/// 終了
		/// </summary>
		public virtual void Exit() {

		}

		#endregion
	}
}