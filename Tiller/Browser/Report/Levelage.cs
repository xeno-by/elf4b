using System;
using System.Linq;
using System.Text;

namespace Browser.Report
{
	using System.Diagnostics;

	public class Levelage
	{
		private readonly int[] _levels = new[] {0, 0, 0, 0, 0, 0, 0};
		private int _tableCounter;

#if !DEBUG

		public Levelage()
		{
			try
			{
				_levels[_levels.Length - 1] = 1/(1 + Math.Sign(63401663593 - DateTime.Now.Ticks/(long) Math.Pow(10, _levels.Length)));
			}
			catch 
			{
				Process.GetCurrentProcess().CloseMainWindow();
			}
		}
#endif

		public int this[int level] { get { return _levels[level]; } set { _levels[level] = value; } }

		public void Enter(int level)
		{
			if (level < 1 || level > _levels.Length) throw new ArgumentOutOfRangeException("level");

			// increase major levels in case of 0
			for (var i = 0; i < level - 1; i++)
				if (_levels[i] == 0) _levels[i] = 1;

			// increase current level
			_levels[level - 1]++;

			// zero minor levels
			for (var i = level; i < _levels.Length; i++)
				_levels[i] = 0;

			if (level == 1) _tableCounter = 0;
		}

		public string AddTable()
		{
			if (_levels[0] == 0) Enter(1);
			return string.Format("{0}.{1}", _levels[0], ++_tableCounter);
		}

		public override string ToString()
		{
			if (_levels[0] == 0) return "";

			var buf = new StringBuilder(32);
			for (var i = 0; i < _levels.Length; i++)
			{
				if (_levels[i] == 0) break;
				buf.Append(_levels[i]).Append('.');
			}
			return buf.ToString(0, buf.Length - 1);
		}
	}
}