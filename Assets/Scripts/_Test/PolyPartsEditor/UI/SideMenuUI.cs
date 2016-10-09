using UnityEngine;
using System;

namespace Scripts._Test.PolyPartsEditor.UI {

	/// <summary>
	/// ポリゴンパーツ編集エディタの左メニュー
	/// </summary>
	public abstract class SideMenuUI : MonoBehaviour {

		[Header("LerpPosition")]
		public LerpRectPosition lerpPosition;

		#region Function

		/// <summary>
		/// 表示
		/// </summary>
		public void Indicate() {
			if(!lerpPosition) return;
			lerpPosition.ToNormal();
		}

		/// <summary>
		/// 非表示
		/// </summary>
		public void Hide() {
			if(!lerpPosition) return;
			lerpPosition.ToOver();
		}

		#endregion
	}
}