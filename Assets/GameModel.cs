using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;


public class GameModel : MonoBehaviour // This class is used to store a position model
{
    public static ulong White_Pawns;
    public static ulong White_Knights;
    public static ulong White_Bishops;
    public static ulong White_Rooks;
    public static ulong White_Queens;
    public static ulong White_Kings;    
    public static ulong Black_Pawns;
    public static ulong Black_Knights;
    public static ulong Black_Bishops;
    public static ulong Black_Rooks;
    public static ulong Black_Queens;
    public static ulong Black_Kings;

    public static ulong Full_Board;
    public static ulong White_Pieces;
    public static ulong Black_Pieces;

    void Awake()
    {
        generateStartingPosition();
        ulong knight_loc = (ulong)1 << 63;
        ulong legalMoves = knight_legal_moves(knight_loc,White_Pieces);

    }

    static void generateStartingPosition()
    {
        // WhitePieces
        White_Pawns |= ((ulong)1 << 8);
        White_Pawns |= ((ulong)1 << 9);
        White_Pawns |= ((ulong)1 << 10);
        White_Pawns |= ((ulong)1 << 11);
        White_Pawns |= ((ulong)1 << 12);
        White_Pawns |= ((ulong)1 << 13);
        White_Pawns |= ((ulong)1 << 14);
        White_Pawns |= ((ulong)1 << 15);

        White_Rooks |= ((ulong)1 << 0); White_Rooks |= ((ulong)1 << 7);
        White_Bishops |= ((ulong)1 << 2); White_Bishops |= ((ulong)1 << 5);
        White_Knights |= ((ulong)1 << 1); White_Knights |= ((ulong)1 << 6);
        White_Queens |= ((ulong)1 << 3);
        White_Kings |= ((ulong)1 << 4);

        // Black Pieces 
        Black_Pawns |= ((ulong)1 << 48);
        Black_Pawns |= ((ulong)1 << 49);
        Black_Pawns |= ((ulong)1 << 50);
        // Black_Pawns |= ((ulong)1 << 51);
        Black_Pawns |= ((ulong)1 << 52);
        Black_Pawns |= ((ulong)1 << 53);
        Black_Pawns |= ((ulong)1 << 54);
        Black_Pawns |= ((ulong)1 << 55);    

        Black_Rooks |= ((ulong)1 << 56); Black_Rooks |= ((ulong)1 << 63);
        Black_Bishops |= ((ulong)1 << 58); Black_Bishops |= ((ulong)1 << 61);
        Black_Knights |= ((ulong)1 << 57); Black_Knights |= ((ulong)1 << 62);
        Black_Queens |= ((ulong)1 << 59);
        Black_Kings |= ((ulong)1 << 60);   

        White_Pieces = White_Knights | White_Kings | White_Pawns | White_Rooks | White_Bishops |  White_Queens;
        Black_Pieces = Black_Rooks | Black_Bishops | Black_Knights | Black_Kings | Black_Queens | Black_Pawns;
        Full_Board = White_Pieces | Black_Pieces;
    }

