#ifndef SAKURA_IMG_H
#define SAKURA_IMG_H

#ifndef SAKURA_CONFIG_H
#include "config.h"
#endif

extern "C" {
	SAKURA_EXPORT void DemoteImage16To8(void *dest, const void *src, int width, int height, int srcSpan, int destSpan);
};

#endif