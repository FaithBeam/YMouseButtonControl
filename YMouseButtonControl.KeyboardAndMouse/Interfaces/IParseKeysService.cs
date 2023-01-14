using System.Collections.Generic;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IParseKeysService
{
    List<string> ParseKeys(string keys);
}