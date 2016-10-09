using UnityEngine;
using UnityEngine.EventSystems;
using Seiro.Scripts.Utility;

namespace Scripts._Test.PolyPartsEditor {
	
	/// <summary>
	/// RectTransformの座標を変更
	/// </summary>
	public class LerpRectPosition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

		public enum Trigger {
			None,
			Enter_Exit,
			Click
		}

		public RectTransform target;

		public Trigger trigger = Trigger.Enter_Exit;

		public Vector2 normal;
		public Vector2 over;

		[Range(0.01f, 60f)]
		public float toOverLerpT = 10f;
		[Range(0.01f, 60f)]
		public float toNormalLerpT = 10f;

		public bool lockX = false;
		public bool lockY = false;

		private LerpFloat xLerp;
		private LerpFloat yLerp;
		private bool toOver;

		#region UnityEvent

		private void Awake() {
			xLerp = new LerpFloat();
			yLerp = new LerpFloat();

			SetLerpTarget(normal);
		}

		private void Update() {
			if (!(xLerp.Processing || yLerp.Processing)) return;
			float t = Time.deltaTime * (toOver ? toOverLerpT : toNormalLerpT);
			xLerp.Update(t);
			yLerp.Update(t);

			target.anchoredPosition = new Vector2(xLerp.Value, yLerp.Value);
		}

		#endregion

		#region Function

		/// <summary>
		/// 目標値の設定
		/// </summary>
		private void SetLerpTarget(Vector2 lerpTarget) {
			if (target == null) return;
			xLerp.SetTarget(lockX ? target.sizeDelta.x : lerpTarget.x);
			yLerp.SetTarget(lockY ? target.sizeDelta.y : lerpTarget.y);
		}

		/// <summary>
		/// Overへ
		/// </summary>
		public void ToOver() {
			SetLerpTarget(over);
			toOver = true;
		}

		/// <summary>
		/// Normalへ
		/// </summary>
		public void ToNormal() {
			SetLerpTarget(normal);
			toOver = false;
		}

		#endregion

		#region IPointerEvent

		public void OnPointerEnter(PointerEventData eventData) {
			if (trigger != Trigger.Enter_Exit) return;
			ToOver();
		}

		public void OnPointerExit(PointerEventData eventData) {
			if (trigger != Trigger.Enter_Exit) return;
			ToNormal();
		}

		public void OnPointerClick(PointerEventData eventData) {
			if (trigger != Trigger.Click) return;
			if (toOver) {
				ToNormal();
			} else {
				ToOver();
			}
		}

		#endregion

	}
}