    // Returns a ulong with 1 bits in the desired rank
    static ulong maskRow(int rank)
    {
        ulong mask = new ulong();
        for (int i = 0 ; i <= 63 ; i++)
        {
            if (Mathf.Floor(i/8) == rank)
            {
                mask |= ((ulong)1 << i);
            }
        }
        return mask;
    }
    static ulong maskColumn(int rank)
    {
        ulong mask = new ulong();
        for (int i = 0 ; i <= 63 ; i++)
        {
            if (i%8 == rank)
            {
                mask |= ((ulong)1 << i);
            }
        }
        return mask;
    }
    // Returns 0s across the requested rank
    static ulong clearRow(int row)
    {
        ulong res = ulong.MaxValue - maskRow(row); //Equivalent to bitwise NOT
        return res;
    }
    static ulong clearColumn(int column)
    {
        ulong res = ulong.MaxValue - maskColumn(column); //Equivalent to bitwise NOT
        return res;
    }
    public static ulong king_legal_moves(in ulong king_loc,in ulong sidePieces)
    {
        
        ulong king_clip_file_a = king_loc & clearColumn(0);
        ulong king_clip_file_h = king_loc & clearColumn(7);
        
        ulong spot_1 = king_clip_file_a << 7;
        ulong spot_2 = king_loc << 8;
        ulong spot_3 = king_clip_file_h << 9;
        ulong spot_4 = king_clip_file_h << 1;
        ulong spot_5 = king_clip_file_h >> 7;
        ulong spot_6 = king_loc >> 8;
        ulong spot_7 = king_clip_file_a >> 9;
        ulong spot_8 = king_clip_file_a >> 1;     

        ulong king_moves = spot_1 | spot_2 | spot_3 | spot_4 | spot_5 | spot_6 | spot_7 | spot_8;
        ulong KingValid = king_moves & (ulong.MaxValue - sidePieces);
        
        // It does not consider the places where it is being checked
        return KingValid;
    }
    public static ulong knight_legal_moves(in ulong knight_loc,in ulong sidePieces)
    {
        ulong spot_1_clip = clearColumn(0) & clearColumn(1);
        ulong spot_2_clip = clearColumn(0);
        ulong spot_3_clip = clearColumn(7);
        ulong spot_4_clip = clearColumn(7) & clearColumn(6);
        ulong spot_5_clip = clearColumn(7) & clearColumn(6);;
        ulong spot_6_clip = clearColumn(7);
        ulong spot_7_clip = clearColumn(0);
        ulong spot_8_clip = clearColumn(0) & clearColumn(1);

        ulong spot_1 = (knight_loc & spot_1_clip) << 6;
        ulong spot_2 = (knight_loc & spot_2_clip) << 15;
        ulong spot_3 = (knight_loc & spot_3_clip) << 17;
        ulong spot_4 = (knight_loc & spot_4_clip) << 10;
        ulong spot_5 = (knight_loc & spot_5_clip) >> 6;
        ulong spot_6 = (knight_loc & spot_6_clip) >> 15;
        ulong spot_7 = (knight_loc & spot_7_clip) >> 17;
        ulong spot_8 = (knight_loc & spot_8_clip) >> 10;      
        ulong knight_moves = spot_1 | spot_2 | spot_3 | spot_4 | spot_5 | spot_6 | spot_7 | spot_8;
        ulong KnightValid = knight_moves & (ulong.MaxValue - sidePieces);

        return KnightValid;

    }

    public static ulong white_pawn_legal_moves(in ulong pawn_loc, in ulong sidePieces)
    {
        ulong spot_1_clip = clearColumn(7);
        ulong spot_3_clip = clearColumn(0);

        ulong spot_1 = (pawn_loc & spot_1_clip) << 9;
        ulong spot_2 = pawn_loc << 8;
        ulong spot_3 = (pawn_loc & spot_3_clip) << 7;        
        

        ulong pawn_moves = spot_1 | spot_2 | spot_3;
        ulong pawnValid = pawn_moves & (ulong.MaxValue - sidePieces);
        return pawnValid;
    }
    public static ulong black_pawn_legal_moves(in ulong pawn_loc, in ulong sidePieces)
    {
        ulong spot_1_clip = clearColumn(0);
        ulong spot_3_clip = clearColumn(7);

        ulong spot_1 = (pawn_loc & spot_1_clip) >> 9;
        ulong spot_2 = pawn_loc >> 8;
        ulong spot_3 = (pawn_loc & spot_3_clip) >> 7;        
        

        ulong pawn_moves = spot_1 | spot_2 | spot_3;
        ulong pawnValid = pawn_moves & (ulong.MaxValue - sidePieces);
        return pawnValid;
    }

    public static ulong bishop_legal_moves(in ulong bishop_loc , in ulong sidePieces, in ulong oppositePieces)
    {
        // Iterate up - right 
        ulong bishop_moves = 0;
        int pos = Bitwise.FirstBitSet(bishop_loc);
        
        // ====================== UPPER RIGHT DIAGONAL =================================
        for (int i = 1 ; i <= 7 ; i++)
        {
            // Check if there is an own piece. If there is break inmediately
            // No own piece , then move on.
            // If capture add this to the list and break
            if ((9*i+pos) % 8 == 0 ) // This indicates we are on the h file. So break
            {
                break;
            }
            
            if ( Bitwise.IsBitSetAtPosition(sidePieces,9*i + pos) ) // Is there a piece of my own?
            {
                break;
            }
            else if ( Bitwise.IsBitSetAtPosition(oppositePieces,9*i+pos))
            {
                bishop_moves |= Bitwise.SetBitAtPosition((ulong)0,9*i+pos);
                break;
            }
            else
            {
                bishop_moves |= Bitwise.SetBitAtPosition((ulong)0,9*i+pos);
            }

            if ((9*i+pos) % 8 == 7 ) // This indicates we are on the h file. So break
            {
                break;
            }
        }
        // ====================== LOWER RIGHT DIAGONAL =================================
        for (int i = 1 ; i <= 7 ; i++)
        {
            int targetPos = (-7*i+pos);
            if (targetPos % 8 == 0 ) // This indicates we are on the h file. So break
            {
                break;
            }
            
            if ( Bitwise.IsBitSetAtPosition(sidePieces,targetPos) ) // Is there a piece of my own?
            {
                break;
            }
            else if ( Bitwise.IsBitSetAtPosition(oppositePieces,targetPos))
            {
                bishop_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos);
                break;
            }
            else
            {
                bishop_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos);
            }

