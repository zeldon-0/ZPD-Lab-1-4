using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ZPD_Lab_1_4
{
    public class GOSTCipher
    {
        private BitArray _key;
        private List<int> _subKeyOrder = new List<int>() 
        {
            1, 2, 3, 4, 5, 6, 7, 8,
            1, 2, 3, 4, 5, 6, 7, 8,
            1, 2, 3, 4, 5, 6, 7, 8,
            8, 7, 6, 5, 4, 3, 2, 1
        };

        public GOSTCipher(BitArray key)
        {
            _key = key;
        }

        public BitArray Encode(BitArray message)
        {
            throw new NotImplementedException();
        }
        public BitArray Decode(BitArray encodedMessage)
        {
            throw new NotImplementedException();
        }

        private BitArray _encodeBlock(BitArray block)
        {
            BitArrayBlock wholeBlock = new BitArrayBlock(block);

            BitArrayBlock leftBlock = wholeBlock.GetLeftHalf();
            BitArrayBlock rightBlock = wholeBlock.GetRightHalf();

            for (int i = 0; i < 32; i++)
            {
                BitArrayBlock rightBlockCopy = rightBlock.Clone();

                BitArrayBlock subkey = _getSubKey(i);
                rightBlock = rightBlock.Xor(subkey);

                // TO DO: S-BLOCK

                rightBlock.CircularLeftShift(11);
                rightBlock = rightBlock.Xor(leftBlock);

                leftBlock = rightBlockCopy;
            }

            rightBlock.ExchangeBits(leftBlock);

            return leftBlock.CombineIntoBitArray(rightBlock);
        }

        private BitArrayBlock _getSubKey(int round)
        {
            int subKeyIndex = _subKeyOrder[round];

            bool[] bits = new bool[32];
            for (int i = 0; i < 32; i++)
            {
                bits[i] = _key.Get(32 * subKeyIndex + i);
            }

            BitArray bitArray = new BitArray(bits);
            return new BitArrayBlock(bitArray);
        }

    }
}
