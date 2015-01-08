using System;

namespace Chess.ChessEngine
{
	/// <summary>
	/// A location on the chess board.
	/// </summary>
	public struct Position
	{
		public int Col;
		public int Row;

		public Position(int col, int row)
		{
			Col = col;
			Row = row;
		}

		/// <summary>
		/// Convert the integer coordinates to chess board notation, e.g. 0,0 becomes A1.
		/// </summary>
		/// <returns>The chess notation for the location on the board, e.g. A1.</returns>
		public override string ToString()
		{
			return String.Format("{0}{1}", GetColumnCharacter(Col), Row + 1);
		}

		// Convert the column integer 0-7 to the corresponding column character A-H.
		private char GetColumnCharacter(int col)
		{
			// The ASCII code for 'A' is 65, 'B' is 66, etc.
			int asciiCode = col + 65;
			return (char) asciiCode;
		}
	}
}
