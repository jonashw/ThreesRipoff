using System;
using NUnit.Framework;

namespace ThreesRipoff.Model
{
    public static class MathUtil
    {
		public static int IntPow(int x, int pow)
		{
			var ret = 1;
			while ( pow != 0 )
			{
				if ( (pow & 1) == 1 )
					ret *= x;
				x *= x;
				pow >>= 1;
			}
			return ret;
		}

        [TestFixture]
        public class TestIntPow
        {
			[Test]
            public void Test()
            {
				test(2,0,1);
				test(2,1,2);
				test(2,2,4);
				test(2,3,8);
				test(2,4,16);
				test(2,5,32);
            }

            private static void test(int input, int power, int expectedOutput)
            {
                Assert.AreEqual(
                    expectedOutput,
                    MathUtil.IntPow(input, power),
                    String.Format("{0}^{1}=={2}",input,power,expectedOutput));
            }
        }
    }
}
