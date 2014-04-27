using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace PInvokePerf
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			InitArray ();
			PerformanceTest.InitInternals ();
			Console.WriteLine ("Performance measuring starting");
			Performance ();
		}

		const int maxElems=100;
		const int maxChars=100;
		static string[] perfArray = new string[maxElems];
		static byte[] testBytes = new byte[16384];
		const int nTests=100000;

		public static void InitArray()
		{
			for (int i = 0; i < maxElems; i++) {
				char[] str = new char[maxChars];

				for (int j = 0; j < maxChars; j++) {
					str [j] = (char)((int)'A'+(j + i) % 26);
				}
				perfArray [i] = new string (str);
				Console.WriteLine ("str[{0,2}]={1}", i,perfArray[i]);
			}

			for (int i = 0; i < testBytes.Length; i++)
				testBytes [i] = (byte)(i & 0xff);
		}

		public static void Performance()
		{
			PerformanceTest test = new PerformanceTest ();

			int s = PerformanceTest.ManagedCount (perfArray,0);
			int sum = 0;

			Stopwatch sw = new Stopwatch ();
			sw.Start ();
			for (int n = 0; n < nTests; n++) {
				for (int i = 0; i < perfArray.Length; i++) {
					int s1 = PerformanceTest.ManagedCount (perfArray, i);
					sum += s1;
				}
			}
			sw.Stop ();

			Console.WriteLine ("Managed char count={0} ms", sw.ElapsedMilliseconds);

			s = PerformanceTest.InternalCount (perfArray,0);
			if (!PerformanceTest.Validate (perfArray, PerformanceTest.InternalCount))
			{
				throw new ArgumentException ("Wrong algorithm implementation"); 
			}

			sum = 0;
			sw.Restart ();
			for (int n = 0; n < nTests; n++) {
				for (int i = 0; i < perfArray.Length; i++) {
					int s1 = PerformanceTest.InternalCount (perfArray, i);
					sum += s1;
				}
			}
			sw.Stop ();

			Console.WriteLine ("Internal char count={0} ms", sw.ElapsedMilliseconds);

			s = PerformanceTest.UnmanagedCount (perfArray, 0);
			if (!PerformanceTest.Validate (perfArray, PerformanceTest.UnmanagedCount))
			{
				throw new ArgumentException ("Wrong algorithm implementation"); 
			}

			sum = 0;
			sw.Restart ();
			for (int n = 0; n < nTests; n++) {
				for (int i = 0; i < perfArray.Length; i++) {
					int s1 = PerformanceTest.UnmanagedCount (perfArray, i);
					sum += s1;
				}
			}
			sw.Stop ();
			Console.WriteLine ("PInvoke char count={0} ms", sw.ElapsedMilliseconds);

			//byte arrays
			PerformanceTest.ManagedXor (testBytes, 0x55);

			sw.Restart ();
			for (int n = 0; n < nTests; n++) {
				PerformanceTest.ManagedXor (testBytes, 0x55);
			}
			sw.Stop ();
			Console.WriteLine ("Managed Xor={0} ms", sw.ElapsedMilliseconds);

			//internal xor
			PerformanceTest.InternalXor (testBytes, 0x55);
			if (!PerformanceTest.Validate (testBytes, 0x55, PerformanceTest.InternalXor))
			{
				throw new ArgumentException ("InternalXor: wrong algorithm implementation"); 
			}

			sw.Restart ();
			for (int n = 0; n < nTests; n++) {
				PerformanceTest.InternalXor (testBytes, 0x55);
			}
			sw.Stop ();
			Console.WriteLine ("Internal Xor={0} ms", sw.ElapsedMilliseconds);

			//unmanaged xor
			PerformanceTest.UnmanagedXor (testBytes, 0x55);
			if (!PerformanceTest.Validate (testBytes, 0x55, PerformanceTest.UnmanagedXor))
			{
				throw new ArgumentException ("UnmanagedXor: wrong algorithm implementation"); 
			}

			sw.Restart ();
			for (int n = 0; n < nTests; n++) {
				PerformanceTest.UnmanagedXor (testBytes, 0x55);
			}
			sw.Stop ();
			Console.WriteLine ("PInvoke Xor={0} ms", sw.ElapsedMilliseconds);
		}

	}
}
