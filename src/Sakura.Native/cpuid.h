#ifndef SAKURA_CPUID_H
#define SAKURA_CPUID_H

#include <intrin.h>
#include <VersionHelpers.h>

//-----------------------------------------------------------------------------
// This CPU-testing code originally came from MDSN, but has been severely
// hacked for our needs.
//
// https://docs.microsoft.com/en-us/cpp/intrinsics/cpuid-cpuidex

class CpuId {
private:
	struct CpuIdResult
	{
		int eax;
		int ebx;
		int ecx;
		int edx;
	};

private:
	int numIds;
	int numExtendedIds;
	char vendor[64];
	char brand[64];
	bool isIntel;
	bool isAMD;
	int function_1_ecx, function_1_edx;
	int function_7_ebx, function_7_ecx;
	int extendedFunction_1_ecx, extendedFunction_1_edx;
	CpuIdResult *data;
	CpuIdResult *extendedData;

public:
	CpuId();
	~CpuId();

	static const CpuId &Instance();

	const char *GetVendor(void) const { return vendor; }
	const char *GetBrand(void) const { return brand; }

	bool HasSSE3(void) const { return (function_1_ecx & (1 << 0)) != 0; }
	bool HasPCLMULQDQ(void) const { return (function_1_ecx & (1 << 1)) != 0; }
	bool HasMONITOR(void) const { return (function_1_ecx & (1 << 3)) != 0; }
	bool HasSSSE3(void) const { return (function_1_ecx & (1 << 9)) != 0; }
	bool HasFMA(void) const { return (function_1_ecx & (1 << 12)) != 0; }
	bool HasCMPXCHG16B(void) const { return (function_1_ecx & (1 << 13)) != 0; }
	bool HasSSE41(void) const { return (function_1_ecx & (1 << 19)) != 0; }
	bool HasSSE42(void) const { return (function_1_ecx & (1 << 20)) != 0; }
	bool HasMOVBE(void) const { return (function_1_ecx & (1 << 22)) != 0; }
	bool HasPOPCNT(void) const { return (function_1_ecx & (1 << 23)) != 0; }
	bool HasAES(void) const { return (function_1_ecx & (1 << 25)) != 0; }
	bool HasXSAVE(void) const { return (function_1_ecx & (1 << 26)) != 0; }
	bool HasOSXSAVE(void) const { return (function_1_ecx & (1 << 27)) != 0; }
	bool HasAVX(void) const { return (function_1_ecx & (1 << 28)) != 0; }
	bool HasF16C(void) const { return (function_1_ecx & (1 << 29)) != 0; }
	bool HasRDRAND(void) const { return (function_1_ecx & (1 << 30)) != 0; }

	bool HasMSR(void) const { return (function_1_edx & (1 << 5)) != 0; }
	bool HasCX8(void) const { return (function_1_edx & (1 << 8)) != 0; }
	bool HasSEP(void) const { return (function_1_edx & (1 << 11)) != 0; }
	bool HasCMOV(void) const { return (function_1_edx & (1 << 15)) != 0; }
	bool HasCLFSH(void) const { return (function_1_edx & (1 << 19)) != 0; }
	bool HasMMX(void) const { return (function_1_edx & (1 << 23)) != 0; }
	bool HasFXSR(void) const { return (function_1_edx & (1 << 24)) != 0; }
	bool HasSSE(void) const { return (function_1_edx & (1 << 25)) != 0; }
	bool HasSSE2(void) const { return (function_1_edx & (1 << 26)) != 0; }

	bool HasFSGSBASE(void) const { return (function_7_ebx & (1 << 0)) != 0; }
	bool HasBMI1(void) const { return (function_7_ebx & (1 << 3)) != 0; }
	bool HasHLE(void) const { return (isIntel && function_7_ebx & (1 << 4)) != 0; }
	bool HasAVX2(void) const { return (function_7_ebx & (1 << 5)) != 0; }
	bool HasBMI2(void) const { return (function_7_ebx & (1 << 8)) != 0; }
	bool HasERMS(void) const { return (function_7_ebx & (1 << 9)) != 0; }
	bool HasINVPCID(void) const { return (function_7_ebx & (1 << 10)) != 0; }
	bool HasRTM(void) const { return (isIntel && function_7_ebx & (1 << 11)) != 0; }
	bool HasAVX512F(void) const { return (function_7_ebx & (1 << 16)) != 0; }
	bool HasRDSEED(void) const { return (function_7_ebx & (1 << 18)) != 0; }
	bool HasADX(void) const { return (function_7_ebx & (1 << 19)) != 0; }
	bool HasAVX512IFMA(void) const { return (function_7_ebx & (1 << 21)) != 0; }
	bool HasAVX512PF(void) const { return (function_7_ebx & (1 << 26)) != 0; }
	bool HasAVX512ER(void) const { return (function_7_ebx & (1 << 27)) != 0; }
	bool HasAVX512CD(void) const { return (function_7_ebx & (1 << 28)) != 0; }
	bool HasAVX512BW(void) const { return (function_7_ebx & (1 << 30)) != 0; }
	bool HasAVX512VL(void) const { return (function_7_ebx & (1 << 31)) != 0; }
	bool HasSHA(void) const { return (function_7_ebx & (1 << 29)) != 0; }

	bool HasPREFETCHWT1(void) const { return (function_7_ecx & (1 << 0)) != 0; }
	bool HasAVX512VBMI(void) const { return (function_7_ecx & (1 << 1)) != 0; }
	bool HasAVX512VBMI2(void) const { return (function_7_ecx & (1 << 6)) != 0; }

	bool HasLAHF(void) const { return (extendedFunction_1_ecx & (1 << 0)) != 0; }
	bool HasLZCNT(void) const { return (isIntel && extendedFunction_1_ecx & (1 << 5)) != 0; }
	bool HasABM(void) const { return (isAMD && extendedFunction_1_ecx & (1 << 5)) != 0; }
	bool HasSSE4a(void) const { return (isAMD && extendedFunction_1_ecx & (1 << 6)) != 0; }
	bool HasXOP(void) const { return (isAMD && extendedFunction_1_ecx & (1 << 11)) != 0; }
	bool HasTBM(void) const { return (isAMD && extendedFunction_1_ecx & (1 << 21)) != 0; }

	bool HasSYSCALL(void) const { return (isIntel && extendedFunction_1_edx & (1 << 11)) != 0; }
	bool HasMMXEXT(void) const { return (isAMD && extendedFunction_1_edx & (1 << 22)) != 0; }
	bool HasRDTSCP(void) const { return (isIntel && extendedFunction_1_edx & (1 << 27)) != 0; }
	bool Has3DNOWEXT(void) const { return (isAMD && extendedFunction_1_edx & (1 << 30)) != 0; }
	bool Has3DNOW(void) const { return (isAMD && extendedFunction_1_edx & (1 << 31)) != 0; }
};

#endif