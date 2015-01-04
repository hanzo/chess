using System.Collections.Generic;

namespace Chess.ChessEngine
{
	public class Turn
	{
		public List<PieceMove> Moves;

		public Turn(List<PieceMove> moves)
		{
			Moves = moves;
		}
	}
}
