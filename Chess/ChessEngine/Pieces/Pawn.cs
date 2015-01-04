using System;
using System.Collections.Generic;

namespace Chess.ChessEngine.Pieces
{
	class Pawn : Piece
	{
		public Pawn(PieceColor color, Position startPos) : base(color, startPos)
		{
		}

		public override PieceType Type
		{
			get { return PieceType.Pawn; }
		}

		public override List<Turn> GetValidTurns(Match match)
		{
			if (IsCaptured)
				throw new Exception("GetValidTurns was called on a captured piece");

			var board = match.BoardState(match.CurrentTurn);

			var validTurns = new List<Turn>();

			// Pawns can only move in one direction. The pawn will move up or down rows depending on which player it belongs to.
			int rowForwardModifier = (match.ActivePlayer == match.PlayerOnBottom) ? 1 : -1;


			// TODO: make sure to account for check/checkmate etc! 

			var newCol = CurrentPosition.Col;
			var newRow = CurrentPosition.Row + rowForwardModifier;
			var oneSpaceAhead = board[newCol, newRow];
			if (oneSpaceAhead.Item1 == 0) // check if space is unoccupied
			{
				validTurns.Add(
					new Turn(
						new List<PieceMove>{
							new PieceMove(this, CurrentPosition, new Position(newCol, newRow))
						}));
			}

			// TODO: verify this comparison works as expected
			// If a pawn hasn't yet moved, it can move two spaces instead of one.
			if (CurrentPosition.Equals(StartingPosition))
			{
				newCol = CurrentPosition.Col;
				newRow = CurrentPosition.Row + (rowForwardModifier * 2);
				var twoSpacesAhead = board[newCol, newRow];

				if (twoSpacesAhead.Item1 == 0) // check if space is unoccupied
				{
					validTurns.Add(
					new Turn(
						new List<PieceMove>{
							new PieceMove(this, CurrentPosition, new Position(newCol, newRow))
						}));
				}
			}

			// TODO: check for pawn's ability to attack diagonally

			return validTurns;
		}
	}
}
