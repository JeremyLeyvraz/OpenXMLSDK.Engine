﻿using DocumentFormat.OpenXml.Wordprocessing;
using MvvX.Open_XML_SDK.Core.Word;
using MvvX.Open_XML_SDK.Core.Word.Bases;

namespace MvvX.Open_XML_SDK.Shared.Word
{
    public class PlatformShading : PlatformOpenXmlElement, IShading
    {
        private readonly Shading shading;

        public PlatformShading(Shading shading)
            : base(shading)
        {
            this.shading = shading;
        }
    }
}
