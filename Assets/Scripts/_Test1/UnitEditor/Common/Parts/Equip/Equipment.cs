using UnityEngine;
using System;

namespace Scripts._Test1.UnitEditor.Common.Parts.Equip {

	/// <summary>
	/// パーツの設備/装備
	/// </summary>
	[Serializable]
	public class Equipment {

		public string tag;		//タグ(識別子)
		public Vector2 point;	//座標
		public float angle;		//角度
		public bool use;		//使用

		public Equipment(string tag, Vector2 point, float angle) {
			this.tag = tag;
			this.point = point;
			this.angle = angle;
			this.use = false;
		}
	}
}