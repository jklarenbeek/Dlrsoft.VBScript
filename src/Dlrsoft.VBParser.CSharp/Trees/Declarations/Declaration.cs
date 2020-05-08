// 
// Visual Basic .NET Parser
// 
// Copyright (C) 2005, Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
// EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
// MERCHANTIBILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// 
// LC Changed Declaration to Inherits from Statement instead of tree so that we can add declaration to statement collection

/// <summary>
/// A parse tree for a declaration.
/// </summary>
using System.Collections.Generic;
using System.Diagnostics;

namespace Dlrsoft.VBScript.Parser
{
    public class Declaration : Statement
    {

        /// <summary>
    /// Creates a bad declaration.
    /// </summary>
    /// <param name="span">The location of the parse tree.</param>
    /// <param name="comments">The comments for the parse tree.</param>
    /// <returns>A bad declaration.</returns>
        public static Declaration GetBadDeclaration(Span span, IList<Comment> comments)
        {
            return new Declaration(span, comments);
        }

        protected Declaration(TreeType type, Span span, IList<Comment> comments) : base(type, span, comments)
        {
            Debug.Assert(type >= TreeType.EmptyDeclaration && type <= TreeType.DelegateFunctionDeclaration);
        }

        private Declaration(Span span, IList<Comment> comments) : base(TreeType.SyntaxError, span, comments)
        {
        }

        public override bool IsBad
        {
            get
            {
                return Type == TreeType.SyntaxError;
            }
        }
    }
}