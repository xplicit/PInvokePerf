CC=gcc
CFLAGS=-O2 `pkg-config --cflags glib-2.0` `pkg-config --cflags mono-2`
LDFLAGS=`pkg-config --libs glib-2.0` `pkg-config --libs mono-2`

all: libperf

libperf: libperf.o
	$(CC) $(LDFLAGS) -shared libperf.o -o libperf.so
	cp libperf.so ../PInvokePerf/bin/Release

libperf.o:
	$(CC) $(CFLAGS) -g -c -fPIC libperf.c -o libperf.o

clean:
	rm -rf libperf.so libperf.o