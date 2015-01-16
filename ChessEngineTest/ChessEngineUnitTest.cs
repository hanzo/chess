using Chess.ChessEngine;
using NUnit.Framework;

namespace ChessEngineTest
{
	[TestFixture]
	public class ChessEngineUnitTest
	{
		[Test]
		public void TurnZeroActivePlayer()
		{
			Match match = new Match(PieceColor.Black, "White", "Black");
			Assert.That(match.ActivePlayer.Color, Is.EqualTo(PieceColor.White));
		}
	}
}
