using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace PInvokePerf
{
	public class PerformanceTest
	{
		public delegate int CountDelegate(string[] array,int index);
		public delegate void XorDelegate(byte[] array,byte xored);

		public PerformanceTest ()
		{
		}

		public static int ManagedCount(string[] arr,int i)
		{
			int sum = 0;
			for (int j = 0; j < arr [i].Length; j++) {
				sum+=(int)arr[i][j];
			}
	
			return sum;
		}

		public static bool Validate(string[] arr,CountDelegate d)
		{
			int sum = 0;

			for (int i = 0; i < arr.Length; i++) {
				sum = 0;
				for (int j = 0; j < arr [i].Length; j++) {
					sum += (int)arr [i] [j];
				}
				if (sum != d (arr, i))
					return false;
			}

			return true;
		}

		public static void ManagedXor(byte[] arr,byte xored)
		{
			for (int i = 0; i < arr.Length; i++) {
				arr [i] ^= xored;
			}
		}

		public static bool Validate(byte[] arr,byte xored, XorDelegate d)
		{
			int sum = 0;
			byte[] old = (byte[])arr.Clone ();
			d (arr, xored);

			for (int i = 0; i < arr.Length; i++) {
				if ((old[i]^xored)!=arr[i])
					return false;
			}

			return true;
		}

		[DllImport ("libperf.so",EntryPoint="internalCount")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public static extern int InternalCount(string[] arr, int i);

		[DllImport ("libperf.so",EntryPoint="unmanagedCount")]
		public static extern int UnmanagedCount([MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr)]
			string[] arr, int i);

		[DllImport ("libperf.so",EntryPoint="internalXor")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public static extern void InternalXor(byte[] arr, byte xored);

		[DllImport ("libperf.so",EntryPoint="unmanagedXor")]
		public static extern void UnmanagedXor([In, Out]byte[] arr, byte xored);

		[DllImport ("libperf.so",EntryPoint="init")]
		public static extern void InitInternals();

		[DllImport ("libperf.so",EntryPoint="getNameInternal")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public static extern string getNameInternal(object t);

	}
}

