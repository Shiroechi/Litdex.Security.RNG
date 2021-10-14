namespace Litdex.Utilities
{
	/// <summary>
	///		Provide 128 it math operations
	/// </summary>
	public static class Math128
	{
		/// <summary>
		///		Do multiplication of 2 <see cref="ulong"/>.
		/// </summary>
		/// <param name="x">
		///		Number to multiply.
		/// </param>
		/// <param name="y">
		///		Number to multiply.
		/// </param>
		/// <returns>
		///		128-bit number, split into 2 <see cref="ulong"/>.
		///		The first one is the high bit, the second one is low bit. 
		/// </returns>
		public static (ulong, ulong) Multiply(ulong x, ulong y)
		{
			ulong hi, lo;

			lo = x * y;

			ulong x0 = (uint)x;
			var x1 = x >> 32;

			ulong y0 = (uint)y;
			var y1 = y >> 32;

			var p11 = x1 * y1;
			var p10 = x1 * y0;
			var p01 = x0 * y1;
			var p00 = x0 * y0;

			// 64-bit product + two 32-bit values
			var middle = p10 + (p00 >> 32) + (uint)p01;

			// 64-bit product + two 32-bit values
			hi = p11 + (middle >> 32) + (p01 >> 32);

			return (hi, lo);
		}
	}
}