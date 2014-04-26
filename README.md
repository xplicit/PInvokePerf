Mono PInvoke performance tests
===============================

These tests compares performance of various aproaches for working with user data.

How to run
-------------------------------
Execute following commands in command line

    make clean
    make
    make run

Managed code
-------------------------------
All calculations are done in managed code

PInvoke
-------------------------------
Calculations on data are done in unmanaged code working with unmanaged data. Calls to unmanaged code by PInvoke mechanism

Internal Call
-------------------------------
Calculations on data are done in unmanaged code working with MANAGED data in contrary to PInvoke. Calls to unmanaged code is maden with Mono Internal Calls.


