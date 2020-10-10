using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ZPD_Lab_1_4
{
    public class KeyGenerator
    {
        private int _size;
        public KeyGenerator(int size)
        {
            _size = size;
        }

        public KeyGenerator() : this(256) { }

        public BitArray GenerateKey()
        {
            bool[] bits = new bool[_size];

            Random random = new Random();
            for(int i = 0; i < _size; i++)
            {
                if(random.Next(2) == 1)
                {
                    bits[i] = true;
                }
            }

            BitArray bitArray = new BitArray(bits);

            return bitArray;
        }
    }
}
