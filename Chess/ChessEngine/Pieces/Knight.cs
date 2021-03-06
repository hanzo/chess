﻿using System;
using System.Collections.Generic;

namespace Chess.ChessEngine.Pieces
{
	class Knight : Piece
	{
		public Knight(PieceColor color, Position startPos) : base(color, startPos)
		{
		}

		public override PieceType Type
		{
			get { return PieceType.Knight; }
		}

		public override List<Turn> GetValidTurns(Match match)
		{
			return new List<Turn>();
		}
	}
}
