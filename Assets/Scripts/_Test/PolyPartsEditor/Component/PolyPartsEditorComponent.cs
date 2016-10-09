using UnityEngine;
using System;
using Seiro.Scripts.Graphics.PolyLine2D;

namespace Scripts._Test.PolyPartsEditor.Component {

	/// <summary>
	/// ポリゴンパーツ編集エディタのコンポーネント
	/// </summary>
	public abstract class PolyPartsEditorComponent : MonoBehaviour {

		protected PolyPartsEditor editor;

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Initialoze(PolyPartsEditor editor) {
			this.editor = editor;
		}

	}
}