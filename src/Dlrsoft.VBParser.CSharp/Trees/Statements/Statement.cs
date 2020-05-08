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
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Dlrsoft.VBScript.Parser
{
    /// <summary>
/// A parse tree for a statement.
/// </summary>
    public abstract class Statement : Tree
    {
        private readonly ReadOnlyCollection<Comment> _Comments;

        /// <summary>
    /// The comments for the tree.
    /// </summary>
        public ReadOnlyCollection<Comment> Comments
        {
            get
            {
                return _Comments;
            }
        }

        protected Statement(TreeType type, Span span, IList<Comment> comments) : base(type, span)
        {

            // LC Allow declarations to be craeted as statement
            Debug.Assert(type >= TreeType.EmptyStatement && type <= TreeType.EndBlockStatement || type >= TreeType.EmptyDeclaration && type <= TreeType.DelegateFunctionDeclaration);
            if (comments is object)
            {
                _Comments = new ReadOnlyCollection<Comment>(comments);
            }
        }
    }
}