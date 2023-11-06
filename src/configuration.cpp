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
bool FlushConfigFile(const configuration &write_config);

fs::path config_file_path;
configuration config {};

/// Initialize the configuration data. Invoke this before calling any functions in this file.
/// \return Indicate whether this is the first time it being launched.
bool Initialize()
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
        //If the configuration file exists.
        try
        {
            std::ifstream config_file(config_file_path);
            if (config_file.is_open())
            {
                json config_content = json::parse(config_file);
                config.lang = config_content["lang"];
                config.main_window_height = config_content["main_window"]["height"];
                config.main_window_width = config_content["main_window"]["width"];
            }
            else throw;
            return false;
        }
        catch (...)
        {
            throw std::runtime_error("(002) Error: Failed to get data from the configuration file(s).");
        }
    }
    else
    {
        //If the configuration file does not exist.
        try
        {
            config = GetLocalDefaultConfig();
            if(!FlushConfigFile(config)) throw;
            return true;
        }
        catch (...)
        {
            throw std::runtime_error("(003) Error: Failed to create new configuration file(s).");
        }
    }
}

configuration GetLocalDefaultConfig()
{
    configuration default_config {};

#pragma region Determin Default Language

    //Logics determine default language name.
    std::string default_lang_name, user_locale, user_lang; //Final result, locale with a region info and language name without a region info.

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

    //Exclude region info.
    std::istringstream slicer(user_locale);
    std::getline(slicer, user_lang, '-');

    //Check whether user's language is supported; if not, set it to en.
    bool language_supported = false;
    for (const std::string &lang : SUPPORTED_LANGUAGES)
    {
        if (lang == user_lang)
        {
            language_supported = true;
            break;
        }
    }

    if (language_supported)
    {
        //Further check is needed if the user language is Chinese.
        if (user_lang == "zh")
        {
            //Determine whether Simplified or Tradition Chinese should be used.
            if (user_locale == "zh-CN")
                default_lang_name = "zh-CN";
            else
                default_lang_name = "zh-TR";
        }
        else default_lang_name = user_lang;
    }
    else
        default_lang_name = SUPPORTED_LANGUAGES[0];

#pragma endregion Determin Default Language

    default_config.lang = default_lang_name;
    return default_config;
}

//All the functions below must be called after Initialize() has been invoked.
//Do not attempt to get/set any value prior to the initialization.

bool FlushConfigFile(const configuration &write_config)
{
    std::ofstream config_file(config_file_path);
    if (config_file.is_open())
    {
        json config_content;
        config_content["lang"] = write_config.lang;
        config_content["main_window"]["height"] = write_config.main_window_height;
        config_content["main_window"]["width"] = write_config.main_window_width;
        config_file << config_content.dump(4) << std::endl;
        return true;
    }
    else return false;
}

const char* GetLanguage()
{
    return config.lang.c_str();
}

/// Set UI Language and flush it to the configuration file.
/// \param lang To set language name.
/// \return Whether the setting operation is succeed.
bool SetLanguage(const char* lang)
{
    config.lang = lang;
    return FlushConfigFile(config);
}

double GetMainWindowHeight()
{
    return config.main_window_height;
}
double GetMainWindowWidth()
{
    return config.main_window_width;
}

/// Set the size of the MainWindow in the user interface.
/// \param height Height of the MainWindow.
/// \param width Width of the MainWindow.
/// \return Whether the setting operation is succeed.
bool SetMainWindowSize(double height, double width)
{
    config.main_window_height = height;
    config.main_window_width = width;
    return FlushConfigFile(config);
}

/**
 * configuration.cpp TODOs List:
 * DONE: I. Implement a feature to store the user's customized size of the MainWindow and retrieve this value upon each startup.
 * TODO: II. Implement a feature to store user's selected directory as default game files folder.
 * TODO: III. Implement a feature to store the user's preferred default Microsoft Account.
 */
