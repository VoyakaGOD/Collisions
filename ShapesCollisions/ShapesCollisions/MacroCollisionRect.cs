using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesCollisions
{
	public struct MacroCollisionRect
	{
		public VectorF Position;
		public VectorF Size;

		public MacroCollisionRect(VectorF Position, VectorF Size)
		{
			this.Position = Position;
			this.Size = Size;
		}

		public bool IsMacroCollision(MacroCollisionRect rect)
		{
			return ((Position.X + Size.X) > rect.Position.X) && ((rect.Position.X + rect.Size.X) > Position.X) &&
				   ((Position.Y + Size.Y) > rect.Position.Y) && ((rect.Position.Y + rect.Size.Y) > Position.Y);
		}
	}
}
