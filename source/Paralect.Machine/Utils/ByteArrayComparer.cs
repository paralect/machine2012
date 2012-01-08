using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paralect.Machine.Utils
{
    /// <summary>
    /// Discussion about this here:
    /// http://stackoverflow.com/a/1440395/407599
    /// 
    /// Usage:
    /// var dict = new Dictionary<Byte[], String>(new ByteArrayComparer());
    /// </summary>
    public class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] left, byte[] right) 
        {
            if (left == right)
                return true;

            if (left == null || right == null)
                return left == right;

            if (left.Length != right.Length)
                return false;

            for (int i= 0; i < left.Length; i++) 
            {
                if ( left[i] != right[i] ) 
                    return false;
            }

            return true;
        }

        /// <summary>
        /// From StackOverflow comments:
        /// -- "Summing the results may not be the best hash code. Perhaps: sum = 33 * sum + cur"
        /// -- "Hmm, yeah this is that "error prone" part I was talking about... :) Getting a truly unique hash code will be tricky. 
        ///     sixlettervariables idea isn't terrible but how can I verify that this is mathematically valid?"
        /// -- "You rarely need a perfect hash code, you need relatively unique hash code. The "Times 33" is a popular, simple hash code, 
        ///     I think used by Perl at one point (like in the 5.0 and prior days). It uses prime or relatively prime numbers 
        ///     to get decent distributions. The wiki page for Hashtables has a good explanation: en.wikipedia.org/wiki/Hash_table"
        /// TODO: Verify this.
        /// </summary>
        public int GetHashCode(byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            int sum = 0;

            foreach (byte cur in key)
                //sum += cur;
                sum = 33 * sum + cur; // This line gives huge performance rise 

            return sum;
        }
    }
}
