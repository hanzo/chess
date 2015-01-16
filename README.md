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
			PieceType
		Methods
			GetValidMoves
	Position - a location on the board
		Define as struct containing two ints
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

How to store move history? Two approaches:
	1. Store a numeric representation of the entire board at each turn
		Pros: simpler, no additional computation needed to jump to the nth turn
	2. Store objects representing each piece as well as a list of turns as operations that can be applied to the board. To set the board to the nth turn, step forward or backward through the list of turns, applying each operation to board as you go
		Pros: 
			Less memory used - don't need to store 8x8 integer array for every turn
			Can store state for each piece, e.g. where did the piece start, how many other pieces has it captured, etc
			
I had originally planned to use the second approach, but now that I've started writing the code I've found that I need a representation of the full board state (e.g. 8x8 integer array) in order to calculate valid moves and to allow callers of the chess engine to be able to display the current board state without needing to expose the entire class hierarchy of the engine (match, players and their pieces). I still like the idea of keeping objects to represent each piece since it will allow interesting state tracking - for example at the end of the game you could answer questions like "which type of piece did White capture the most pieces with". 

Special moves
	Castle - rook and king move irregularly 
	En passant - irregular pawn capture
	Pawn promotion - pawn replaced by another piece 


Progression of a turn
	Select a piece with valid moves available
		If in check, this limits the available moves
	Make a valid move
	As a result of the move, look for check, checkmate or stalemate 

Should support PGN format for chess match history  
Need to think more about access levels - only a few classes need to be public

