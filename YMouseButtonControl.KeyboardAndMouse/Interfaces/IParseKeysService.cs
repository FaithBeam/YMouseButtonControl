using System.Collections.Generic;
using YMouseButtonControl.KeyboardAndMouse.Models;

namespace YMouseButtonControl.KeyboardAndMouse.Interfaces;

public interface IParseKeysService
{
    List<ParsedKey> ParseKeys(string? keys);
}
