chess
=====

Simple chess implementation

Classes

	Match - represents a match between two players 
		Properties
			Turns - List<Turn>
			CurrentTurn - int
			TopPlayerColor- PieceColor 
		Methods 
			ResetBoardToStartPosition()
			NextTurn()
			PreviousTurn()
			GoToTurn(int turn)
	Turn - a single players turn (white or black, not both) 
		Properties
			ActivePlayerColor - PieceColor 
			Moves - List<Move> (need list for multi-piece moves like castling) 
			IsCheck - bool
			IsCheckmate - bool
			IsStalemate - bool 
	PieceMove - the change in position of a piece as part of a turn. Multiple pieces can change state in a single turn
		Properties
			Piece - Piece
			StartPos - Position 
			EndPos - Position
	Piece - one of the pieces on the board
		Subclasses 
			Pawn
			Knight
			Bishop
			Rook
			Queen
			King
		Properties
			Color - PieceColor 
			CurrentPosition - Position
			Starting position - Position
			PieceType ? 
		Methods
			GetValidMoves
	Position - a location on the board
		Define as struct containing two ints? 
			A1 -> 0,0
			B1 -> 1,0
			Null when piece has been captured 
		Needs to be reversible when moving backwards through the match history 
	Player - one of the two players in the match 
		Properties
			PlayerName - string
			Color - PieceColor 
			Pieces - List<Piece>
		Methods
			PromotePawn(Pawn pawn)
			HasAvailableMoves
	PieceColor - enum to represent the colors white and black
		0 - white
		1 - black

Special moves
	Castle - rook and king move irregularly 
	En passant - irregular pawn capture
	Pawn promotion - pawn replaced by another piece 


Progression of a turn
	Select a piece with valid moves available
		If in check, this limits the available moves
	Make a valid move
	As a result of the move, look for check, checkmate or stalemate 

Should support existing format for storing chess matches 
