#ifndef SAKURA_CONFIG_H
#define SAKURA_CONFIG_H

#ifndef __cplusplus
#	error This requires a C++ compiler.
#endif

#if defined(_MSC_VER)
#	ifdef SAKURA_IMPL
#		define SAKURA_EXPORT __declspec(dllimport)
#	else
#		define SAKURA_EXPORT __declspec(dllexport)
#	endif
#	define WIN32_LEAN_AND_MEAN
#	include <windows.h>
#else
#	define SAKURA_EXPORT extern
#endif

#include <cstdint>
#include <cstddef>
#include <cstdarg>
#include <cstdio>
#include <cstdlib>
#include <cstring>

#include <xmmintrin.h>
#include <immintrin.h>

#endif