using UnityEngine;
using System;
using Seiro.Scripts.ObjectPool;
using Seiro.Scripts.EventSystems;

namespace Scripts._Test.PolyPartsEditor.Common {

	/// <summary>
	/// スプライトプールのアイテム
	/// </summary>
	public class SpritePoolItem : MonoBehaviour, IMonoPoolItem<SpritePoolItem>, ICollisionEventHandler {

		public SpriteRenderer spRenderer;


		#region IMonoPoolItem

		public void Initialize() {

		}

		public void Activate() {

		}

		public SpritePoolItem GetThis() {
			return this;
		}

		#endregion

		#region ICollisionEventHandler

		public void OnPointerEnter(RaycastHit hit) { }

		public void OnPointerExit(RaycastHit hit) { }

		public void OnPointerDown(RaycastHit hit) {
			
		}

		public void OnPointerUp(RaycastHit hit) {
			
		}

		public void OnPointerClick(RaycastHit hit) {
			
		}

		#endregion
	}
}