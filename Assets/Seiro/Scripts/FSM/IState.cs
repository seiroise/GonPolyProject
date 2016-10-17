using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seiro.Scripts.FSM {

	/// <summary>
	/// FSM等で使用する基本的な状態インタフェース
	/// </summary>
	public interface IState {

		void Enter();

		void Exit();
	}
}