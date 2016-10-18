using UnityEngine;
using System;

namespace Scripts._Test1.UnitEditor.Common.Parts.Equipment {

	/// <summary>
	/// 設備
	/// </summary>
	public class Equipment {

		public Vector2 point;	//座標
		public string tag;		//タグ(識別子)
		public bool use;		//使用

		public Equipment(Vector2 point, string tag) {
			this.point = point;
			this.tag = tag;
			this.use = false;
		}
	}
}