            if (targetPos % 8 == 7 ) // This indicates we are on the h file. So break
            {
                break;
            }
        }
        // UPPER LEFT DIAGONAL
        for (int i = 1 ; i <= 7 ; i++)
        {
            int targetPos = (7*i+pos);
            if (targetPos % 8 == 7 ) // This indicates we are on the h file. So break
            {
                break;
            }
            
            if ( Bitwise.IsBitSetAtPosition(sidePieces,targetPos) ) // Is there a piece of my own?
            {
                break;
            }
            else if ( Bitwise.IsBitSetAtPosition(oppositePieces,targetPos))
            {
                bishop_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos);
                break;
            }
            else
            {
                bishop_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos);
            }

            if (targetPos % 8 == 0 ) // This indicates we are on the h file. So break
            {
                break;
            }
        }
        // LOWER LEFT DIAGONAL
        for (int i = 1 ; i <= 7 ; i++)
        {
            int targetPos = (-9*i+pos);
            if (targetPos % 8 == 7 ) // This indicates we are on the h file. So break
            {
                break;
            }
            
            if ( Bitwise.IsBitSetAtPosition(sidePieces,targetPos) ) // Is there a piece of my own?
            {
                break;
            }
            else if ( Bitwise.IsBitSetAtPosition(oppositePieces,targetPos))
            {
                bishop_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos);
                break;
            }
            else
            {
                bishop_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos);
            }

            if (targetPos % 8 == 0 ) // This indicates we are on the h file. So break
            {
                break;
            }
        }
        // All moves.
        return bishop_moves;
    }

    public static ulong rook_legal_moves(in ulong rook_loc , in ulong ownSide, in ulong oppositePieces)
    {
        // Iterate up - right 
        ulong rook_moves = 0;
        int pos = Bitwise.FirstBitSet(rook_loc);

        // Movement to the right
        for (int i = 1 ; i <= 7 ; i++)
        {
            int targetPos = pos + i;
            // Check for right bounds.
            if (targetPos % 8 == 0) // You are on the h file. You cannot move right anymore.
            {
                break;
            }
            if (Bitwise.IsBitSetAtPosition(ownSide,targetPos))
            {
                break;
            }
            if (Bitwise.IsBitSetAtPosition(oppositePieces,targetPos))
            {
                rook_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos); // Allows to capture. Blocks further movement in that direction.
                break;
            }            
            rook_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos);
        }
        // Movement to the left
        for (int i = 1 ; i <= 7 ; i++)
        {
            int targetPos = pos - i;
            // Check for right bounds.
            if (targetPos % 8 == 7) // You are on the a file. You cannot move left anymore.
            {
                break;
            }
            if (Bitwise.IsBitSetAtPosition(ownSide,targetPos))
            {
                break;
            }
            if (Bitwise.IsBitSetAtPosition(oppositePieces,targetPos))
            {
                rook_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos); // Allows to capture. Blocks further movement in that direction.
                break;
            }            
            rook_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos);
        }
        // Move up
        for (int i = 1 ; i <= 7 ; i++)
        {
            int targetPos = pos + 8*i;
            // Check for right bounds.
            if (targetPos >= 64) // You cant go higher
            {
                break;
            }
            if (Bitwise.IsBitSetAtPosition(ownSide,targetPos))
            {
                break;
            }
            if (Bitwise.IsBitSetAtPosition(oppositePieces,targetPos))
            {
                rook_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos); // Allows to capture. Blocks further movement in that direction.
                break;
            }   
            rook_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos);
        }
        // Move down
        for (int i = 1 ; i <= 7 ; i++)
        {
            int targetPos = pos - 8*i;
            // Check for right bounds.
            if (targetPos <= -1) // You cant go higher
            {
                break;
            }
            if (Bitwise.IsBitSetAtPosition(ownSide,targetPos))
            {
                break;
            }
            if (Bitwise.IsBitSetAtPosition(oppositePieces,targetPos))
            {
                rook_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos); // Allows to capture. Blocks further movement in that direction.
                break;
            }   
            rook_moves |= Bitwise.SetBitAtPosition((ulong)0,targetPos);
        }

        return rook_moves;
    }

    public static ulong queen_legal_moves(in ulong queen_loc, in ulong ownSide, in ulong oppositePieces)
    {
        ulong queen_legal = 0;
        queen_legal |= rook_legal_moves(queen_loc,ownSide,oppositePieces);
        queen_legal |= bishop_legal_moves(queen_loc,ownSide,oppositePieces);
        return queen_legal;
    }

    public static void DebugPosition(ulong integerToConvert)
    {
        byte[] val = BitConverter.GetBytes(integerToConvert);
        string s = "";
        for (int i = 0; i < 8; i++)
        {
            s += new String(Convert.ToString(val[7-i],2).PadLeft(8,'0').Reverse().ToArray()); 
            s += "\n";
        }
        Debug.Log(s);
    }
    private static void NullPieces(int tileToNull)
    {
        Black_Pawns &= ~((ulong)1 << tileToNull);
        Black_Rooks &= ~((ulong)1 << tileToNull);
        Black_Knights &= ~((ulong)1 << tileToNull);
        Black_Bishops &= ~((ulong)1 << tileToNull);
        Black_Kings &= ~((ulong)1 << tileToNull);
        Black_Queens &= ~((ulong)1 << tileToNull);

        White_Pawns &= ~((ulong)1 << tileToNull);
        White_Rooks &= ~((ulong)1 << tileToNull);
        White_Knights &= ~((ulong)1 << tileToNull);
        White_Bishops &= ~((ulong)1 << tileToNull);
        White_Kings &= ~((ulong)1 << tileToNull);
        White_Queens &= ~((ulong)1 << tileToNull);  
    }

    public static void UpdateGameModel(int tileToNull, int tileToSet, string pieceColorToSet)
    {
        switch (pieceColorToSet)
        {
                case "white_pawn":
                    White_Pawns &= ~((ulong)1 << tileToNull); // Sets that bit to zero.
                    White_Pawns |= ((ulong)1 << tileToSet); // Sets that bit to one.
                    /* Pending to check for captures process */ // Nulls pieces in the target square
                    NullPieces(tileToNull); // Handles captures
                    break;
                case "black_pawn": 
                    Black_Pawns &= ~((ulong)1 << tileToNull);
                    Black_Pawns |= ((ulong)1 << tileToSet);
                    NullPieces(tileToNull); // Handles captures
                    break;
                case "white_rook": 
                    White_Rooks &= ~((ulong)1 << tileToNull);
                    White_Rooks |= ((ulong)1 << tileToSet); 
                    NullPieces(tileToNull); // Handles captures                
                    break;
                case "black_rook":
                    Black_Rooks &= ~((ulong)1 << tileToNull);
                    Black_Rooks |= ((ulong)1 << tileToSet); 
                    NullPieces(tileToNull); // Handles captures      
                    break;
                case "white_knight":
                    White_Knights &= ~((ulong)1 << tileToNull);
                    White_Knights |= ((ulong)1 << tileToSet); 
                    NullPieces(tileToNull); // Handles captures                        
                    break;
                case "black_knight":
                    Black_Knights &= ~((ulong)1 << tileToNull);
                    Black_Knights |= ((ulong)1 << tileToSet); 
                    NullPieces(tileToNull); // Handles captures              
                    break;
                case "white_bishop":
                    White_Bishops &= ~((ulong)1 << tileToNull);
                    White_Bishops |= ((ulong)1 << tileToSet); 
                    NullPieces(tileToNull); // Handles captures                
                    break;
                case "black_bishop": 
                    Black_Bishops &= ~((ulong)1 << tileToNull);
                    Black_Bishops |= ((ulong)1 << tileToSet);
                    NullPieces(tileToNull); // Handles captures                
                    break; 
                case "white_queen": 
                    White_Queens &= ~((ulong)1 << tileToNull);
                    White_Queens |= ((ulong)1 << tileToSet);
                    NullPieces(tileToNull); // Handles captures
                    break;
                case "black_queen":
                    Black_Queens &= ~((ulong)1 << tileToNull);
                    Black_Queens |= ((ulong)1 << tileToSet);
                    NullPieces(tileToNull); // Handles captures                 
                    break;
                case "white_king":
                    White_Kings &= ~((ulong)1 << tileToNull);
                    White_Kings |= ((ulong)1 << tileToSet);
                    NullPieces(tileToNull); // Handles captures
                    break;
                case "black_king":
                    Black_Kings &= ~((ulong)1 << tileToNull);
                    Black_Kings |= ((ulong)1 << tileToSet);
                    NullPieces(tileToNull); // Handles captures 
                    break;
        }
        // Recalculate position
        White_Pieces = White_Knights | White_Kings | White_Pawns | White_Rooks | White_Bishops |  White_Queens;
        Black_Pieces = Black_Rooks | Black_Bishops | Black_Knights | Black_Kings | Black_Queens | Black_Pawns;
        Full_Board = White_Pieces | Black_Pieces;
        
        DebugPosition(Black_Pawns);
    }
}
