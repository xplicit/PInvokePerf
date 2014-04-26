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

			Console.WriteLine ("Managed={0} ms", sw.ElapsedMilliseconds);

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

			Console.WriteLine ("Internal={0} ms", sw.ElapsedMilliseconds);

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
			Console.WriteLine ("PInvoke={0} ms", sw.ElapsedMilliseconds);

		}

	}
}
