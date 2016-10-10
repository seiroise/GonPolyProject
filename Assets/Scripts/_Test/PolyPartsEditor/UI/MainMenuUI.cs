﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts._Test.PolyPartsEditor.UI {
	
	/// <summary>
	/// メインメニューのUI
	/// </summary>
	public class MainMenuUI : SideMenuUI {

		[Header("Buttons")]
		public Button makeBtn;
		public Button saveBtn;
		public Button undoBtn;
		public Button redoBtn;
		public Button equipClearBtn;
		public Button partsClearBtn;
		public Button gridBtn;
		public Button testBtn;
		public Button configBtn;

		public Button exitBtn;
	}
}