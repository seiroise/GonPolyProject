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

		[Header("Callback")]
		public MarkerEvent onDown;
		public MarkerEvent onUp;
		public MarkerEvent onClick;

		private bool overed = false;

		#region ICollisionEventHandler メンバー

		public void OnPointerEnter(RaycastHit hit) {
			lerpSprite.SetTargetColor(overColor);
			overed = true;
		}

		public void OnPointerExit(RaycastHit hit) {
			lerpSprite.SetTargetColor(normalColor);
			overed = true;
		}

		public void OnPointerDown(RaycastHit hit) {
			lerpSprite.SetTargetColor(clickColor);
			//コールバック
			onDown.Invoke(gameObject);
		}

		public void OnPointerUp(RaycastHit hit) {
			lerpSprite.SetTargetColor(overColor);
			//コールバック
			onUp.Invoke(gameObject);
		}

		public void OnPointerClick(RaycastHit hit) {
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