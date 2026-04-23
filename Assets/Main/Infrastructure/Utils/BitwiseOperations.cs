namespace Main.Infrastructure.Utils
{
    public static class BitwiseOperations
    {
        public static bool IsBitSet(this long number, int position)
        {
            return (number & (1L << position)) != 0;
        }
        
        public static long SetBit(this long number, int position, bool set)
        {
            if (set)
            {
                return number | (1L << position);
            }
            else
            {
                return number & ~(1L << position);
            }
        }

        public static long ExtractLong(this long fromNumber, int fromPosition, int toPosition) 
        { 
            return ((1L << (toPosition - fromPosition + 1)) - 1) & (fromNumber >> fromPosition); 
        }

        public static int ExtractInt(this long fromNumber, int fromPosition, int toPosition) 
        { 
            return (int)ExtractLong(fromNumber, fromPosition, toPosition); 
        }
        
        public static byte ExtractByte(this long fromNumber, int fromPosition, int toPosition) 
        { 
            return (byte)ExtractLong(fromNumber, fromPosition, toPosition); 
        }

        public static int UpdateInt(this int toNumber, int number, int fromPosition, int toPosition) 
        {
            const int allOnes = ~0;
            
            var left = allOnes << System.Math.Min(31, toPosition + 1); 
            var right = ((1 << fromPosition) - 1);
            
            var mask = left | right; 
        
            var clearedToNumber = toNumber & mask;
            var shiftedNumber = number << fromPosition;   
            return (clearedToNumber | shiftedNumber);  
        }
        
        public static long UpdateLong(this long toNumber, int number, int fromPosition, int toPosition) 
        {
            const long allOnes = ~0;
            
            var left = allOnes << System.Math.Min(63, toPosition + 1); 
            var right = ((1L << fromPosition) - 1);
            
            var mask = left | right; 
        
            var clearedToNumber = toNumber & mask;
            var shiftedNumber = (long)number << fromPosition;   
            return (clearedToNumber | shiftedNumber);  
        }
        
        public static long UpdateLong(this long toNumber, long number, int fromPosition, int toPosition) 
        {
            const long allOnes = ~0;
            
            var left = allOnes << System.Math.Min(63, toPosition + 1); 
            var right = ((1L << fromPosition) - 1);
            
            var mask = left | right;
            
            var clearedToNumber = toNumber & mask;
            var shiftedNumber = number << fromPosition;   
            return (clearedToNumber | shiftedNumber);  
        }
    }
}
