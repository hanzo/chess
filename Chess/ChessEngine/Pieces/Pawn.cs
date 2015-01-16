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

			var board = match.GetBoardState();

			var validTurns = new List<Turn>();

			// Pawns can only move in one direction. The pawn will move up or down rows depending on which player it belongs to.
			int rowForwardModifier = (match.CurrentActivePlayer == match.PlayerOnBottom) ? 1 : -1;

			// TODO: make sure to account for check/checkmate etc! 

			var newCol = CurrentPosition.Col;
			var newRow = CurrentPosition.Row + rowForwardModifier;

			// TODO: determine if a pawn should be promoted once it reaches the other side of the board. Remove this once promoting pawns is supported
			if (newRow < 0 || newRow > 7)
			{
				return new List<Turn>();
			}

			var oneSpaceAhead = board[newCol, newRow];
			if (oneSpaceAhead.Item1 == 0) // check if space is unoccupied
			{
				validTurns.Add(
					new Turn(
						new List<PieceMove>{
							new PieceMove(this, CurrentPosition, new Position(newCol, newRow))
						}));
			}

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

			// Check for pawn's ability to attack diagonally
			foreach (int columnModifier in new List<int> { 1, -1})
			{
				newCol = CurrentPosition.Col + columnModifier;

				if (newCol < 0 || newCol > 7)
					continue;

				newRow = CurrentPosition.Row + rowForwardModifier;

				var diagonalSpace = board[newCol, newRow];

				if (diagonalSpace.Item1 != 0 && (PieceColor)diagonalSpace.Item2 != Color) // check if space is occupied by an opposing piece
				{
					Piece capturedPiece = match.GetPieceAtPosition(newCol, newRow);

					validTurns.Add(
					new Turn(
						new List<PieceMove>{
							new PieceMove(this, CurrentPosition, new Position(newCol, newRow), isCapturing:true, isCaptured:false),  // capturing pawn
							new PieceMove(capturedPiece, capturedPiece.CurrentPosition, capturedPiece.CurrentPosition, isCapturing:false, isCaptured:true),  // captured piece
						}));
				}
			}

			return validTurns;
		}
	}
}
