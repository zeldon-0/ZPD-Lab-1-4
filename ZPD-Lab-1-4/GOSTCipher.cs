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

        public GOSTCipher()
        {
            _key = new KeyGenerator().GenerateKey();
            _functionF = new FunctionF();
        }

        public char[] Encode(char[] message)
        {
            BitArrayBlock encodedMessage = new BitArrayBlock(
                new BitArray(new bool[0])
                );

            message = _fillCharArrayToSize(message);

            for (int i = 0; i < message.Length; i += 8)
            {
                char[] messageSlice = _getCharArraySlice(message, i, 8); 
                BitArray messageBlock = _convertToBitArray(messageSlice);

                BitArrayBlock encodedBlock = new BitArrayBlock(
                    _encodeBlock(messageBlock)
                );

                encodedMessage = new BitArrayBlock( 
                    encodedMessage.CombineIntoBitArray(encodedBlock)
                    );
 
            }
            bool[] bits = new bool[message.Length * 8];
            encodedMessage.GetBits().CopyTo(bits, 0);


            return _convertToCharArray(encodedMessage);
        }

        public char[] Decode(char[] message)
        {
            BitArrayBlock encodedMessage = new BitArrayBlock(
                new BitArray(new bool[0])
                );

            message = _fillCharArrayToSize(message);

            for (int i = 0; i < message.Length; i += 8)
            {
                char[] messageSlice = _getCharArraySlice(message, i, 8);
                BitArray messageBlock = _convertToBitArray(messageSlice);

                BitArrayBlock encodedBlock = new BitArrayBlock(
                    _decodeBlock(messageBlock)
                );

                encodedMessage = new BitArrayBlock(
                    encodedMessage.CombineIntoBitArray(encodedBlock)
                    );

            }
            bool[] bits = new bool[message.Length * 8];
            encodedMessage.GetBits().CopyTo(bits, 0);


            return _convertToCharArray(encodedMessage);
        }


        public BitArray _convertToBitArray(char[] message)
        {


            byte[] bytes = message.Reverse().Select(c => (byte)c).ToArray();

            BitArray bitArray = new BitArray(bytes);
            bool[] bits = new bool[bytes.Length * 8];
            bitArray.CopyTo(bits, 0);

            bitArray = new BitArray(bits);
            return bitArray;

        }

        public char[] _fillCharArrayToSize(char[] array)
        {
            char[] newArray = new char[array.Length + 8 - array.Length % 8];
            if (array.Length % 8 != 0)
            {
                for(int i = 0; i < newArray.Length; i++)
                {
                    if (i < array.Length)
                    {
                        newArray[i] = array[i];
                    }
                    else 
                    {
                        newArray[i] = ' ';
                    }
                }
                return newArray;
            }
            return array;
        }
        public char[] _convertToCharArray(BitArrayBlock block)
        {
            BitArray bitArray = block.GetBits();

            char[] chars = new char[bitArray.Length / 8];
            for (int i = bitArray.Length - 1; i > 0; i -= 8)
            {
                int numericValue = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (bitArray[i - j])
                    {
                        numericValue += (int)Math.Pow(2, 7 - j);
                    }
                }
                chars[(bitArray.Length - i) / 8] = (char)numericValue;
            }

            return chars;

        }
        public char[] _getCharArraySlice(char[] charArray, int startIndex, int size)
        {
            char[] sliceChars = new char[size];

            for(int i = startIndex; i < startIndex + size;  i++)
            {
                sliceChars[i - startIndex] = charArray[i];
            }

            return sliceChars;
            
        }

        public BitArray _encodeBlock(BitArray block)
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

        public BitArray _decodeBlock(BitArray block)
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

        public BitArrayBlock _getSubKey(int round)
        {
            int subKeyIndex = _subKeyOrder[round] - 1;

            bool[] bits = new bool[32];
            for (int i = 0; i < 32; i++)
            {
                bits[i] = _key.Get(32 * subKeyIndex + i);
            }

            BitArray bitArray = new BitArray(bits);
            return new BitArrayBlock(bitArray);
        }

        public int[] _convertTo4BitInts(BitArray bitArray)
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

        public BitArrayBlock _convertToBits(int[] numbers)
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
