using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace ZPD_Lab_1_4
{

    public class BitArrayBlock
    {
        const int size = 64;

        private BitArray _block;
        public BitArrayBlock(BitArray block )
        {

            _block = block;
        }


        public BitArrayBlock GetLeftHalf()
        {
            bool[] leftHalf = new bool[size / 2];

            for (int i = 0; i < size/2; i++)
            {
                leftHalf[i] = _block[i + size / 2];
            }
            BitArray bitBlock = new BitArray(leftHalf);

            return new BitArrayBlock(bitBlock);
        }

        public BitArrayBlock GetRightHalf()
        {
            bool[] rightHalf = new bool[size / 2];

            for (int i = 0; i < size / 2; i++)
            {
                rightHalf[i] = _block[i];
            }
            BitArray bitBlock = new BitArray(rightHalf);

            return new BitArrayBlock(bitBlock);
        }

        public BitArrayBlock Xor(BitArrayBlock block2)
        {
            return new BitArrayBlock(
                this._block.Xor(block2._block));
        }

        public void CircularLeftShift(int positions)
        {
            for (int i = 0; i < positions; i++)
            {
                bool mostSignificantBit = _block.Get(size / 2 - 1);
                _block.LeftShift(1);
                _block.Set(0, mostSignificantBit);
            }
        }

        public void ExchangeBits(BitArrayBlock block)
        {
            BitArray blockBits = block._block;

            block._block = this._block;
            this._block = blockBits;
        }

        public BitArray GetBits()
        {
            return _block;
        }

        public BitArrayBlock Clone()
        {
            return new BitArrayBlock(
                (BitArray)_block.Clone()
            );
        }

        public BitArray CombineIntoBitArray(BitArrayBlock rightHalf)
        {
            bool[] array = new bool[_block.Length + rightHalf._block.Length];

            rightHalf._block.CopyTo(array, 0);
            this._block.CopyTo(array, rightHalf._block.Length);

            return new BitArray(array);
        }
    }
}
