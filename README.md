Mono PInvoke performance tests
===============================

These tests compares performance of various aproaches for working with user data.

How to run
-------------------------------
Execute following commands in command line

    make clean
    make
    make run

Tests    
-------------------------------
The first test is a sum counting of character codes in string from array of strings. So there is 

    string[] arr=new string[100];
Each string contains 100 ASCII chars, and tested function computes sum of these chars

    for(int j=0;j<arr[index].Length;j++)
      sum+=arr[index][j];
      
Second test encodes buffer of length 16384 with fixed xor mask. 

    for(int i=0;i<buffer.Length;i++)
      buffer[i]^=mask;

Results
-------------------------------
In results you will see execution time in milleseconds for 100K calls of the tests.

* `Managed` 
All calculations perform in managed code

* `PInvoke`
Calculations on data perform in unmanaged code working with unmanaged data. Calls to unmanaged code are made with PInvoke mechanism

* `Internal Call`
Calculations on data perform in unmanaged code working with MANAGED data in contrary to PInvoke. Calls to unmanaged code are made with Mono Internal Calls.


