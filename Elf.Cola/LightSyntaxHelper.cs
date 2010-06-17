using System;
using Elf.Helpers;
using Elf.Syntax.Light;

namespace Elf.Cola
{
    public static class LightSyntaxHelper
    {
        public static String ToCanonicalCola(this String elfLight)
        {
            return elfLight.ToCanonicalElf(typeof(CocaScriptHost).RtimplOf());
        }
    }
}