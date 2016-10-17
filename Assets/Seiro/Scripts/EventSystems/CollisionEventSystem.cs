﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Seiro.Scripts.EventSystems {

	/// <summary>
	/// 画面上のあたり判定への各種入力イベントの通知
	/// </summary>
	public class CollisionEventSystem : MonoBehaviour {

		[Header("Parameter")]
		[SerializeField]
		private Camera camera;				//レイキャスト用カメラ
		[SerializeField]
		private int mouseButton = 0;		//クリック判定を行うボタン
		private Collider downCollider;		//押した時のコライダ

		[Header("PriorityEventSystem")]
		public EventSystem prioritySystem;	//優位なイベントシステム

		//その他
		private Collider prevCollider;
		private RaycastHit hitInfo;
		private const float EPSILON = 0.001f;
		private Dictionary<Collider, ICollisionEventHandler[]> cache;

		#region UnityEvent

		private void Awake() {
			cache = new Dictionary<Collider, ICollisionEventHandler[]>();
		}

		private void Update() {
			Vector2 screenPos = Input.mousePosition;
			CheckHighlight(screenPos);
			CheckClick();
		}

		#endregion

		#region Function

		/// <summary>
		/// 重なり確認
		/// </summary>
		private void CheckHighlight(Vector2 screenPos) {
			if (!camera) return;
			if (prioritySystem && prioritySystem.IsPointerOverGameObject()) {
				NohitCollider();
			} else {
				Ray ray = camera.ScreenPointToRay(screenPos);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100f)) {
					//ヒットした場合
					Collider hitCollider = hit.collider;
					HitCollider(hitCollider);
				} else {
					//ヒットしなかった場合
					NohitCollider();
				}
				hitInfo = hit;
			}
		}

		/// <summary>
		/// コライダに当たった
		/// </summary>
		private void HitCollider(Collider hitCollider) {
			if (prevCollider != hitCollider) {
				if (prevCollider == null) {
					//Enter
					EnterCollider(hitCollider);
				} else {
					//Exit & Enter
					ExitCollider(prevCollider);
					EnterCollider(hitCollider);
				}
			}
		}

		/// <summary>
		/// コライダに当たらなかった
		/// </summary>
		private void NohitCollider() {
			if (prevCollider != null) {
				//Exit
				ExitCollider(prevCollider);
			}
		}

		/// <summary>
		/// クリック確認
		/// </summary>
		private void CheckClick() {
			if(Input.GetMouseButtonUp(mouseButton)) {
				if(downCollider == prevCollider) {
					UpCollider(downCollider);
					ClickCollider(downCollider);
				}
				downCollider = null;
			}
			if(Input.GetMouseButtonDown(mouseButton)) {
				if(prevCollider != null) {
					downCollider = prevCollider;
					DownCollider(downCollider);
				}
			}
		}

		/// <summary>
		/// コライダー範囲に侵入
		/// </summary>
		private void EnterCollider(Collider col) {
			//コンポーネントの取得
			ICollisionEventHandler[] handlers = GetHandlers(col);
			if(handlers != null) {
				foreach(var e in handlers) {
					e.OnPointerEnter(hitInfo);
				}
			}
			prevCollider = col;
		}

		/// <summary>
		/// コライダー範囲から退出
		/// </summary>
		private void ExitCollider(Collider col) {
			//コンポーネントの取得
			ICollisionEventHandler[] handlers = GetHandlers(col);
			if(handlers != null) {
				foreach(var e in handlers) {
					e.OnPointerExit(hitInfo);
				}
			}
			prevCollider = null;
		}

		/// <summary>
		/// コライダー範囲でのボタン押下
		/// </summary>
		private void DownCollider(Collider col) {
			//コンポーネントの取得
			ICollisionEventHandler[] handlers = GetHandlers(col);
			if(handlers != null) {
				foreach(var e in handlers) {
					e.OnPointerDown(hitInfo);
				}
			}
		}

		/// <summary>
		/// コライダー範囲でのボタン押上
		/// </summary>
		private void UpCollider(Collider col) {
			//コンポーネントの取得
			ICollisionEventHandler[] handlers = GetHandlers(col);
			if(handlers != null) {
				foreach(var e in handlers) {
					e.OnPointerUp(hitInfo);
				}
			}
		}

		/// <summary>
		/// コライダー範囲でのクリック
		/// </summary>
		private void ClickCollider(Collider col) {
			ICollisionEventHandler[] handlers = GetHandlers(downCollider);
			if(handlers != null) {
				foreach(var e in handlers) {
					e.OnPointerClick(hitInfo);
				}
			}
		}

		/// <summary>
		/// コライダーからハンドラーを取得
		/// </summary>
		private ICollisionEventHandler[] GetHandlers(Collider col) {
			if(col == null) return null;
			//キャッシュを確認
			if(cache.ContainsKey(col)) {
				return cache[col];
			}
			//キャッシュになければ追加
			ICollisionEventHandler[] handlers = col.GetComponents<ICollisionEventHandler>();
			if(handlers != null) {
				cache.Add(col, handlers);
			}
			return handlers;
		}

		#endregion
	}
}