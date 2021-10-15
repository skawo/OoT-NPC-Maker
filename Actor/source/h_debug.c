#include "../include/h_debug.h"
 
#if LOGGING == 1

ReadableTime Time_Convert(u16 time)
{
    ReadableTime out;

    float tMinutes = (float)time / (float)((float)65535 / (float)1440);

    out.hour = tMinutes / 60;
    out.minutes = tMinutes - (out.hour * 60);

    return out;
}

#endif