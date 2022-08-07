using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;

namespace ColorUtilitiesTestApp
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true, maxDepth: 3)]
    public class VectorSum
    {
        private readonly Vector256<int> _vector = Vector256.Create(69);

        private readonly Vector<int> _varSized = new(69);

        [Benchmark] public int SimdHelper() => _vector.HorizontalAddInt();
        [Benchmark] public int BclSum() => Vector256.Sum(_vector);
        //[Benchmark] public int VarSizedSum() => Vector.Sum(_varSized);
    }

    public static class SIMDHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int HorizontalAddInt(this Vector256<int> vec)
        {
            var Upper = vec.GetUpper();

            var Lower = vec.GetLower();

            var Sum = Avx2.Add(Upper, Lower);

            return HorizontalAddInt(Sum);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int HorizontalAddInt(this Vector128<int> vec)
        {
            const int ShuffleMask = (3 << 2) + 2; //00_00_11_10 [0, 0, 3, 2]
            const int ShuffleMask2 = 1;  //00000001 [0, 0, 0, 1]

            vec = Avx2.Add(vec, Avx2.Shuffle(vec, ShuffleMask));
            return Avx2.Add(vec, Avx2.Shuffle(vec, ShuffleMask2)).GetElement(0);
        }
    }
}