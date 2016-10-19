using UnityEngine;
using Seiro.Scripts.ObjectPool;
using Seiro.Scripts.Utility;
using Seiro.Scripts.EventSystems;

namespace Scripts._Test1.UnitEditor.Component.Utility.Marker {

	/// <summary>
	/// スプライトのマーカー
	/// </summary>
	public class SpriteMarker : MonoBehaviour, IMonoPoolItem<SpriteMarker>, ICollisionEventHandler {

		[Header("Sprite")]
		public LerpSpriteColor lerpSprite;
		public Color normalColor = Color.white;
		public Color overColor = new Color(0.8f, 0.8f, 0.8f);
		public Color clickColor = new Color(0.5f, 0.5f, 0.5f);
		[Range(0f, 1f)]
		public float disableColorScale = 0.75f;     //無効化時の色倍率
		private Color targetColor;

		[Header("Callback")]
		public MarkerEvent onDown;
		public MarkerEvent onUp;
		public MarkerEvent onClick;

		private bool overed = false;

		private bool disabled = false;

		#region Function

		/// <summary>
		/// 有効化
		/// </summary>
		public void Enable() {
			if(!disabled) return;
			disabled = false;
			//色の変更
			SetTargetColor(normalColor);
		}

		/// <summary>
		/// 無効化
		/// </summary>
		public void Disable() {
			if(disabled) return;
			disabled = true;
			//色の変更
			SetTargetColor(normalColor);
		}

		/// <summary>
		/// 目標色の設定
		/// </summary>
		private void SetTargetColor(Color color) {
			targetColor = color;
			color *= (disabled ? disableColorScale : 1f);
			lerpSprite.SetTargetColor(color);
		}

		#endregion

		#region ICollisionEventHandler メンバー

		public void OnPointerEnter(RaycastHit hit) {
			if(disabled) return;
			SetTargetColor(overColor);
			overed = true;
		}

		public void OnPointerExit(RaycastHit hit) {
			if(disabled) return;
			SetTargetColor(normalColor);
			overed = true;
		}

		public void OnPointerDown(RaycastHit hit) {
			if(disabled) return;
			SetTargetColor(clickColor);
			//コールバック
			onDown.Invoke(gameObject);
		}

		public void OnPointerUp(RaycastHit hit) {
			if(disabled) return;
			SetTargetColor(overColor);
			//コールバック
			onUp.Invoke(gameObject);
		}

		public void OnPointerClick(RaycastHit hit) {
			if(disabled) return;
			//コールバック
			onClick.Invoke(gameObject);
		}

		#endregion

		#region IMonoPoolItem<SpriteMarker> メンバー

		public void Initialize() {
			lerpSprite.SetColor(normalColor, normalColor);
		}

		public void Activate() {

		}

		public SpriteMarker GetThis() {
			return this;
		}

		#endregion
	}
}