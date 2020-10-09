using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZPD_Lab_1_4
{
    public class GOSTCipher
    {
        private BitArray _key;
        private FunctionF _functionF;
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
            _functionF = new FunctionF();
        }

        public string Encode(string message)
        {
            BitArrayBlock encodedMessage = new BitArrayBlock(
                new BitArray(new bool[0])
                );

            for (int i = 0; i < message.Length; i += 8)
            {
                string messageSlice = message.Substring(i, 8);
                BitArray messageBlock = _convertToBitArray(messageSlice);

                BitArrayBlock encodedBlock = new BitArrayBlock(
                    _encodeBlock(messageBlock)
                );

                encodedMessage.CombineIntoBitArray(encodedBlock);
            }

            return _convertToString(encodedMessage);
        }

        public string Decode(string message)
        {
            BitArrayBlock decodedMessage = new BitArrayBlock(
                 new BitArray(new bool[0])
            );

            for (int i = 0; i < message.Length; i += 8)
            {
                string messageSlice = message.Substring(i, 8);
                BitArray messageBlock = _convertToBitArray(messageSlice);

                BitArrayBlock decodedBlock = new BitArrayBlock(
                    _encodeBlock(messageBlock)
                );

                decodedMessage.CombineIntoBitArray(decodedBlock);
            }

            return _convertToString(decodedMessage);
        }


        private BitArray _convertToBitArray(string message)
        {
            if (message.Length % 8 != 0)
            {
                for (int i = message.Length % 8; i < 8; i++)
                {
                    message+=" ";
                }
            }

            char[] chars = message.ToCharArray();
            byte[] bytes = chars.Select(c => (byte)c).ToArray();

            BitArray bitArray = new BitArray(bytes);
            bool[] bits = new bool[bytes.Length * 8];
            bitArray.CopyTo(bits, 0);
            bits = bits.Reverse().ToArray();

            bitArray = new BitArray(bits);
            return bitArray;

        }

        private string _convertToString(BitArrayBlock block)
        {
            BitArray bitArray = block.GetBits();

            char[] chars = new char[bitArray.Length / 8];
            for (int i = bitArray.Length - 1; i >= 0; i -= 8)
            {
                int numericValue = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (bitArray[i - j])
                    {
                        numericValue += (int)Math.Pow(2, 7 - j);
                    }
                }
                chars[7 - i / 8] = (char)numericValue;
            }

            return new string(chars);

        }
        private BitArray _getBitArraySlice(BitArray bitArray, int startIndex, int endIndex)
        {
            bool[] sliceBits = new bool[endIndex - startIndex + 1];

            for(int i = startIndex; i < endIndex;  i++)
            {
                sliceBits[i - startIndex] = bitArray.Get(i);
            }

            return new BitArray(sliceBits);
            
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


                int[] numbers = _convertTo4BitInts(rightBlock.GetBits());
                _functionF.ApplySBlocks(numbers);

                rightBlock = _convertToBits(numbers);


                rightBlock.CircularLeftShift(11);
                rightBlock = rightBlock.Xor(leftBlock);

                leftBlock = rightBlockCopy;
            }

            rightBlock.ExchangeBits(leftBlock);

            return leftBlock.CombineIntoBitArray(rightBlock);
        }

        private BitArray _decodeBlock(BitArray block)
        {
            BitArrayBlock wholeBlock = new BitArrayBlock(block);

            BitArrayBlock leftBlock = wholeBlock.GetLeftHalf();
            BitArrayBlock rightBlock = wholeBlock.GetRightHalf();

            for (int i = 0; i < 32; i++)
            {
                BitArrayBlock rightBlockCopy = rightBlock.Clone();

                BitArrayBlock subkey = _getSubKey(31 - i);
                rightBlock = rightBlock.Xor(subkey);


                int[] numbers = _convertTo4BitInts(rightBlock.GetBits());
                _functionF.ApplySBlocks(numbers);

                rightBlock = _convertToBits(numbers);


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

        private int[] _convertTo4BitInts(BitArray bitArray)
        {
            bool[] bits = new bool[bitArray.Length];
            bitArray.CopyTo(bits, 0);

            int[] numbers = new int[bits.Length / 4];

            for(int i = 0; i < numbers.Length; i++)
            {
                if(bits[i])
                {
                    numbers[i / 4] += (int) Math.Pow(2, i % 4);
                }
            }

            return numbers;
        }

        private BitArrayBlock _convertToBits(int[] numbers)
        {
            bool[] bits = new bool[numbers.Length * 4];
            for(int i = 0; i < numbers.Length; i++)
            {
                for (int j = 3; j >= 0; j--)
                {
                    if (numbers[i] - Math.Pow(2, j) >= 0)
                    {
                        bits[i * 4 + j] = true;
                        numbers[i] -= (int) Math.Pow(2, j);
                    }
                }
            }
            BitArray bitArray = new BitArray(bits);

            return new BitArrayBlock(bitArray);
        }

    }
}
