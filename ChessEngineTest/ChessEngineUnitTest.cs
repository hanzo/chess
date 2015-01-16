using System.Collections.Generic;
using Chess.ChessEngine;
using NUnit.Framework;

namespace ChessEngineTest
{
	[TestFixture]
	public class ChessEngineUnitTest
	{
		[Test]
		public void TurnZero_MatchState()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.White));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.White));
			Assert.That(match.LastTurn, Is.EqualTo(0));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(0));
		}

		[Test]
		public void TurnZero_WhitePlayerOnTop()
		{
			Match match = new Match(PieceColor.White, "White", "Black");
			Assert.That(match.PlayerOnTop.Color, Is.EqualTo(PieceColor.White));
		}

		[Test]
		public void TurnZero_BlackPlayerOnTop()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			Assert.That(match.PlayerOnTop.Color, Is.EqualTo(PieceColor.Black));
		}

		[Test]
		public void TurnZero_BothPlayersHaveSixteenPieces()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			Assert.That(match.WhitePlayer.Pieces.Count, Is.EqualTo(16));
			Assert.That(match.WhitePlayer.ActivePieces.Count, Is.EqualTo(16));
			Assert.That(match.WhitePlayer.CapturedPieces.Count, Is.EqualTo(0));
			Assert.That(match.BlackPlayer.Pieces.Count, Is.EqualTo(16));
			Assert.That(match.BlackPlayer.ActivePieces.Count, Is.EqualTo(16));
			Assert.That(match.BlackPlayer.CapturedPieces.Count, Is.EqualTo(0));
		}

		[Test]
		public void TurnOne_MatchState()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			var validTurns = match.CurrentValidTurns;
			match.ExecuteNewTurn(validTurns[0]);
			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.LastTurn, Is.EqualTo(1));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(1));
		}

		[Test]
		public void TurnOne_PreviousTurn()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			var validTurns = match.CurrentValidTurns;
			match.ExecuteNewTurn(validTurns[0]);
			match.PreviousTurn();
			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.White));
			Assert.That(match.LastTurn, Is.EqualTo(1));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(0));
		}

		[Test]
		public void TurnOne_PreviousTurn_NextTurn()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			var validTurns = match.CurrentValidTurns;
			match.ExecuteNewTurn(validTurns[0]);
			match.PreviousTurn();
			match.NextTurn();
			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.LastTurn, Is.EqualTo(1));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(1));
		}

		[Test]
		public void TurnOne_GoToStart_NextTurn()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			var validTurns = match.CurrentValidTurns;
			match.ExecuteNewTurn(validTurns[0]);
			match.GoToStart();
			match.NextTurn();
			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.LastTurn, Is.EqualTo(1));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(1));
		}

		[Test]
		public void TurnOne_GoToStart_GoToEnd()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			var validTurns = match.CurrentValidTurns;
			match.ExecuteNewTurn(validTurns[0]);
			match.GoToStart();
			match.GoToEnd();
			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.LastTurn, Is.EqualTo(1));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(1));
		}

		[Test]
		public void TurnOne_GoToStart_GoToTurnOne()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			var validTurns = match.CurrentValidTurns;
			match.ExecuteNewTurn(validTurns[0]);
			match.GoToStart();
			match.GoToTurn(1);
			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.LastTurn, Is.EqualTo(1));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(1));
		}

		[Test]
		public void CapturePawn()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A2 -> A4
			match.ExecuteNewTurn(match.CurrentValidTurns[3]); // B7 -> B5
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A4 takes B5

			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.LastTurn, Is.EqualTo(3));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(3));
			Assert.That(match.WhitePlayer.ActivePieces.Count, Is.EqualTo(16));
			Assert.That(match.WhitePlayer.CapturedPieces.Count, Is.EqualTo(0));
			Assert.That(match.BlackPlayer.ActivePieces.Count, Is.EqualTo(15));
			Assert.That(match.BlackPlayer.CapturedPieces.Count, Is.EqualTo(1));
		}

		[Test]
		public void CapturePawn_PreviousTurn()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A2 -> A4
			match.ExecuteNewTurn(match.CurrentValidTurns[3]); // B7 -> B5
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A4 takes B5
			match.PreviousTurn();

			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.White));
			Assert.That(match.LastTurn, Is.EqualTo(3));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(2));
			Assert.That(match.WhitePlayer.ActivePieces.Count, Is.EqualTo(16));
			Assert.That(match.WhitePlayer.CapturedPieces.Count, Is.EqualTo(0));
			Assert.That(match.BlackPlayer.ActivePieces.Count, Is.EqualTo(16));
			Assert.That(match.BlackPlayer.CapturedPieces.Count, Is.EqualTo(0));
		}

		[Test]
		public void CapturePawn_PreviousTurn_NextTurn()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A2 -> A4
			match.ExecuteNewTurn(match.CurrentValidTurns[3]); // B7 -> B5
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A4 takes B5
			match.PreviousTurn();
			match.NextTurn();

			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.LastTurn, Is.EqualTo(3));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(3));
			Assert.That(match.WhitePlayer.ActivePieces.Count, Is.EqualTo(16));
			Assert.That(match.WhitePlayer.CapturedPieces.Count, Is.EqualTo(0));
			Assert.That(match.BlackPlayer.ActivePieces.Count, Is.EqualTo(15));
			Assert.That(match.BlackPlayer.CapturedPieces.Count, Is.EqualTo(1));
		}

		[Test]
		public void CapturePawn_PreviousTurn_GoToEnd()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A2 -> A4
			match.ExecuteNewTurn(match.CurrentValidTurns[3]); // B7 -> B5
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A4 takes B5
			match.PreviousTurn();
			match.GoToEnd();

			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.LastTurn, Is.EqualTo(3));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(3));
			Assert.That(match.WhitePlayer.ActivePieces.Count, Is.EqualTo(16));
			Assert.That(match.WhitePlayer.CapturedPieces.Count, Is.EqualTo(0));
			Assert.That(match.BlackPlayer.ActivePieces.Count, Is.EqualTo(15));
			Assert.That(match.BlackPlayer.CapturedPieces.Count, Is.EqualTo(1));
		}

		[Test]
		public void CapturePawn_PreviousTurn_GoToTurn3()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A2 -> A4
			match.ExecuteNewTurn(match.CurrentValidTurns[3]); // B7 -> B5
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A4 takes B5
			match.PreviousTurn();
			match.GoToTurn(3);

			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.LastTurn, Is.EqualTo(3));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(3));
			Assert.That(match.WhitePlayer.ActivePieces.Count, Is.EqualTo(16));
			Assert.That(match.WhitePlayer.CapturedPieces.Count, Is.EqualTo(0));
			Assert.That(match.BlackPlayer.ActivePieces.Count, Is.EqualTo(15));
			Assert.That(match.BlackPlayer.CapturedPieces.Count, Is.EqualTo(1));
		}

		[Test]
		public void CapturePawn_GoToStart_GoToEnd()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A2 -> A4
			match.ExecuteNewTurn(match.CurrentValidTurns[3]); // B7 -> B5
			match.ExecuteNewTurn(match.CurrentValidTurns[1]); // A4 takes B5
			match.GoToStart();
			match.GoToEnd();

			Assert.That(match.CurrentActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.ViewingActivePlayer.Color, Is.EqualTo(PieceColor.Black));
			Assert.That(match.LastTurn, Is.EqualTo(3));
			Assert.That(match.ViewingLastTurn, Is.EqualTo(3));
			Assert.That(match.WhitePlayer.ActivePieces.Count, Is.EqualTo(16));
			Assert.That(match.WhitePlayer.CapturedPieces.Count, Is.EqualTo(0));
			Assert.That(match.BlackPlayer.ActivePieces.Count, Is.EqualTo(15));
			Assert.That(match.BlackPlayer.CapturedPieces.Count, Is.EqualTo(1));
		}
	}
}
