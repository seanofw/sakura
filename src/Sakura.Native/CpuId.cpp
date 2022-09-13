#include "pch.h"
#include "cpuid.h"

CpuId::CpuId()
{
	// Calling __cpuid with 0x0 as the function_id argument
	// gets the number of the highest valid function ID.
	CpuIdResult cpuIdResult;
	__cpuid((int *)&cpuIdResult, 0);
	numIds = cpuIdResult.eax;

	data = new CpuIdResult[(size_t)numIds + 1];
	for (int i = 0; i <= numIds; ++i)
	{
		__cpuidex((int *)&cpuIdResult, i, 0);
		data[i] = cpuIdResult;
	}

	// Capture vendor string
	memset(vendor, 0, sizeof(vendor));
	*(int *)(vendor) = data[0].ebx;
	*(int *)(vendor + 4) = data[0].edx;
	*(int *)(vendor + 8) = data[0].ecx;
	vendor[63] = '\0';
	isIntel = !strcmp(vendor, "GenuineIntel");
	isAMD = !strcmp(vendor, "AuthenticAMD");

	// load bitset with flags for function 0x00000001
	if (numIds >= 1)
	{
		function_1_ecx = data[1].ecx;
		function_1_edx = data[1].edx;
	}

	// load bitset with flags for function 0x00000007
	if (numIds >= 7)
	{
		function_7_ebx = data[7].ebx;
		function_7_ecx = data[7].ecx;
	}

	// Calling __cpuid with 0x80000000 as the function_id argument
	// gets the number of the highest valid extended ID.
	__cpuid((int *)&cpuIdResult, 0x80000000);
	numExtendedIds = cpuIdResult.eax & 0x7FFFFFFF;

	extendedData = new CpuIdResult[(size_t)numExtendedIds + 1];
	for (int i = 0; i <= numExtendedIds; ++i)
	{
		__cpuidex((int *)&cpuIdResult, i, 0);
		extendedData[i] = cpuIdResult;
	}

	// load bitset with flags for function 1
	if (numExtendedIds >= 1)
	{
		extendedFunction_1_ecx = extendedData[1].ecx;
		extendedFunction_1_edx = extendedData[1].edx;
	}

	// Interpret CPU brand string if reported
	memset(brand, 0, sizeof(brand));
	if (numExtendedIds >= 4)
	{
		memcpy(brand, &extendedData[2], sizeof(CpuIdResult));
		memcpy(brand + 16, &extendedData[3], sizeof(CpuIdResult));
		memcpy(brand + 32, &extendedData[4], sizeof(CpuIdResult));
	}
	brand[63] = '\0';
}

CpuId::~CpuId()
{
	delete[] data;
	delete[] extendedData;
}

static CpuId *instance;

const CpuId &CpuId::Instance()
{
	if (instance == nullptr)
		instance = new CpuId();
	return *instance;
}
