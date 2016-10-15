﻿using UnityEngine;
using UnityEngine.EventSystems;
using Seiro.Scripts.Utility;

namespace Scripts._Test1.UnitEditor.Common.Lerp {

	/// <summary>
	/// RectTransformのサイズを変更
	/// </summary>
	public class UILerpRectSize : UILerp<Vector2> {

		public bool lockX = false;
		public bool lockY = false;

		private LerpFloat xLerp;
		private LerpFloat yLerp;
		

		#region UnityEvent

		private void Awake() {
			xLerp = new LerpFloat();
			yLerp = new LerpFloat();

			ToBefore();
		}

		private void Update() {
			if (!(xLerp.Processing || yLerp.Processing)) return;
			float t = Time.deltaTime * (toAfter ? afterLerpT : beforeLerpT);
			xLerp.Update(t);
			yLerp.Update(t);

			target.sizeDelta = new Vector2(xLerp.Value, yLerp.Value);
		}

		#endregion
	}
}