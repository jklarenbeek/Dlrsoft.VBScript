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
    public class TokenXmlSerializer
    {
        private readonly XmlWriter Writer;

        public TokenXmlSerializer(XmlWriter Writer)
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

        private Dictionary<TypeCharacter, string> TypeCharacterTable;
        private void Serialize(TypeCharacter TypeCharacter)
        {
            if (TypeCharacter != TypeCharacter.None)
            {
                if (TypeCharacterTable is null)
                {
                    var Table = new Dictionary<TypeCharacter, string>();
                    // NOTE: These have to be in the same order as the enum!
                    var TypeCharacters = new string[] { "$", "%", "&", "S", "I", "L", "!", "#", "@", "F", "R", "D", "US", "UI", "UL" };
                    var TableTypeCharacter = TypeCharacter.StringSymbol;
                    for (int Index = 0, loopTo = TypeCharacters.Length - 1; Index <= loopTo; Index++)
                    {
                        Table.Add(TableTypeCharacter, TypeCharacters[Index]);
                        TableTypeCharacter = (TypeCharacter)Conversions.ToInteger((int)TableTypeCharacter << 1);
                    }

                    TypeCharacterTable = Table;
                }

                Writer.WriteAttributeString("typeChar", TypeCharacterTable[TypeCharacter]);
            }
        }

        public void Serialize(Token Token)
        {
            Writer.WriteStartElement(Token.Type.ToString());
            Serialize(Token.Span);
            var switchExpr = Token.Type;
            switch (switchExpr)
            {
                case TokenType.LexicalError:
                    {
                        {
                            var withBlock = (ErrorToken)Token;
                            Writer.WriteAttributeString("errorNumber", Conversions.ToString(withBlock.SyntaxError.Type));
                            Writer.WriteString(withBlock.SyntaxError.ToString());
                        }

                        break;
                    }

                case TokenType.Comment:
                    {
                        {
                            var withBlock1 = (CommentToken)Token;
                            Writer.WriteAttributeString("isRem", Conversions.ToString(withBlock1.IsREM));
                            Writer.WriteString(withBlock1.Comment);
                        }

                        break;
                    }

                case TokenType.Identifier:
                    {
                        {
                            var withBlock2 = (IdentifierToken)Token;
                            Writer.WriteAttributeString("escaped", Conversions.ToString(withBlock2.Escaped));
                            Serialize(withBlock2.TypeCharacter);
                            Writer.WriteString(withBlock2.Identifier);
                        }

                        break;
                    }

                case TokenType.StringLiteral:
                    {
                        {
                            var withBlock3 = (StringLiteralToken)Token;
                            Writer.WriteString(withBlock3.Literal);
                        }

                        break;
                    }

                case TokenType.CharacterLiteral:
                    {
                        {
                            var withBlock4 = (CharacterLiteralToken)Token;
                            Writer.WriteString(Conversions.ToString(withBlock4.Literal));
                        }

                        break;
                    }

                case TokenType.DateLiteral:
                    {
                        {
                            var withBlock5 = (DateLiteralToken)Token;
                            Writer.WriteString(Conversions.ToString(withBlock5.Literal));
                        }

                        break;
                    }

                case TokenType.IntegerLiteral:
                    {
                        {
                            var withBlock6 = (IntegerLiteralToken)Token;
                            Writer.WriteAttributeString("base", withBlock6.IntegerBase.ToString());
                            Serialize(withBlock6.TypeCharacter);
                            Writer.WriteString(Conversions.ToString(withBlock6.Literal));
                        }

                        break;
                    }

                case TokenType.FloatingPointLiteral:
                    {
                        {
                            var withBlock7 = (FloatingPointLiteralToken)Token;
                            Serialize(withBlock7.TypeCharacter);
                            Writer.WriteString(Conversions.ToString(withBlock7.Literal));
                        }

                        break;
                    }

                case TokenType.DecimalLiteral:
                    {
                        {
                            var withBlock8 = (DecimalLiteralToken)Token;
                            Serialize(withBlock8.TypeCharacter);
                            Writer.WriteString(Conversions.ToString(withBlock8.Literal));
                        }

                        break;
                    }

                case TokenType.UnsignedIntegerLiteral:
                    {
                        {
                            var withBlock9 = (UnsignedIntegerLiteralToken)Token;
                            Serialize(withBlock9.TypeCharacter);
                            Writer.WriteString(Conversions.ToString(withBlock9.Literal));
                        }

                        break;
                    }

                default:
                    {
                        break;
                    }
                    // Fall through
            }

            Writer.WriteEndElement();
        }

        public void Serialize(Token[] Tokens)
        {
            foreach (Token Token in Tokens)
                Serialize(Token);
        }
    }
}