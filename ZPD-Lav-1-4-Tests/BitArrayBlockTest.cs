using System;
using System.Collections;
using System.Linq;
using Xunit;
using ZPD_Lab_1_4;

namespace ZPD_Lav_1_4_Tests
{
    public class BitArrayBlockTest
    {
        [Fact]
        public void GetLeftHalf_FourZeroesAndFourOnes_ReturnsBitArrayConstistingOf32Zeros()
        {
            int[] values = new int[] { 0, 0, 0, 0, 1, 1, 1, 1 };
            BitArray bits = new BitArray(values.Select(a => (byte)a).Reverse().ToArray());
            BitArrayBlock block = new BitArrayBlock(bits);
            bool[] arrayToCheck = new bool[32];
            bool[] expectedArray = new bool[32];

            block = block.GetLeftHalf();
            bits = block.GetBits();
            bits.CopyTo(arrayToCheck, 0);

            Assert.Equal(expectedArray, arrayToCheck);

        }


        [Fact]
        public void GetRightHalf_FourZeroesAndFourOnes_ReturnsBitArrayConstistingOf4OneOctets()
        {
            int[] values = new int[] { 0, 0, 0, 0, 1, 1, 1, 1 };
            BitArray bits = new BitArray(values.Select(a => (byte)a).Reverse().ToArray());
            BitArrayBlock block = new BitArrayBlock(bits);
            bool[] arrayToCheck = new bool[32];
            bool[] expectedArray = new bool[32];

            block = block.GetRightHalf();
            bits = block.GetBits();
            bits.CopyTo(arrayToCheck, 0);
            bits = new BitArray(new int[] { 1, 1, 1, 1 }.Select(a => (byte)a).ToArray());
            bits.CopyTo(expectedArray, 0);

            Assert.Equal(expectedArray, arrayToCheck);

        }
        [Fact]
        public void Exchange_TwoBitArrays_BitArrayBlocksShouldHaveTheirVitArraysSwitched()
        {
            int[] firstArray = { 0, 0, 0, 0 };
            int[] secondArray = { 1, 1, 1, 1 };

            BitArray firstBitArray = new BitArray(firstArray.Select(a => (byte)a).Reverse().ToArray());
            BitArray secondBitArray = new BitArray(secondArray.Select(a => (byte)a).Reverse().ToArray());

            BitArrayBlock firstBlock = new BitArrayBlock(firstBitArray);
            BitArrayBlock secondBlock = new BitArrayBlock(secondBitArray);

            firstBlock.ExchangeBits(secondBlock);

            Assert.Equal(secondBitArray, firstBlock.GetBits());
            Assert.Equal(firstBitArray, secondBlock.GetBits());
        }
    }
}
