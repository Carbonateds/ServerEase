#ifndef SERVEREASE_CONFIGURATION_H
#define SERVEREASE_CONFIGURATION_H

#include <vector>

#include "util.h"

typedef struct configuration
{
    std::string lang;
    double main_window_height = 500;
    double main_window_width = 800;
}
configuration;

const std::vector<std::string> SUPPORTED_LANGUAGES {"en", "zh", "de", "fr", "ru"};

extern "C" EXPORT void Initialize();
extern "C" EXPORT const char* GetLanguage();
extern "C" EXPORT bool SetLanguage(const char* lang);
extern "C" EXPORT double GetMainWindowHeight();
extern "C" EXPORT double GetMainWindowWidth();
extern "C" EXPORT bool SetMainWindowSize(double height, double width);

#endif //SERVEREASE_CONFIGURATION_H
