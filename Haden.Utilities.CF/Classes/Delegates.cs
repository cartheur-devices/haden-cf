using System;
using System.Collections.Generic;
using System.Text;

namespace Haden.Utilities.CF {
	public delegate void LargeOperationStart(object sender, int depth, string description, int max);

	public delegate void LargeOperationStatusUpdate(object sender, int depth, string description, int count, out bool cancel);

	public delegate void LargeOperationEnd(object sender, int depth, string description);
}
