// The functions contained in this file are pretty dummy
// and are included only as a placeholder. Nevertheless,
// they *will* get included in the shared library if you
// don't remove them :)
//
// Obviously, you 'll have to write yourself the super-duper
// functions to include in the resulting library...
// Also, it's not necessary to write every function in this file.
// Feel free to add more files in this project. They will be
// included in the resulting library.

// A function adding two integers and returning the result
#include <glib.h>
#include <mono/metadata/object.h>
#include <mono/metadata/metadata.h>
#include <mono/metadata/class.h>
#include <mono/metadata/appdomain.h>
#include <mono/metadata/loader.h>


int SampleAddInt(int i1, int i2)
{
    return i1 + i2;
}

// A function doing nothing ;)
void SampleFunction1()
{
    // insert code here
}

// A function always returning zero
int SampleFunction2()
{
    // insert code here

    return 0;
}

MonoString*
get_Name (MonoObject *obj)
{
    MonoDomain *domain = mono_domain_get ();
    MonoClass *klass = mono_object_get_class(obj);

    if (mono_type_is_byref(mono_class_get_type(klass)))
    {
        char *n = g_strdup_printf ("%s&", mono_class_get_name(klass));
        MonoString *res = mono_string_new (domain, n);

        g_free (n);

        return res;
    }
    else
    {
        return mono_string_new (domain, mono_class_get_name(klass));
    }
}

MonoString*
getNameInternal (MonoObject *obj)
{
    MonoDomain *domain = mono_domain_get ();
    MonoClass *klass = mono_object_get_class(obj);

    char* h="Hello";
    return mono_string_new (domain, h);

    if (mono_type_is_byref(mono_class_get_type(klass)))
    {
        char *n = g_strdup_printf ("%s&", mono_class_get_name(klass));
        MonoString *res = mono_string_new (domain, n);

        g_free (n);

        return res;
    }
    else
    {
        return mono_string_new (domain, mono_class_get_name(klass));
    }
}

int
internalCount (MonoArray *arr,int index)
{
    MonoString* el=mono_array_get(arr,MonoString *,index);
    int len=mono_string_length(el);
    gint32 sum=0;
    guint16 *str=mono_string_chars(el);
    int i;

    for(i=0;i<len;i++)
    {
        sum+=str[i];
    }

    return sum;
}

int
unmanagedCount (guint16 **arr,int index)
{
    int sum=0;

    guint16 *str=arr[index];

    while(*str)
    {
        sum+=*str;
        str++;
    }

    return sum;
}

gpointer
getAddr()
{
    return getNameInternal;
}

void
init()
{
    mono_add_internal_call ("PInvokePerf.PerformanceTest::InternalCount(string[],int)",internalCount);
    mono_add_internal_call ("PInvokePerf.PerformanceTest::getNameInternal(object)",getNameInternal);

}
