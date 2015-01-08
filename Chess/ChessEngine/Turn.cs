using System.Collections.Generic;

namespace Chess.ChessEngine
{
	/// <summary>
	/// Represents a single turn by a player.
	/// </summary>
	public class Turn
	{
		public List<PieceMove> Moves;

		public Turn(List<PieceMove> moves)
		{
			Moves = moves;
		}
	}
}
