chess
=====

Simple chess implementation


First stab at laying out class structure:

Classes
	Piece
		Subclasses 
			Pawn
			Knight
			Bishop
			Rook
			Queen
			King
		Properties
			IsWhite - bool
			Position - Position
			Starting position - Position
		Methods
			GetValidMoves
	Turn
		ActivePlayerIsWhite - bool
		ActivePieceStartPos - Position 
		ActivePieceEndPos - Position 
		What about multi-piece moves like castling? 
		IsCheck - bool
		IsCheckmate - bool
	Game
		Properties
			Turns - Array<Turn>
			StartingPlayerIsWhite - bool
			CurrentTurn - int
		Methods 
			ResetBoardToStartPosition()
			NextTurn()
			PreviousTurn()
			GoToTurn(int turn)
