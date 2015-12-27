#include <stdio.h>
#include <stdlib.h>
int setValue(int port, int value) { char command[50] = "";
sprintf("echo %d=%d > /dev/servoblaster", port, value);
}
