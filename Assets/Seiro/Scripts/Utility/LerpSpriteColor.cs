﻿using UnityEngine;
using System;

namespace Seiro.Scripts.Utility {

	/// <summary>
	/// スプライトのColorの線形補間
	/// </summary>
	[RequireComponent(typeof(SpriteRenderer))]
	public class LerpSpriteColor : MonoBehaviour {

		private SpriteRenderer target;
		public SpriteRenderer Target { get { return target; } }

		private LerpColor lerpColor;

		[SerializeField]
		private float t = 10f;

		#region UnityEvent

		private void Awake() {
			target = GetComponent<SpriteRenderer>();
			lerpColor = new LerpColor(target.color, target.color);
		}

		private void Update() {
			if(!lerpColor.Processing) return;
			lerpColor.Update(t * Time.deltaTime);
			target.color = lerpColor.Value;
		}

		#endregion

		#region Function

		/// <summary>
		/// 色の設定
		/// </summary>
		public void SetColor(Color value, Color target) {
			lerpColor.SetValues(value, target);
		}

		/// <summary>
		/// 色の目標値の設定
		/// </summary>
		public void SetTargetColor(Color target) {
			lerpColor.SetTarget(target);
		}

		/// <summary>
		/// 透明度の目標値の設定
		/// </summary>
		public void SetTargetAlpha(float value) {
			Color v = lerpColor.Target;
			v.a = value;
			lerpColor.SetTarget(v);
		}

		#endregion
	}
}