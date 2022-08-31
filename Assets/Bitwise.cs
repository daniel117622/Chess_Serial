using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bitwise
{
    // Class to set bits, to check etc

    public static bool IsBitSetAtPosition(in ulong bitboard, in int position)
    {
        ulong compare = 0;
        compare = bitboard & ((ulong)1 << position);

        return compare == 0 ? false : true;
    }

    public static ulong SetBitAtPosition(ulong bitboard, in int position)
    {
        if (position >= 64 || position <= -1) // Dont do anything to prevent weird behavior
        {
            return bitboard;
        }
        bitboard |= ((ulong)1 << position);
        return bitboard;
    }

    public static int FirstBitSet(ulong bitboard) // Returns the position of the first bit.
    {
        int i = 0;
        while (!IsBitSetAtPosition(bitboard,i))
        {
            i++;
            if (i >= 64)
            {
                return -1;
            }
        }
        return i;
    }
}
