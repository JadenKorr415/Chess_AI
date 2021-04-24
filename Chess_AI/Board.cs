using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_AI
{
	class Board
	{
		// Piece types are represented by numbers 1-6 while color is represented by the number being positive for white and negative for black
		public const int PAWN_WHITE = 1,  KNIGHT_WHITE = 2,  BISHOP_WHITE = 3,  CASTLE_WHITE = 4,  QUEEN_WHITE = 5,  KING_WHITE = 6;
		public const int PAWN_BLACK = -1, KNIGHT_BLACK = -2, BISHOP_BLACK = -3, CASTLE_BLACK = -4, QUEEN_BLACK = -5, KING_BLACK = -6;

		/*
		 * board is set so that first int [*,] is the column, generally defined by letters A-H on a standard chess board are mapped to values 0-7.
		 * second int [,*] is the row, generally defined by numbers 1-8 on a standard chess board are mapped to values 0-7
		 * So on B6 on a standard board would be located at board[1][5] and E2 would be at board[4][1]
		 * empty spaces are marked as 0 and filled spaces are marked by the above designations; white is positive black is negative
		 */
		private int[,] board;

		/* Constructor
		 * Initializes the board object and resets the board to a clean slate
		 */
		public Board()
		{
			board = new int[8,8];
			Reset();
		}

		/*
		 * Resets the board for a new game
		 */
		public void Reset()
		{
			// Setup white main row
			board[0, 0] = CASTLE_WHITE;
			board[1, 0] = KNIGHT_WHITE;
			board[2, 0] = BISHOP_WHITE;
			board[3, 0] = QUEEN_WHITE;
			board[4, 0] = KING_WHITE;
			board[5, 0] = BISHOP_WHITE;
			board[6, 0] = KNIGHT_WHITE;
			board[7, 0] = CASTLE_WHITE;
			for (int i = 0; i < 8; i++) { board[i, 1] = PAWN_WHITE; } // populate white pawns

			// Setup black main row
			board[0, 7] = CASTLE_BLACK;
			board[1, 7] = KNIGHT_BLACK;
			board[2, 7] = BISHOP_BLACK;
			board[3, 7] = QUEEN_BLACK;
			board[4, 7] = KING_BLACK;
			board[5, 7] = BISHOP_BLACK;
			board[6, 7] = KNIGHT_BLACK;
			board[7, 7] = CASTLE_BLACK;
			for (int i = 0; i < 8; i++) { board[i, 6] = PAWN_BLACK; } // populate black pawns
		}
		

		/*
		 * Unchecked and Unsafe method to move a piece on the board
		 */
		public void Move(Tuple<int, int> source, Tuple<int, int> destination)
		{
			board[destination.Item1, destination.Item2] = board[source.Item1, source.Item2];
			board[source.Item1, source.Item2] = 0;
		}

		/*
		 * @param color: 1 if white, -1 if black
		 */
		public List<Tuple<int,int>> GetPawnMoves(Tuple<int,int> position, int color)
		{
			List<Tuple<int, int>> moves = new List<Tuple<int, int>>();
			if (color > 0 && position.Item2 < 7) // if pawn is at the top of the board it cannot move
			{
				// check if position in front of pawn is valid
				if(board[position.Item1, position.Item2 + 1] == 0) { moves.Add(new Tuple<int, int>(position.Item1, position.Item2 + 1)); }
				// check if pawn is at the start and can jump
				if(position.Item2 == 1 && board[position.Item1, position.Item2 + 2] == 0) { moves.Add(new Tuple<int, int>(position.Item1, position.Item2 + 2)); }
				// check if pawn attack right is valid
				if(position.Item1 < 7 && board[position.Item1 + 1, position.Item2 + 1] < 0) { moves.Add(new Tuple<int, int>(position.Item1 + 1, position.Item2 + 1)); }
				// check if pawn attack left is valid
				if (position.Item1 > 0 && board[position.Item1 - 1, position.Item2 + 1] < 0) { moves.Add(new Tuple<int, int>(position.Item1 - 1, position.Item2 + 1)); }
			}
			else if (position.Item2 > 0) // if pawn is at the bottom of the board it cannot move
			{
				// check if position in front of pawn is valid
				if (board[position.Item1, position.Item2 - 1] == 0) { moves.Add(new Tuple<int, int>(position.Item1, position.Item2 - 1)); }
				// check if pawn is at the start and can jump
				if (position.Item2 == 6 && board[position.Item1, position.Item2 - 2] == 0) { moves.Add(new Tuple<int, int>(position.Item1, position.Item2 - 2)); }
				// check if pawn attack right is valid
				if (position.Item1 < 7 && board[position.Item1 + 1, position.Item2 - 1] > 0) { moves.Add(new Tuple<int, int>(position.Item1 + 1, position.Item2 - 1)); }
				// check if pawn attack left is valid
				if (position.Item1 > 0 && board[position.Item1 - 1, position.Item2 - 1] > 0) { moves.Add(new Tuple<int, int>(position.Item1 - 1, position.Item2 - 1)); }
			}
			//TODO: en passant implementation
			return moves;
		}

		public List<Tuple<int,int>> GetCastleMoves(Tuple<int,int> position, int color)
		{
			List<Tuple<int, int>> moves = new List<Tuple<int, int>>();
			/*
			for(int i = position.Item1 + 1; i <= 7; i++) // check to the right of the castle
			{
				if(board[i, position.Item2] == 0) { moves.Add(new Tuple<int, int>(i, position.Item2)); } // empty space, valid and continue
				if(board[i, position.Item2] * color < 0) { moves.Add(new Tuple<int, int>(i, position.Item2)); break; } // enemy space, valid move; exit loop
				else { break; } // ally space, invalid move; exit
			}
			for (int i = position.Item1 - 1; i >= 0; i--) // check to the left of the castle
			{
				if (board[i, position.Item2] == 0) { moves.Add(new Tuple<int, int>(i, position.Item2)); } // empty space, valid and continue
				if (board[i, position.Item2] * color < 0) { moves.Add(new Tuple<int, int>(i, position.Item2)); break; } // enemy space, valid move; exit loop
				else { break; } // ally space, invalid move; exit
			}
			for (int i = position.Item2 + 1; i <= 7; i++) // check above the castle
			{
				if (board[position.Item1, i] == 0) { moves.Add(new Tuple<int, int>(position.Item1, i)); } // empty space, valid and continue
				if (board[position.Item1, i] * color < 0) { moves.Add(new Tuple<int, int>(position.Item1, i)); break; } // enemy space, valid move; exit loop
				else { break; } // ally space, invalid move; exit
			}
			for (int i = position.Item2 - 1; i >= 0; i--) // check below the castle
			{
				if (board[position.Item1, i] == 0) { moves.Add(new Tuple<int, int>(position.Item1, i)); } // empty space, valid and continue
				if (board[position.Item1, i] * color < 0) { moves.Add(new Tuple<int, int>(position.Item1, i)); break; } // enemy space, valid move; exit loop
				else { break; } // ally space, invalid move; exit
			}
			*/
			bool[] dir = new bool[4] { true, true, true, true }; //up, down, left, right
			for(int i = 1; i < 8; i++)
			{
				// Checking above
				if (position.Item2 + i < 8 && board[position.Item1, position.Item2 + i] == 0)  // empty space, valid and continue
				{
					moves.Add(new Tuple<int, int>(position.Item1, position.Item2 + i));
				}
				else
				{
					if (position.Item2 + i < 8 && board[position.Item1, position.Item2 + i] * color < 0) // enemy space, valid move; exit loop
					{
						moves.Add(new Tuple<int, int>(position.Item1, position.Item2 + i));
					}
					dir[0] = false;
					if (!(dir[1] || dir[2] || dir[3])) { break; } // all directions finished
				}

				// Checking below
				if (position.Item2 - i <= 0 && board[position.Item1, position.Item2 - i] == 0)  // empty space, valid and continue
				{
					moves.Add(new Tuple<int, int>(position.Item1, position.Item2 - i));
				}
				else
				{
					if (position.Item2 - i >= 0 && board[position.Item1, position.Item2 - i] * color < 0) // enemy space, valid move; exit loop
					{
						moves.Add(new Tuple<int, int>(position.Item1, position.Item2 - i));
					}
					dir[1] = false;
					if (!(dir[0] || dir[2] || dir[3])) { break; } // all directions finished
				}

				// Checking to the left
				if (position.Item1 - i >= 0 && board[position.Item1 - i, position.Item2] == 0)  // empty space, valid and continue
				{
					moves.Add(new Tuple<int, int>(position.Item1 - i, position.Item2));
				}
				else
				{
					if (position.Item1 -i >= 0 && board[position.Item1 - i, position.Item2] * color < 0) // enemy space, valid move; exit loop
					{
						moves.Add(new Tuple<int, int>(position.Item1 - i, position.Item2));
					}
					dir[2] = false;
					if (!(dir[0] || dir[1] || dir[3])) { break; } // all directions finished
				}

				// Checking to the right
				if (position.Item1 + i < 8 && board[position.Item1 + i, position.Item2] == 0)  // empty space, valid and continue
				{ 
					moves.Add(new Tuple<int, int>(position.Item1 + i, position.Item2)); 
				}
				else {
					if (position.Item1 + i < 8 && board[position.Item1 + i, position.Item2] * color < 0) // enemy space, valid move; exit loop
					{
						moves.Add(new Tuple<int, int>(position.Item1 + i, position.Item2)); 
					}
					dir[3] = false;
					if(!(dir[0] || dir[1] || dir[2])) { break; } // all directions finished
				} 
			}

			//TODO: Castling implementation
			return moves;
		}

		public List<Tuple<int, int>> GetKnightMoves(Tuple<int, int> position, int color)
		{
			List<Tuple<int, int>> moves = new List<Tuple<int, int>>();

			if(position.Item2 + 2 < 8) // checking above positions
			{
				if(board[position.Item1 + 1, position.Item2 + 2] == 0 || board[position.Item1 + 1, position.Item2 + 2] * color < 0) // position is either empty or an opponent space
				{
					moves.Add(new Tuple<int,int>(position.Item1 + 1, position.Item2 + 2));
				}

				if (board[position.Item1 - 1, position.Item2 + 2] == 0 || board[position.Item1 - 1, position.Item2 + 2] * color < 0) // position is either empty or an opponent space
				{
					moves.Add(new Tuple<int, int>(position.Item1 - 1, position.Item2 + 2));
				}
			}

			if (position.Item2 - 2 >= 0) // checking below positions
			{
				if (board[position.Item1 + 1, position.Item2 - 2] == 0 || board[position.Item1 + 1, position.Item2 - 2] * color < 0) // position is either empty or an opponent space
				{
					moves.Add(new Tuple<int, int>(position.Item1 + 1, position.Item2 - 2));
				}

				if (board[position.Item1 - 1, position.Item2 - 2] == 0 || board[position.Item1 - 1, position.Item2 - 2] * color < 0) // position is either empty or an opponent space
				{
					moves.Add(new Tuple<int, int>(position.Item1 - 1, position.Item2 - 2));
				}
			}

			if (position.Item1 - 2 >= 0) // checking left positions
			{
				if (board[position.Item1 - 2, position.Item2 + 1] == 0 || board[position.Item1 - 2, position.Item2 + 1] * color < 0) // position is either empty or an opponent space
				{
					moves.Add(new Tuple<int, int>(position.Item1 - 2, position.Item2 - 1));
				}

				if (board[position.Item1 - 2, position.Item2 - 1] == 0 || board[position.Item1 - 2, position.Item2 - 1] * color < 0) // position is either empty or an opponent space
				{
					moves.Add(new Tuple<int, int>(position.Item1 - 2, position.Item2 - 1));
				}
			}

			if (position.Item1 + 2 < 8) // checking right positions
			{
				if (board[position.Item1 + 2, position.Item2 + 1] == 0 || board[position.Item1 + 2, position.Item2 + 1] * color < 0) // position is either empty or an opponent space
				{
					moves.Add(new Tuple<int, int>(position.Item1 + 2, position.Item2 + 1));
				}

				if (board[position.Item1 + 2, position.Item2 - 1] == 0 || board[position.Item1 + 2, position.Item2 - 1] * color < 0) // position is either empty or an opponent space
				{
					moves.Add(new Tuple<int, int>(position.Item1 + 2, position.Item2 - 1));
				}
			}

			return moves;
		}

		public List<Tuple<int, int>> GetCBishopMoves(Tuple<int, int> position, int color)
		{
			List<Tuple<int, int>> moves = new List<Tuple<int, int>>();

			// bools to see if that direction has reached its end
			// respectively up right, up left, down right, down left
			bool[] ends = new bool[] { true, true, true, true };
			bool[] dir; // U D L R

			for (int i = 1; i < 8; i++)
			{
				// checks if the potential move has gone beyond the bounds of the board
				dir = new bool[4] { position.Item2 + i > 7, position.Item2 - i < 0, position.Item1 - i < 0, position.Item1 + i > 7 };
				ends[0] = !(!ends[0] || (dir[0] || dir[3]));
				ends[1] = !(!ends[1] || (dir[0] || dir[2]));
				ends[2] = !(!ends[2] || (dir[1] || dir[3]));
				ends[3] = !(!ends[3] || (dir[1] || dir[2]));


				//check diagonal up right
				if (ends[0] && board[position.Item1 + i, position.Item2 + i] == 0) // empty space, valid and continue
				{ 
					moves.Add(new Tuple<int, int>(position.Item1 + i, position.Item2 + i)); 
				} 
				else if (ends[0] && board[position.Item1 + i, position.Item2 + i] * color < 0) // enemy space, valid move; direction complete
				{ 
					moves.Add(new Tuple<int, int>(position.Item1 + i, position.Item2 + i));
					ends[0] = false;
				} 
				else if(ends[0]) // ally space, invalid move; end direction
				{
					ends[0] = false;
				}

				//check diagonal up left
				if (ends[1] && board[position.Item1 - i, position.Item2 + i] == 0) // empty space, valid and continue
				{
					moves.Add(new Tuple<int, int>(position.Item1 - i, position.Item2 + i));
				}
				else if (ends[1] && board[position.Item1 - i, position.Item2 + i] * color < 0) // enemy space, valid move; direction complete
				{
					moves.Add(new Tuple<int, int>(position.Item1 - i, position.Item2 + i));
					ends[1] = false;
				}
				else if (ends[1]) // ally space, invalid move; end direction
				{
					ends[1] = false;
				}

				//check diagonal down right
				if (ends[2] && board[position.Item1 + i, position.Item2 - i] == 0) // empty space, valid and continue
				{
					moves.Add(new Tuple<int, int>(position.Item1 + i, position.Item2 - i));
				}
				else if (ends[2] && board[position.Item1 + i, position.Item2 - i] * color < 0) // enemy space, valid move; direction complete
				{
					moves.Add(new Tuple<int, int>(position.Item1 + i, position.Item2 - i));
					ends[2] = false;
				}
				else if (ends[2]) // ally space, invalid move; end direction
				{
					ends[2] = false;
				}

				//check diagonal down left
				if (ends[3] && board[position.Item1 - i, position.Item2 - i] == 0) // empty space, valid and continue
				{
					moves.Add(new Tuple<int, int>(position.Item1 - i, position.Item2 - i));
				}
				else if (ends[3] && board[position.Item1 - i, position.Item2 - i] * color < 0) // enemy space, valid move; direction complete
				{
					moves.Add(new Tuple<int, int>(position.Item1 - i, position.Item2 - i));
					ends[3] = false;
				}
				else if (ends[3]) // ally space, invalid move; end direction
				{
					ends[3] = false;
				}

				//if all directions are ended then exit
				if(!(ends[0] && ends[1] && ends[2] && ends[3])) { break; }
			}

			return moves;
		}

		public List<Tuple<int, int>> GetQueenMoves(Tuple<int, int> position, int color)
		{
			List<Tuple<int, int>> moves = new List<Tuple<int, int>>();

			//TODO: stub

			return moves;
		}

		public List<Tuple<int, int>> GetKingMoves(Tuple<int, int> position, int color)
		{
			List<Tuple<int, int>> moves = new List<Tuple<int, int>>();

			//TODO: stub

			return moves;
		}

		public bool InCheck(Tuple<int,int> position, int color)
		{
			//TODO: stub

			return false;
		}
	}
}
