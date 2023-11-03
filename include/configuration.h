#ifndef SERVEREASE_CONFIGURATION_H
#define SERVEREASE_CONFIGURATION_H

#include <vector>

#include "util.h"

typedef struct configuration
{
    std::string lang;
}
configuration;

const std::vector<std::string> SUPPORTED_LANGUAGES {"en-US", "zh-CN", "zh-TR", "de-DE", "fr-FR", "ru-RU"};

extern "C" EXPORT void Initialize();
extern "C" EXPORT const char* GetLanguage();
extern "C" EXPORT bool SetLanguage(const char* lang);

#endif //SERVEREASE_CONFIGURATION_H
