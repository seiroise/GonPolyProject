using UnityEngine;
using System.Collections.Generic;

namespace Scripts._Test1.UnitEditor.Component {

	/// <summary>
	/// ユニットエディタのコンポーネント
	/// </summary>
	public abstract class UnitEditorComponent : MonoBehaviour {

		protected UnitEditor unitEditor;

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public virtual void Initialize(UnitEditor unitEditor) {
			this.unitEditor = unitEditor;
		}

		/// <summary>
		/// 全てのコンポーネントの初期化後に呼ばれる
		/// </summary>
		public virtual void LateInitialize() {
			
		}

		#endregion

	}
}