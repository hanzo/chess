using System.Collections.Generic;
using Chess.ChessEngine.Pieces;

namespace Chess.ChessEngine
{
	/// <summary>
	/// One of the two players in a chess match.
	/// </summary>
	public class Player
	{
		public string Name { get; private set; }

		public PieceColor Color { get; private set; }

		public List<Piece> Pieces { get; set; }

		public Player(PieceColor color, string name)
		{
			Color = color;
			Name = name;
			Pieces = new List<Piece>();
		}
	}
}
