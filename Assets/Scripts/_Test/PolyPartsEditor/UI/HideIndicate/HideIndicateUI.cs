using UnityEngine;
using Scripts._Test.PolyPartsEditor.Common;

namespace Scripts._Test.PolyPartsEditor.UI.HideIndicate {

	/// <summary>
	/// ポリゴンパーツ編集エディタの表示/非表示UI
	/// </summary>
	public abstract class HideIndicateUI : MonoBehaviour, IHideIndicateUI<HideIndicateUI> {

		[Header("LerpPosition")]
		public LerpRectPosition lerpPosition;

		#region IHideIndicateUI

		/// <summary>
		/// 表示
		/// </summary>
		public void Indicate() {
			if (!lerpPosition) return;
			lerpPosition.ToNormal();
		}

		/// <summary>
		/// 非表示
		/// </summary>
		public void Hide() {
			if (!lerpPosition) return;
			lerpPosition.ToOver();
		}

		#endregion
	}
}