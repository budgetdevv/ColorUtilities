using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Color.Encoding
{
    public static class SIMDHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int HorizontalAddInt(this Vector256<int> Vec)
        {
            var Upper = Vec.GetUpper();

            var Lower = Vec.GetLower();

            var Sum = Avx2.Add(Upper, Lower);

            return HorizontalAddInt(Sum);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int HorizontalAddInt(this Vector128<int> Vec)
        {
            const int ShuffleMask = (3 << 2) + 2; //00_00_11_10 [0, 0, 3, 2]

            Vec = Avx2.Add(Vec, Avx2.Shuffle(Vec, ShuffleMask));

            const int ShuffleMask2 = 1;  //00000001 [0, 0, 0, 1]
            
            return Avx2.Add(Vec, Avx2.Shuffle(Vec, ShuffleMask2)).GetElement(0);
        }
    }
}