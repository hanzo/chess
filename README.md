chess
=====

Simple chess implementation

Second attempt at class layout, preparing to start writing code

Classes

	Game - represents a match between two players 
		Properties
			Turns - List<Turn>
			StartingPlayerIsWhite - bool
			CurrentTurn - int
		Methods 
			ResetBoardToStartPosition()
			NextTurn()
			PreviousTurn()
			GoToTurn(int turn)
	Turn - a single players turn (white or black, not both) 
		Properties
			ActivePlayerIsWhite - bool
			Moves - List<Move>
			What about multi-piece moves like castling? 
			IsCheck - bool
			IsCheckmate - bool
			IsStalemate - bool 
	Move - the change in position of a piece as part of a turn. Multiple moves can happen in a single turn
		Properties
			Piece - Piece
			StartPos - Position 
			EndPos - nullable Position (null for captured pieces)  
	Piece - one of the pieces on the board
		Subclasses 
			Pawn
			Knight
			Bishop
			Rook
			Queen
			King
		Properties
			IsWhite - bool
			CurrentPosition - Position
			Starting position - Position
		Methods
			GetValidMoves
	Position - a location on the board
		Define as struct containing two ints? 
		A1 == 0,0
		B1 == 1,0
	Player - one of the two players in the match 
		Properties
			IsWhite - bool
			Pieces - List<Piece>
		Methods
			PromotePawn(Pawn pawn)
			HasAvailableMoves

Special moves
	Castle - rook and king move irregularly 
	En passant - irregular pawn capture
	Pawn promotion - pawn replaced by another piece 

