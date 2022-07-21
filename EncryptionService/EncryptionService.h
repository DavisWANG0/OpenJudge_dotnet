#pragma once

#pragma once

#ifdef CPPDLL_EXPORTS  
#define CPPDLL_EXPORTS __declspec(dllexport)   
#else  
#define CPPDLL_EXPORTS __declspec(dllimport)   
#endif  


CPPDLL_EXPORTS int Add(int a, int b);

