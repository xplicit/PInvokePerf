using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace PInvokePerf
{
	class MainClass
	{
		delegate void HideFromJit();

		const int nTests=100000;
		const int maxElems=100;
		const int maxChars=100;
		static string[] perfArray = new string[maxElems];
		static byte[] testBytes = new byte[16384];

		public static void Main (string[] args)
		{
			InitArray ();
			PerformanceTest.InitInternals ();
//			GC ();
			Console.WriteLine ("Performance measuring starting");

			//You can call it directly in AOT mode
//			for (int i = 0; i < 5; i++) {
//				Performance ();
//			}

			//This is workaround for mono bug/feature
			//When mono started it tries to compile 'Main' and recursively 
			//all methods, which are called from 'Main' method
			//And in our case it compiles managed_to_native wrapper for our Internal Call methods 
			//which are not registred yet by 'InitInternal'. (We are expecting to register icalls
			//first and only after compile them). In this case JIT cannot find icall entry points,
			//generates 'MissingMethodException' body for managed_to_native wrapper and cashes it. 
			//When we register new icall entry later we can't regenerate wrapper or remove it from 
			//the wrapper's cache. 
			//To workaround this behaviour we hide our functions from JIT by calling them through
			//delegate 
			//By the way in AOT mode you don't need to use such workaround all works out of the box.
			HideFromJit d=Performance;
			for (int i = 0; i < 5; i++) {
				d ();
			}
		}

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

		public static void GC()
		{
			int sum = 0;
			for (int n = 0; n < nTests; n++) {
				for (int i = 0; i < perfArray.Length; i++) {
					perfArray [i] = "abcdefg"+i.ToString()+n.ToString();
					int s1 = PerformanceTest.InternalCount (perfArray, i);
					sum += s1;
				}
			}
		}

		public static void Performance()
		{
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
