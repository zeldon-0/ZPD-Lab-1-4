using System;
using System.Collections.Generic;
using System.Text;

namespace ZPD_Lab_1_4
{
    public class FunctionF
    {
        private List<int> _s1 = new List<int> { 4, 10, 9, 2, 13, 8, 0, 14, 6, 11, 1, 12, 7, 15, 5, 3 };
        private List<int> _s2 = new List<int> { 14, 11, 4, 12, 6, 13, 15, 10, 2, 3, 8, 1, 0, 7, 5, 9 };
        private List<int> _s3 = new List<int> { 5, 8, 1, 13, 10, 3, 4, 2, 14, 15, 12, 7, 6, 0, 9, 11 };
        private List<int> _s4 = new List<int> { 7, 13, 10, 1, 0, 8, 9, 15, 14, 4, 6, 12, 11, 2, 5, 3 };
        private List<int> _s5 = new List<int> { 6, 12, 7, 1, 5, 15, 13, 8, 4, 10, 9, 14, 0, 3, 11, 2 };
        private List<int> _s6 = new List<int> { 4, 11, 10, 0, 7, 2, 1, 13, 3, 6, 8, 5, 9, 12, 15, 14 };
        private List<int> _s7 = new List<int> { 13, 11, 4, 1, 3, 15, 5, 9, 0, 10, 14, 7, 6, 8, 2, 12 };
        private List<int> _s8 = new List<int> { 1, 15, 13, 0, 5, 7, 10, 4, 9, 2, 3, 14, 6, 11, 8, 12 };

        private List<List<int>> _sBlocks = new List<List<int>>();
        
        public FunctionF()
        {
            _sBlocks.Add(_s1);
            _sBlocks.Add(_s2);
            _sBlocks.Add(_s3);
            _sBlocks.Add(_s4);
            _sBlocks.Add(_s5);
            _sBlocks.Add(_s6);
            _sBlocks.Add(_s7);
            _sBlocks.Add(_s8);
        }

        public void ApplySBlocks(int[] numbersToReplace)
        {
            for (int i = 0; i < numbersToReplace.Length; i++)
            {
                numbersToReplace[i] = _sBlocks[i] [numbersToReplace[i]];
            }
        }
    }
}