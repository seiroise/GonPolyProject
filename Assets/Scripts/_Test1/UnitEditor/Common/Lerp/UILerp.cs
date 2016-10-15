using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts._Test1.UnitEditor.Common.Lerp {

	/// <summary>
	/// UI用の補間処理クラス
	/// </summary>
	public abstract class UILerp<T> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

		/// <summary>
		/// トリガー
		/// </summary>
		public enum Trigger {
			None,
			EnterExit,
			Click
		}

		public RectTransform target;
		public Trigger trigger = Trigger.EnterExit;

		public T before;
		public T after;

		[Range(0.01f, 60f)]
		public float beforeLerpT = 10f;
		[Range(0.01f, 60f)]
		public float afterLerpT = 10f;

		protected bool toAfter = false;

		#region Function

		/// <summary>
		/// "後"状態へ
		/// </summary>
		public abstract void ToAfter() {
			SetLerpTarget(after);
			toAfter = true;
		}

		/// <summary>
		/// "前"状態へ
		/// </summary>
		public virtual void ToBefore() {
			SetLerpTarget(before);
			toAfter = false;
		}

		#endregion

		#region VirtualFunction

		protected abstract void SetLerpTarget(T lerpTarget);

		#endregion

		#region IPointerEvent

		public void OnPointerEnter(PointerEventData eventData) {
			if (trigger != Trigger.EnterExit) return;
			ToAfter();
		}

		public void OnPointerExit(PointerEventData eventData) {
			if (trigger != Trigger.EnterExit) return;
			ToBefore();
		}

		public void OnPointerClick(PointerEventData eventData) {
			if (trigger != Trigger.Click) return;
			if (toAfter) {
				ToBefore();
			} else {
				ToAfter();
			}
		}

		#endregion
	}
}