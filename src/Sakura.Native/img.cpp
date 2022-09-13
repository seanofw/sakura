#include "pch.h"
#include "img.h"
#include "cpuid.h"

#include <emmintrin.h>
#include <tmmintrin.h>
#include <mmintrin.h>

static void DemoteImageInParallel(void *dest, const void *src, int width, int height, int srcSpan, int destSpan)
{
	uint8_t *destRowStart = (uint8_t *)dest;
	uint16_t *srcRowStart = (uint16_t *)src;

	__m128i shuffler1 = _mm_set_epi8(-1, -1, -1, -1, -1, -1, -1, -1, 15, 9, 11, 13, 7, 1, 3, 5);
	__m128i shuffler2 = _mm_set_epi8(15, 9, 11, 13, 7, 1, 3, 5, -1, -1, -1, -1, -1, -1, -1, -1);

	for (int y = 0; y < height; y++) {
		uint8_t *destPtr = destRowStart;
		uint16_t *srcPtr = srcRowStart;

		uint16_t *srcEnd = srcPtr + (width & ~7) * 4;
		while (srcPtr < srcEnd) {
			// Heavily-unrolled loop:  Reads in 64 bytes at a time, writes out 32, and
			// does all of the intermediate math in parallel.
			__m128i v12 = _mm_loadu_si128((__m128i *)srcPtr);
			__m128i v34 = _mm_loadu_si128((__m128i *)(srcPtr + 8));
			__m128i v56 = _mm_loadu_si128((__m128i *)(srcPtr + 16));
			__m128i v78 = _mm_loadu_si128((__m128i *)(srcPtr + 24));
			__m128i r12 = _mm_shuffle_epi8(v12, shuffler1);
			__m128i r34 = _mm_shuffle_epi8(v34, shuffler2);
			__m128i r56 = _mm_shuffle_epi8(v56, shuffler1);
			__m128i r78 = _mm_shuffle_epi8(v78, shuffler2);
			__m128i fused1 = _mm_or_si128(r12, r34);
			__m128i fused2 = _mm_or_si128(r56, r78);
			_mm_storeu_si128((__m128i *)destPtr, fused1);
			_mm_storeu_si128((__m128i *)(destPtr + 16), fused2);

			destPtr += 32;
			srcPtr += 32;
		}

		if (width & 4) {
			// Read in 32 bytes, write out 16.
			__m128i v12 = _mm_loadu_si128((__m128i *)srcPtr);
			__m128i v34 = _mm_loadu_si128((__m128i *)(srcPtr + 8));
			__m128i r12 = _mm_shuffle_epi8(v12, shuffler1);
			__m128i r34 = _mm_shuffle_epi8(v34, shuffler2);
			__m128i fused = _mm_or_si128(r12, r34);
			_mm_storeu_si128((__m128i *)destPtr, fused);

			srcPtr += 16;
			destPtr += 16;
		}

		if (width & 2) {
			// Read in 16 bytes, write out 8.
			__m128i v12 = _mm_loadu_si128((__m128i *)srcPtr);
			__m128i r12 = _mm_shuffle_epi8(v12, shuffler1);
			_mm_storeu_epi64((__m128i *)destPtr, r12);

			srcPtr += 8;
			destPtr += 8;
		}

		if (width & 1) {
			// Read in 8 bytes, write out 4.
			__m128i v1 = _mm_loadl_epi64((__m128i *)srcPtr);
			__m128i r1 = _mm_shuffle_epi8(v1, shuffler1);
			_mm_storeu_epi32((__m128i *)destPtr, r1);
		}

		destRowStart += destSpan;
		srcRowStart = (uint16_t *)((uint8_t *)srcRowStart + srcSpan);
	}
}

