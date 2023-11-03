#include <stdexcept>
#include <filesystem>
#include <fstream>
#include <codecvt>

#ifdef WINDOWS
    #include <shlobj.h>
    #include <winnls.h>
#endif

#include "nlohmann/json.hpp"
#include "configuration.h"

namespace fs = std::filesystem;
using json = nlohmann::json;

configuration GetLocalDefaultConfig();

fs::path config_file_path;
configuration config;

void Initialize()
{
    std::wstring user_data_dir; //Indicate the directory where user-specific data is stored.

#ifdef WINDOWS
    PWSTR user_data_dir_ptr = nullptr;
    if (SUCCEEDED(SHGetKnownFolderPath(FOLDERID_RoamingAppData, 0, nullptr, &user_data_dir_ptr)))
    {
        user_data_dir = user_data_dir_ptr;
        CoTaskMemFree(user_data_dir_ptr);
    }
    else throw std::runtime_error("(001) Error: Failed to get the user data directory.");
#endif

    fs::path config_dir = fs::path(user_data_dir) / "ServerEase";
    config_file_path = config_dir / "Config.json";

    if (!(fs::exists(config_dir) && fs::is_directory(config_dir)))
        fs::create_directory(config_dir);

    if (fs::exists(config_file_path) && fs::is_regular_file(config_file_path))
    {
        try
        {
            std::ifstream config_file(config_file_path);
            if (config_file.is_open())
            {
                json config_content = json::parse(config_file);
                config.lang = config_content["lang"];
            }
            else throw;
        }
        catch (...)
        {
            throw std::runtime_error("(002) Error: Failed to get data from the configuration file(s).");
        }
    }
    else
    {
        try
        {
            std::ofstream config_file(config_file_path);
            if (config_file.is_open())
            {
                config = GetLocalDefaultConfig();
                json json_content;
                json_content["lang"] = config.lang;
                config_file << json_content.dump(4) << std::endl;
            }
            else throw;
        }
        catch (...)
        {
            throw std::runtime_error("(003) Error: Failed to create new configuration file(s).");
        }
    }
}

configuration GetLocalDefaultConfig()
{
    configuration default_config;

#pragma region Determin Default Language

    //Logics determine default language name.
    std::string default_locale_name, user_locale;

    //Determine the user's locale language, based on the platform.
#ifdef WINDOWS
    wchar_t wide_user_locale_name[LOCALE_NAME_MAX_LENGTH]; //The memory size allocated for this variable must be limited; otherwise, access violations may occur.
    if (GetUserDefaultLocaleName(wide_user_locale_name, LOCALE_NAME_MAX_LENGTH) != 0)
    {
        //Convert wstring -> string.
        std::wstring_convert<std::codecvt_utf8<wchar_t>> converter;
        user_locale = converter.to_bytes(wide_user_locale_name);
    }
    else throw;
#endif

    //Check whether user's language is supported; if not, set it to en-US.
    bool language_supported = false;
    for (const std::string &lang : SUPPORTED_LANGUAGES)
    {
        if (lang == user_locale)
        {
            language_supported = true;
            break;
        }
    }

    if (language_supported)
        default_locale_name = user_locale;
    else
        default_locale_name = SUPPORTED_LANGUAGES[0];

#pragma endregion Determin Default Language

    default_config.lang = default_locale_name;
    return default_config;
}

const char* GetLanguage()
{
    return config.lang.c_str();
}

/// Set UI Language and flush it to the configuration file.
/// \param lang To set language name.
/// \return Whether the setting is succeed.
bool SetLanguage(const char* lang)
{
    config.lang = lang;
    std::ofstream config_file(config_file_path);
    if (config_file.is_open())
    {
        json config_content;
        config_content["lang"] = config.lang;
        config_file << config_content.dump(4) << std::endl;
        return true;
    }
    else return false;
}
