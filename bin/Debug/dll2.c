#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <signal.h>
#include <fcntl.h>
#include <string.h>
#include <time.h>
#include "sensor.c"

extern int __cdecl setValue(int port, int value)
{
char command[50] = "";
sprintf("echo %d=%d > /dev/servoblaster", port, value);
}

extern int __cdecl 