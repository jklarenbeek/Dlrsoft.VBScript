// 
// Visual Basic .NET Parser
// 
// Copyright (C) 2005, Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
// EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
// MERCHANTIBILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// 

using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualBasic.CompilerServices;

namespace Dlrsoft.VBScript.Parser
{
    public class ErrorXmlSerializer
    {
        private readonly XmlWriter Writer;

        public ErrorXmlSerializer(XmlWriter Writer)
        {
            this.Writer = Writer;
        }

        private void Serialize(Span Span)
        {
            Writer.WriteAttributeString("startLine", Conversions.ToString(Span.Start.Line));
            Writer.WriteAttributeString("startCol", Conversions.ToString(Span.Start.Column));
            Writer.WriteAttributeString("endLine", Conversions.ToString(Span.Finish.Line));
            Writer.WriteAttributeString("endCol", Conversions.ToString(Span.Finish.Column));
        }

        public void Serialize(SyntaxError SyntaxError)
        {
            Writer.WriteStartElement(SyntaxError.Type.ToString());
            Serialize(SyntaxError.Span);
            Writer.WriteString(SyntaxError.ToString());
            Writer.WriteEndElement();
        }

        public void Serialize(List<SyntaxError> SyntaxErrors)
        {
            foreach (SyntaxError SyntaxError in SyntaxErrors)
                Serialize(SyntaxError);
        }
    }
}