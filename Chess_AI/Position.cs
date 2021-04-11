using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_AI
{
	class Position
	{
		protected char column;
		protected int row;

		public Position()
		{
			this.column = 'A';
			this.row = 1;
		}

		public Position(char c, int r)
		{
			this.column = c;
			this.row = r;
		}

		public void setColumn(char c)
		{
			this.column = c;
		}

		public char getColumn()
		{
			return this.column;
		}

		public void setRow(int r)
		{
			this.row = r;
		}

		public int getRow()
		{
			return this.row;
		}
	}
}
