using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;
using ZPD_Lab_1_4;

namespace ZPD_Lav_1_4_Tests
{
    public class GostCipherTest
    {
        [Theory]
        [InlineData("asadfasfsafsdfsdfdfdgtyk")]
        [InlineData("Rojou StunGun no Dengeki ga Utsu Gunshuu no Kage Yai Yai To Hito wa Yuki Himitsuri ni Koto wa Naru Kikeyo Monokage de Yoki koto no tame to Sasayaku     ")]
        [InlineData("Tobe Kagaku no ada Yuurei Hikouki Yuke jigoku no sata shouki wo taite Dare mo shiranu ma ni tobitatsu Ari mo senu sora wo KIMI e Gokuhi Shinri no bagu Yuurei Hikouki Hentai yozora ni saku dai karin Dare mo koe kakete chi ni ochi Ari mo senu machi no KIMI n")]
        [InlineData("Meta no kumo tooku yuk(u) choo p(u)ranetarium(u) Hiratai kurono no yume Harashoo to miageta sora kaguwashik(u) Hoshi wo shiryo de umi Saa, iki wo haki Saa, jeneshisu wo Saa, me wo samashi Saa, jeneshisu wo   ")] 
        public void EncodeDecode_MessageString_DecodedCipherMAthcesWithMessage(string message)
        {
            GOSTCipher cipher = new GOSTCipher();

            char[] encodedMessage = cipher.Encode(message.ToCharArray());
            char[] decodedMessage = cipher.Decode(encodedMessage);

            Assert.Equal(message, new string(decodedMessage));
        }

        [Fact]
        public void _fillCharArrayToSize_UndersizedArray_ReturnsArrayWithSpaces()
        {
            char[] message = "asadfasfsafsdfsdfdf".ToCharArray();
            GOSTCipher cipher = new GOSTCipher();

            char[] filled = cipher._fillCharArrayToSize(message);

            Assert.Equal("asadfasfsafsdfsdfdf     ", new string(filled));
        }

        [Fact]
        public void ConvertToBitArrayTest()
        {
            char[] message = { 'a', 'b', 'c' };
            GOSTCipher cipher = new GOSTCipher();

            BitArray bitArray = cipher._convertToBitArray(message);

            bool[] bits = new bool[24];
            bitArray.CopyTo(bits, 0);

            bool[] values = new bool[24] { true, true, false, false, false, true, true,
            false, false, true, false, false, false, true, true, false, true, false,
            false, false, false, true, true, false};

            Assert.Equal(values, bits);
        }

        [Fact]
        public void GetCharArraySliceTest()
        {
            char[] message = { 'a', 'b', 'c' };
            GOSTCipher cipher = new GOSTCipher();

            char[] slice = cipher._getCharArraySlice(message, 1, 2);

            Assert.Equal(new char[] { 'b', 'c' }, slice);

        }

        [Fact]
        public void ConvertToCharArrayTest()
        {
            bool[] values = new bool[24] { true, true, false, false, false, true, true,
            false, false, true, false, false, false, true, true, false, true, false,
            false, false, false, true, true, false};
            GOSTCipher cipher = new GOSTCipher();

            BitArrayBlock block = new BitArrayBlock(new BitArray(values));

            char[] chars = cipher._convertToCharArray(block);

            Assert.Equal(new char[] { 'a', 'b', 'c' }, chars);
        }
    }
}
