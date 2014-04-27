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
First test is a sum counting of character codes in string from array of strings. So there is 

    string[] arr=new string[100];
Each string contains 100 ASCII chars, and tested functions computes sum of string chars

    for(int j=0;j<arr[index].Length;j++)
      sum+=arr[index][j];

Results
-------------------------------
In results you will see execution time in milleseconds for 10M calls of the various function implementations.

Managed code
-------------------------------
All calculations are done in managed code

PInvoke
-------------------------------
Calculations on data are done in unmanaged code working with unmanaged data. Calls to unmanaged code by PInvoke mechanism

Internal Call
-------------------------------
Calculations on data are done in unmanaged code working with MANAGED data in contrary to PInvoke. Calls to unmanaged code is maden with Mono Internal Calls.


