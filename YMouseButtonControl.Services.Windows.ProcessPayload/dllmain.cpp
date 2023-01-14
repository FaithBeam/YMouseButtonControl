#include <windows.h>
#include <map>
#include <string>
#include <iostream>
#include <fstream>

typedef int disabled;
std::map<UINT, disabled> key_disabled;

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
        case DLL_THREAD_ATTACH:
        case DLL_THREAD_DETACH:
        case DLL_PROCESS_DETACH:
            break;
    }

    return TRUE;
}

// Do not call the next hook if the key is disabled.
// returning -1 is how to prevent a key from reaching a thread
LRESULT handle_key(UINT key, const int n_code, const WPARAM w_param, const LPARAM l_param)
{
    if (key_disabled[key])
    {
        return -1;
    }
    return CallNextHookEx(nullptr, n_code, w_param, l_param);
}

// Handles lmb, rmb, mmb
LRESULT handle_normal_key(const int n_code, const WPARAM w_param, const LPARAM l_param)
{
    return handle_key(w_param, n_code, w_param, l_param);
}

// Handles xbutton1, xbutton2
LRESULT handle_xmb_keys(const int n_code, const WPARAM w_param, const LPARAM l_param)
{
    const auto* xmd_struct = reinterpret_cast<MOUSEHOOKSTRUCTEX*>(l_param);
    const auto xmb = HIWORD(xmd_struct->mouseData);
    std::ofstream file("sample.txt", std::ios_base::app);
    file << xmb << "\n";
    file.flush();
    file.close();
    return handle_key(xmb, n_code, w_param, l_param);
}

// Every time the mouse is pressed in the injected(?) thread(?), this callback is fired.
// This allows us to prevent mouse buttons from reaching the injected(?) thread.
extern "C" __declspec(dllexport) int start_callback(const int n_code, const WPARAM w_param, const LPARAM l_param)
{
    if (n_code < 0 || n_code != HC_ACTION)
    {
        return CallNextHookEx(nullptr, n_code, w_param, l_param);
    }

    std::ofstream file("callback.txt", std::ios_base::app);
    file << w_param << "\n";
    file.flush();
    file.close();
    switch (w_param)
    {
        case WM_LBUTTONDOWN:
        case WM_LBUTTONUP:
        case WM_RBUTTONDOWN:
        case WM_RBUTTONUP:
        case WM_MBUTTONDOWN:
        case WM_MBUTTONUP:
            return handle_normal_key(n_code, w_param, l_param);
        case WM_XBUTTONDOWN:
        case WM_XBUTTONUP: {
            std::ofstream file2("xbutton.txt", std::ios_base::app);
            file2 << w_param << "\n";
            file2.flush();
            file2.close();
            return handle_xmb_keys(n_code, w_param, l_param);
        }
        default:
            break;
    }
    return CallNextHookEx(nullptr, n_code, w_param, l_param);
}

// Update the disabled keys map with new values
// Valid keys are lmb, rmb, mmb, mb4, mb5
extern "C" __declspec(dllexport) void update_disabled_keys(LPCWSTR key, UINT disabled)
{
    std::wstring ws(key);
    auto parsed = std::string(ws.begin(), ws.end());
    if (parsed.compare("lmb") == 0)
    {
        key_disabled.insert_or_assign(WM_LBUTTONDOWN, disabled);
        key_disabled.insert_or_assign(WM_LBUTTONUP, disabled);
    }
    else if (parsed.compare("rmb") == 0)
    {
        key_disabled.insert_or_assign(WM_RBUTTONDOWN, disabled);
        key_disabled.insert_or_assign(WM_RBUTTONUP, disabled);
    }
    else if (parsed.compare("mmb") == 0)
    {
        key_disabled.insert_or_assign(WM_MBUTTONUP, disabled);
        key_disabled.insert_or_assign(WM_MBUTTONDOWN, disabled);
    }
    else if (parsed.compare("mb4") == 0)
    {
        key_disabled.insert_or_assign(XBUTTON1, disabled);
    }
    else if (parsed.compare("mb5") == 0)
    {
        key_disabled.insert_or_assign(XBUTTON2, disabled);
    }
}