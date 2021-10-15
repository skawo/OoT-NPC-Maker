#include "../include/h_lib.h"

s32 Lib_StringCompare(char* that, char* this, u32 size) 
{
    if (that == NULL || this == NULL)
        return -1;
    
    for (s32 i = 0; i < size; i++) 
    {
        if (that[i] != this[i])
            return 0;
    }
    
    return 1;
}