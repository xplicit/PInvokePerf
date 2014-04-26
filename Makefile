SUBDIRS = libperf

all: pinvokeperf subdirs

pinvokeperf:
	xbuild /p:Configuration=Release PInvokePerf.sln
     
.PHONY: subdirs $(SUBDIRS)
     
subdirs: $(SUBDIRS)
     
$(SUBDIRS):
	$(MAKE) -C $@

run:
	# 'unsafe' is a most powerfull optimization for mono, so use it to get better results for managed code
	# --aot is REQUIRED for internal calls. Otherwise you'll get MissingMethodException 
	cd PInvokePerf/bin/Release && ls -al && mono --aot --optimize=unsafe ./PInvokePerf.exe && mono --optimize=unsafe ./PInvokePerf.exe

clean:
	cd libperf && make clean