static void DemoteImageOldSchool(void *dest, const void *src, int width, int height, int srcSpan, int destSpan)
{
	uint8_t *destRowStart = (uint8_t *)dest;
	uint16_t *srcRowStart = (uint16_t *)src;

	for (int y = 0; y < height; y++) {
		uint8_t *destPtr = destRowStart;
		uint16_t *srcPtr = srcRowStart;

		uint16_t *srcEnd = srcPtr + (width & ~3) * 4;
		while (srcPtr < srcEnd) {
			uint64_t v1 = ((uint64_t *)srcPtr)[0];
			uint64_t v2 = ((uint64_t *)srcPtr)[1];
			uint64_t v3 = ((uint64_t *)srcPtr)[2];
			uint64_t v4 = ((uint64_t *)srcPtr)[3];
			uint32_t r1 = (
					  ((uint32_t)(v1 >> 40) & 0x000000FFU)
					| ((uint32_t)(v1 >> 16) & 0x0000FF00U)
					| ((uint32_t)(v1 <<  8) & 0x00FF0000U)
					| ((uint32_t)(v1 >> 32) & 0xFF000000U)
				);
			uint32_t r2 = (
					  ((uint32_t)(v2 >> 40) & 0x000000FFU)
					| ((uint32_t)(v2 >> 16) & 0x0000FF00U)
					| ((uint32_t)(v2 <<  8) & 0x00FF0000U)
					| ((uint32_t)(v2 >> 32) & 0xFF000000U)
				);
			uint32_t r3 = (
					  ((uint32_t)(v3 >> 40) & 0x000000FFU)
					| ((uint32_t)(v3 >> 16) & 0x0000FF00U)
					| ((uint32_t)(v3 <<  8) & 0x00FF0000U)
					| ((uint32_t)(v3 >> 32) & 0xFF000000U)
				);
			uint32_t r4 = (
					  ((uint32_t)(v4 >> 40) & 0x000000FFU)
					| ((uint32_t)(v4 >> 16) & 0x0000FF00U)
					| ((uint32_t)(v4 <<  8) & 0x00FF0000U)
					| ((uint32_t)(v4 >> 32) & 0xFF000000U)
				);
			((uint32_t *)destPtr)[0] = r1;
			((uint32_t *)destPtr)[1] = r2;
			((uint32_t *)destPtr)[2] = r3;
			((uint32_t *)destPtr)[3] = r4;

			srcPtr += 16;
			destPtr += 16;
		}

		if (width & 2) {
			uint64_t v1 = ((uint64_t *)srcPtr)[0];
			uint64_t v2 = ((uint64_t *)srcPtr)[1];
			uint32_t r1 = (
					  ((uint32_t)(v1 >> 40) & 0x000000FFU)
					| ((uint32_t)(v1 >> 16) & 0x0000FF00U)
					| ((uint32_t)(v1 <<  8) & 0x00FF0000U)
					| ((uint32_t)(v1 >> 32) & 0xFF000000U)
				);
			uint32_t r2 = (
					  ((uint32_t)(v2 >> 40) & 0x000000FFU)
					| ((uint32_t)(v2 >> 16) & 0x0000FF00U)
					| ((uint32_t)(v2 <<  8) & 0x00FF0000U)
					| ((uint32_t)(v2 >> 32) & 0xFF000000U)
				);
			((uint32_t *)destPtr)[0] = r1;
			((uint32_t *)destPtr)[1] = r2;

			srcPtr += 8;
			destPtr += 8;
		}

		if (width & 1) {
			uint64_t v1 = ((uint64_t *)srcPtr)[0];
			uint32_t r1 = (
					  ((uint32_t)(v1 >> 40) & 0x000000FFU)
					| ((uint32_t)(v1 >> 16) & 0x0000FF00U)
					| ((uint32_t)(v1 <<  8) & 0x00FF0000U)
					| ((uint32_t)(v1 >> 32) & 0xFF000000U)
				);
			((uint32_t *)destPtr)[0] = r1;
		}

		destRowStart += destSpan;
		srcRowStart = (uint16_t *)((uint8_t *)srcRowStart + srcSpan);
	}
}

/**
* Turn an RGBA (16,16,16,16) image into an equivalent BGRA (8,8,8,8) image.
*/
void DemoteImage16To8(void *dest, const void *src, int width, int height, int srcSpan, int destSpan)
{
	uint8_t *destRowStart = (uint8_t *)dest;
	uint16_t *srcRowStart = (uint16_t *)src;

	if (CpuId::Instance().HasAVX()) {
		DemoteImageInParallel(dest, src, width, height, srcSpan, destSpan);
	}
	else {
		DemoteImageOldSchool(dest, src, width, height, srcSpan, destSpan);
	}
}
