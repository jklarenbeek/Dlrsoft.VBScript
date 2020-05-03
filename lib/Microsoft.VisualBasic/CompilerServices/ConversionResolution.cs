// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualBasic.CompilerServices.ConversionResolution
// Assembly: Microsoft.VisualBasic, Version=10.0.4.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: F52897B6-9169-4970-91D8-A40CD1428CBD
// Assembly location: C:\Users\rkarman\.nuget\packages\microsoft.visualbasic\10.4.0-preview.18571.3\lib\netstandard2.0\Microsoft.VisualBasic.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
    internal class ConversionResolution
    {
        private static readonly ConversionResolution.ConversionClass[][] s_conversionTable = new ConversionResolution.ConversionClass[19][]
        {
      new ConversionResolution.ConversionClass[19],
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Widening
      },
      new ConversionResolution.ConversionClass[19],
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.None,
        ConversionResolution.ConversionClass.Identity,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing
      },
      new ConversionResolution.ConversionClass[19],
      new ConversionResolution.ConversionClass[19]
      {
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Widening,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Narrowing,
        ConversionResolution.ConversionClass.Bad,
        ConversionResolution.ConversionClass.Identity
      }
        };
        internal static readonly int[] NumericSpecificityRank = new int[19];
        internal static readonly TypeCode[][] ForLoopWidestTypeCode;

        static ConversionResolution()
        {
            ConversionResolution.NumericSpecificityRank[6] = 1;
            ConversionResolution.NumericSpecificityRank[5] = 2;
            ConversionResolution.NumericSpecificityRank[7] = 3;
            ConversionResolution.NumericSpecificityRank[8] = 4;
            ConversionResolution.NumericSpecificityRank[9] = 5;
            ConversionResolution.NumericSpecificityRank[10] = 6;
            ConversionResolution.NumericSpecificityRank[11] = 7;
            ConversionResolution.NumericSpecificityRank[12] = 8;
            ConversionResolution.NumericSpecificityRank[15] = 9;
            ConversionResolution.NumericSpecificityRank[13] = 10;
            ConversionResolution.NumericSpecificityRank[14] = 11;
            ConversionResolution.ForLoopWidestTypeCode = new TypeCode[19][]
            {
        new TypeCode[19],
        new TypeCode[19],
        new TypeCode[19],
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Int16,
          TypeCode.Empty,
          TypeCode.SByte,
          TypeCode.Int16,
          TypeCode.Int16,
          TypeCode.Int32,
          TypeCode.Int32,
          TypeCode.Int64,
          TypeCode.Int64,
          TypeCode.Decimal,
          TypeCode.Single,
          TypeCode.Double,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19],
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.SByte,
          TypeCode.Empty,
          TypeCode.SByte,
          TypeCode.Int16,
          TypeCode.Int16,
          TypeCode.Int32,
          TypeCode.Int32,
          TypeCode.Int64,
          TypeCode.Int64,
          TypeCode.Decimal,
          TypeCode.Single,
          TypeCode.Double,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Int16,
          TypeCode.Empty,
          TypeCode.Int16,
          TypeCode.Byte,
          TypeCode.Int16,
          TypeCode.UInt16,
          TypeCode.Int32,
          TypeCode.UInt32,
          TypeCode.Int64,
          TypeCode.UInt64,
          TypeCode.Single,
          TypeCode.Double,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Int16,
          TypeCode.Empty,
          TypeCode.Int16,
          TypeCode.Int16,
          TypeCode.Int16,
          TypeCode.Int32,
          TypeCode.Int32,
          TypeCode.Int64,
          TypeCode.Int64,
          TypeCode.Decimal,
          TypeCode.Single,
          TypeCode.Double,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Int32,
          TypeCode.Empty,
          TypeCode.Int32,
          TypeCode.UInt16,
          TypeCode.Int32,
          TypeCode.UInt16,
          TypeCode.Int32,
          TypeCode.UInt32,
          TypeCode.Int64,
          TypeCode.UInt64,
          TypeCode.Single,
          TypeCode.Double,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Int32,
          TypeCode.Empty,
          TypeCode.Int32,
          TypeCode.Int32,
          TypeCode.Int32,
          TypeCode.Int32,
          TypeCode.Int32,
          TypeCode.Int64,
          TypeCode.Int64,
          TypeCode.Decimal,
          TypeCode.Single,
          TypeCode.Double,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Int64,
          TypeCode.Empty,
          TypeCode.Int64,
          TypeCode.UInt32,
          TypeCode.Int64,
          TypeCode.UInt32,
          TypeCode.Int64,
          TypeCode.UInt32,
          TypeCode.Int64,
          TypeCode.UInt64,
          TypeCode.Single,
          TypeCode.Double,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Int64,
          TypeCode.Empty,
          TypeCode.Int64,
          TypeCode.Int64,
          TypeCode.Int64,
          TypeCode.Int64,
          TypeCode.Int64,
          TypeCode.Int64,
          TypeCode.Int64,
          TypeCode.Decimal,
          TypeCode.Single,
          TypeCode.Double,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Decimal,
          TypeCode.UInt64,
          TypeCode.Decimal,
          TypeCode.UInt64,
          TypeCode.Decimal,
          TypeCode.UInt64,
          TypeCode.Decimal,
          TypeCode.UInt64,
          TypeCode.Single,
          TypeCode.Double,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Single,
          TypeCode.Empty,
          TypeCode.Single,
          TypeCode.Single,
          TypeCode.Single,
          TypeCode.Single,
          TypeCode.Single,
          TypeCode.Single,
          TypeCode.Single,
          TypeCode.Single,
          TypeCode.Single,
          TypeCode.Double,
          TypeCode.Single,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Double,
          TypeCode.Empty,
          TypeCode.Double,
          TypeCode.Double,
          TypeCode.Double,
          TypeCode.Double,
          TypeCode.Double,
          TypeCode.Double,
          TypeCode.Double,
          TypeCode.Double,
          TypeCode.Double,
          TypeCode.Double,
          TypeCode.Double,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19]
        {
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Decimal,
          TypeCode.Decimal,
          TypeCode.Decimal,
          TypeCode.Decimal,
          TypeCode.Decimal,
          TypeCode.Decimal,
          TypeCode.Decimal,
          TypeCode.Decimal,
          TypeCode.Single,
          TypeCode.Double,
          TypeCode.Decimal,
          TypeCode.Empty,
          TypeCode.Empty,
          TypeCode.Empty
        },
        new TypeCode[19],
        new TypeCode[19],
        new TypeCode[19]
            };
        }

        internal static ConversionResolution.ConversionClass ClassifyConversion(
          Type targetType,
          Type sourceType,
          ref Symbols.Method operatorMethod)
        {
            ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyPredefinedConversion(targetType, sourceType);
            if (conversionClass == ConversionResolution.ConversionClass.None && !Symbols.IsInterface(sourceType) && !Symbols.IsInterface(targetType) && ((Symbols.IsClassOrValueType(sourceType) || Symbols.IsClassOrValueType(targetType)) && (!Symbols.IsIntrinsicType(sourceType) || !Symbols.IsIntrinsicType(targetType))))
                conversionClass = ConversionResolution.ClassifyUserDefinedConversion(targetType, sourceType, ref operatorMethod);
            return conversionClass;
        }

        internal static ConversionResolution.ConversionClass ClassifyIntrinsicConversion(
          TypeCode targetTypeCode,
          TypeCode sourceTypeCode)
        {
            return ConversionResolution.s_conversionTable[(int)targetTypeCode][(int)sourceTypeCode];
        }

        internal static ConversionResolution.ConversionClass ClassifyPredefinedCLRConversion(
          Type targetType,
          Type sourceType)
        {
            ConversionResolution.ConversionClass conversionClass;
            if ((object)targetType == (object)sourceType)
                conversionClass = ConversionResolution.ConversionClass.Identity;
            else if (Symbols.IsRootObjectType(targetType) || Symbols.IsOrInheritsFrom(sourceType, targetType))
                conversionClass = ConversionResolution.ConversionClass.Widening;
            else if (Symbols.IsRootObjectType(sourceType) || Symbols.IsOrInheritsFrom(targetType, sourceType))
                conversionClass = ConversionResolution.ConversionClass.Narrowing;
            else if (Symbols.IsInterface(sourceType))
            {
                conversionClass = Symbols.IsClass(targetType) || Symbols.IsArrayType(targetType) || Symbols.IsGenericParameter(targetType) ? ConversionResolution.ConversionClass.Narrowing : (!Symbols.IsInterface(targetType) ? (!Symbols.IsValueType(targetType) ? ConversionResolution.ConversionClass.Narrowing : (!Symbols.Implements(targetType, sourceType) ? ConversionResolution.ConversionClass.None : ConversionResolution.ConversionClass.Narrowing)) : ConversionResolution.ConversionClass.Narrowing);
            }
            else
            {
                if (Symbols.IsInterface(targetType))
                {
                    if (Symbols.IsArrayType(sourceType))
                    {
                        conversionClass = ConversionResolution.ClassifyCLRArrayToInterfaceConversion(targetType, sourceType);
                        goto label_32;
                    }
                    else if (Symbols.IsValueType(sourceType))
                    {
                        conversionClass = !Symbols.Implements(sourceType, targetType) ? ConversionResolution.ConversionClass.None : ConversionResolution.ConversionClass.Widening;
                        goto label_32;
                    }
                    else if (Symbols.IsClass(sourceType))
                    {
                        conversionClass = !Symbols.Implements(sourceType, targetType) ? ConversionResolution.ConversionClass.Narrowing : ConversionResolution.ConversionClass.Widening;
                        goto label_32;
                    }
                }
                if (Symbols.IsEnum(sourceType) || Symbols.IsEnum(targetType))
                    conversionClass = sourceType.GetTypeCode() != targetType.GetTypeCode() ? ConversionResolution.ConversionClass.None : (!Symbols.IsEnum(targetType) ? ConversionResolution.ConversionClass.Widening : ConversionResolution.ConversionClass.Narrowing);
                else if (Symbols.IsGenericParameter(sourceType))
                {
                    if (!Symbols.IsClassOrInterface(targetType))
                    {
                        conversionClass = ConversionResolution.ConversionClass.None;
                    }
                    else
                    {
                        Type[] interfaceConstraints = Symbols.GetInterfaceConstraints(sourceType);
                        int index = 0;
                        while (index < interfaceConstraints.Length)
                        {
                            Type sourceType1 = interfaceConstraints[index];
                            switch (ConversionResolution.ClassifyPredefinedConversion(targetType, sourceType1))
                            {
                                case ConversionResolution.ConversionClass.Identity:
                                case ConversionResolution.ConversionClass.Widening:
                                    conversionClass = ConversionResolution.ConversionClass.Widening;
                                    goto label_32;
                                default:
                                    checked { ++index; }
                                    continue;
                            }
                        }
                        Type classConstraint = Symbols.GetClassConstraint(sourceType);
                        if ((object)classConstraint != null)
                        {
                            switch (ConversionResolution.ClassifyPredefinedConversion(targetType, classConstraint))
                            {
                                case ConversionResolution.ConversionClass.Identity:
                                case ConversionResolution.ConversionClass.Widening:
                                    conversionClass = ConversionResolution.ConversionClass.Widening;
                                    goto label_32;
                            }
                        }
                        conversionClass = Interaction.IIf<ConversionResolution.ConversionClass>(Symbols.IsInterface(targetType), ConversionResolution.ConversionClass.Narrowing, ConversionResolution.ConversionClass.None);
                    }
                }
                else if (Symbols.IsGenericParameter(targetType))
                {
                    Type classConstraint = Symbols.GetClassConstraint(targetType);
                    conversionClass = (object)classConstraint == null || !Symbols.IsOrInheritsFrom(classConstraint, sourceType) ? ConversionResolution.ConversionClass.None : ConversionResolution.ConversionClass.Narrowing;
                }
                else
                    conversionClass = !Symbols.IsArrayType(sourceType) || !Symbols.IsArrayType(targetType) ? ConversionResolution.ConversionClass.None : (sourceType.GetArrayRank() != targetType.GetArrayRank() ? ConversionResolution.ConversionClass.None : ConversionResolution.ClassifyCLRConversionForArrayElementTypes(targetType.GetElementType(), sourceType.GetElementType()));
            }
        label_32:
            return conversionClass;
        }

        private static ConversionResolution.ConversionClass ClassifyCLRArrayToInterfaceConversion(
          Type targetInterface,
          Type sourceArrayType)
        {
            ConversionResolution.ConversionClass conversionClass1;
            if (Symbols.Implements(sourceArrayType, targetInterface))
                conversionClass1 = ConversionResolution.ConversionClass.Widening;
            else if (sourceArrayType.GetArrayRank() > 1)
            {
                conversionClass1 = ConversionResolution.ConversionClass.Narrowing;
            }
            else
            {
                Type elementType = sourceArrayType.GetElementType();
                ConversionResolution.ConversionClass conversionClass2 = ConversionResolution.ConversionClass.None;
                if (targetInterface.IsGenericType && !targetInterface.IsGenericTypeDefinition)
                {
                    Type genericTypeDefinition = targetInterface.GetGenericTypeDefinition();
                    if ((object)genericTypeDefinition == (object)typeof(IList<>) || (object)genericTypeDefinition == (object)typeof(ICollection<>) || (object)genericTypeDefinition == (object)typeof(IEnumerable<>))
                        conversionClass2 = ConversionResolution.ClassifyCLRConversionForArrayElementTypes(targetInterface.GetGenericArguments()[0], elementType);
                }
                else
                    conversionClass2 = ConversionResolution.ClassifyPredefinedCLRConversion(targetInterface, typeof(IList<>).MakeGenericType(elementType));
                conversionClass1 = conversionClass2 == ConversionResolution.ConversionClass.Identity || conversionClass2 == ConversionResolution.ConversionClass.Widening ? ConversionResolution.ConversionClass.Widening : ConversionResolution.ConversionClass.Narrowing;
            }
            return conversionClass1;
        }

        private static ConversionResolution.ConversionClass ClassifyCLRConversionForArrayElementTypes(
          Type targetElementType,
          Type sourceElementType)
        {
            ConversionResolution.ConversionClass conversionClass;
            if (Symbols.IsReferenceType(sourceElementType) && Symbols.IsReferenceType(targetElementType))
                conversionClass = ConversionResolution.ClassifyPredefinedCLRConversion(targetElementType, sourceElementType);
            else if (Symbols.IsValueType(sourceElementType) && Symbols.IsValueType(targetElementType))
            {
                conversionClass = ConversionResolution.ClassifyPredefinedCLRConversion(targetElementType, sourceElementType);
            }
            else
            {
                if (Symbols.IsGenericParameter(sourceElementType) && Symbols.IsGenericParameter(targetElementType))
                {
                    if ((object)sourceElementType == (object)targetElementType)
                    {
                        conversionClass = ConversionResolution.ConversionClass.Identity;
                        goto label_12;
                    }
                    else if (Symbols.IsReferenceType(sourceElementType) && Symbols.IsOrInheritsFrom(sourceElementType, targetElementType))
                    {
                        conversionClass = ConversionResolution.ConversionClass.Widening;
                        goto label_12;
                    }
                    else if (Symbols.IsReferenceType(targetElementType) && Symbols.IsOrInheritsFrom(targetElementType, sourceElementType))
                    {
                        conversionClass = ConversionResolution.ConversionClass.Narrowing;
                        goto label_12;
                    }
                }
                conversionClass = ConversionResolution.ConversionClass.None;
            }
        label_12:
            return conversionClass;
        }

        internal static ConversionResolution.ConversionClass ClassifyPredefinedConversion(
          Type targetType,
          Type sourceType)
        {
            ConversionResolution.ConversionClass conversionClass;
            if ((object)targetType == (object)sourceType)
            {
                conversionClass = ConversionResolution.ConversionClass.Identity;
            }
            else
            {
                TypeCode typeCode1 = sourceType.GetTypeCode();
                TypeCode typeCode2 = targetType.GetTypeCode();
                conversionClass = !Symbols.IsIntrinsicType(typeCode1) || !Symbols.IsIntrinsicType(typeCode2) ? (!Symbols.IsCharArrayRankOne(sourceType) || !Symbols.IsStringType(targetType) ? (!Symbols.IsCharArrayRankOne(targetType) || !Symbols.IsStringType(sourceType) ? ConversionResolution.ClassifyPredefinedCLRConversion(targetType, sourceType) : ConversionResolution.ConversionClass.Narrowing) : ConversionResolution.ConversionClass.Widening) : (!Symbols.IsEnum(targetType) || !Symbols.IsIntegralType(typeCode1) || !Symbols.IsIntegralType(typeCode2) ? (typeCode1 != typeCode2 || !Symbols.IsEnum(sourceType) ? ConversionResolution.ClassifyIntrinsicConversion(typeCode2, typeCode1) : ConversionResolution.ConversionClass.Widening) : ConversionResolution.ConversionClass.Narrowing);
            }
            return conversionClass;
        }

        private static List<Symbols.Method> CollectConversionOperators(
          Type targetType,
          Type sourceType,
          ref bool foundTargetTypeOperators,
          ref bool foundSourceTypeOperators)
        {
            if (Symbols.IsIntrinsicType(targetType))
                targetType = typeof(object);
            if (Symbols.IsIntrinsicType(sourceType))
                sourceType = typeof(object);
            List<Symbols.Method> methodList = Operators.CollectOperators(Symbols.UserDefinedOperator.Widen, targetType, sourceType, ref foundTargetTypeOperators, ref foundSourceTypeOperators);
            methodList.AddRange((IEnumerable<Symbols.Method>)Operators.CollectOperators(Symbols.UserDefinedOperator.Narrow, targetType, sourceType, ref foundTargetTypeOperators, ref foundSourceTypeOperators));
            return methodList;
        }

        private static bool Encompasses(Type larger, Type smaller)
        {
            ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyPredefinedConversion(larger, smaller);
            return conversionClass == ConversionResolution.ConversionClass.Widening || conversionClass == ConversionResolution.ConversionClass.Identity;
        }

        private static bool NotEncompasses(Type larger, Type smaller)
        {
            ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyPredefinedConversion(larger, smaller);
            return conversionClass == ConversionResolution.ConversionClass.Narrowing || conversionClass == ConversionResolution.ConversionClass.Identity;
        }

        private static Type MostEncompassing(List<Type> types)
        {
            Type type1 = types[0];
            int num = checked(types.Count - 1);
            int index = 1;
            Type type2;
            while (index <= num)
            {
                Type type3 = types[index];
                if (ConversionResolution.Encompasses(type3, type1))
                    type1 = type3;
                else if (!ConversionResolution.Encompasses(type1, type3))
                {
                    type2 = (Type)null;
                    goto label_8;
                }
                checked { ++index; }
            }
            type2 = type1;
        label_8:
            return type2;
        }

        private static Type MostEncompassed(List<Type> types)
        {
            Type type1 = types[0];
            int num = checked(types.Count - 1);
            int index = 1;
            Type type2;
            while (index <= num)
            {
                Type type3 = types[index];
                if (ConversionResolution.Encompasses(type1, type3))
                    type1 = type3;
                else if (!ConversionResolution.Encompasses(type3, type1))
                {
                    type2 = (Type)null;
                    goto label_8;
                }
                checked { ++index; }
            }
            type2 = type1;
        label_8:
            return type2;
        }

        private static void FindBestMatch(
          Type targetType,
          Type sourceType,
          List<Symbols.Method> searchList,
          List<Symbols.Method> resultList,
          ref bool genericMembersExistInList)
        {
            try
            {
                foreach (Symbols.Method search in searchList)
                {
                    MethodBase methodBase = search.AsMethod();
                    Type parameterType = methodBase.GetParameters()[0].ParameterType;
                    Type returnType = ((MethodInfo)methodBase).ReturnType;
                    if ((object)parameterType == (object)sourceType && (object)returnType == (object)targetType)
                        ConversionResolution.InsertInOperatorListIfLessGenericThanExisting(search, resultList, ref genericMembersExistInList);
                }
            }
            finally
            {
                List<Symbols.Method>.Enumerator enumerator;
                enumerator.Dispose();
            }
        }

        private static void InsertInOperatorListIfLessGenericThanExisting(
          Symbols.Method operatorToInsert,
          List<Symbols.Method> operatorList,
          ref bool genericMembersExistInList)
        {
            if (Symbols.IsGeneric(operatorToInsert.DeclaringType))
                genericMembersExistInList = true;
            if (genericMembersExistInList)
            {
                int index = checked(operatorList.Count - 1);
                while (index >= 0)
                {
                    Symbols.Method left = operatorList[index];
                    Symbols.Method method = OverloadResolution.LeastGenericProcedure(left, operatorToInsert);
                    if ((object)method == (object)left)
                        return;
                    if ((object)method != null)
                        operatorList.Remove(left);
                    checked { index += -1; }
                }
            }
            operatorList.Add(operatorToInsert);
        }

        private static List<Symbols.Method> ResolveConversion(
          Type targetType,
          Type sourceType,
          List<Symbols.Method> operatorSet,
          bool wideningOnly,
          ref bool resolutionIsAmbiguous)
        {
            resolutionIsAmbiguous = false;
            Type sourceType1 = (Type)null;
            Type targetType1 = (Type)null;
            bool genericMembersExistInList = false;
            List<Symbols.Method> methodList1 = new List<Symbols.Method>(operatorSet.Count);
            List<Symbols.Method> searchList = new List<Symbols.Method>(operatorSet.Count);
            List<Type> types1 = new List<Type>(operatorSet.Count);
            List<Type> types2 = new List<Type>(operatorSet.Count);
            List<Type> types3 = (List<Type>)null;
            List<Type> types4 = (List<Type>)null;
            if (!wideningOnly)
            {
                types3 = new List<Type>(operatorSet.Count);
                types4 = new List<Type>(operatorSet.Count);
            }
            try
            {
                foreach (Symbols.Method operatorToInsert in operatorSet)
                {
                    MethodBase method = operatorToInsert.AsMethod();
                    if (wideningOnly)
                    {
                        if (Symbols.IsNarrowingConversionOperator(method))
                            break;
                    }
                    Type parameterType = method.GetParameters()[0].ParameterType;
                    Type returnType = ((MethodInfo)method).ReturnType;
                    if (!Symbols.IsGeneric(method) && !Symbols.IsGeneric(method.DeclaringType) || ConversionResolution.ClassifyPredefinedConversion(returnType, parameterType) == ConversionResolution.ConversionClass.None)
                    {
                        if ((object)parameterType == (object)sourceType && (object)returnType == (object)targetType)
                            ConversionResolution.InsertInOperatorListIfLessGenericThanExisting(operatorToInsert, methodList1, ref genericMembersExistInList);
                        else if (methodList1.Count == 0)
                        {
                            if (ConversionResolution.Encompasses(parameterType, sourceType) && ConversionResolution.Encompasses(targetType, returnType))
                            {
                                searchList.Add(operatorToInsert);
                                if ((object)parameterType == (object)sourceType)
                                    sourceType1 = parameterType;
                                else
                                    types1.Add(parameterType);
                                if ((object)returnType == (object)targetType)
                                    targetType1 = returnType;
                                else
                                    types2.Add(returnType);
                            }
                            else if (!wideningOnly && ConversionResolution.Encompasses(parameterType, sourceType) && ConversionResolution.NotEncompasses(targetType, returnType))
                            {
                                searchList.Add(operatorToInsert);
                                if ((object)parameterType == (object)sourceType)
                                    sourceType1 = parameterType;
                                else
                                    types1.Add(parameterType);
                                if ((object)returnType == (object)targetType)
                                    targetType1 = returnType;
                                else
                                    types4.Add(returnType);
                            }
                            else if (!wideningOnly && ConversionResolution.NotEncompasses(parameterType, sourceType) && ConversionResolution.NotEncompasses(targetType, returnType))
                            {
                                searchList.Add(operatorToInsert);
                                if ((object)parameterType == (object)sourceType)
                                    sourceType1 = parameterType;
                                else
                                    types3.Add(parameterType);
                                if ((object)returnType == (object)targetType)
                                    targetType1 = returnType;
                                else
                                    types4.Add(returnType);
                            }
                        }
                    }
                }
            }
            finally
            {
                List<Symbols.Method>.Enumerator enumerator;
                enumerator.Dispose();
            }
            List<Symbols.Method> methodList2;
            if (methodList1.Count == 0 && searchList.Count > 0)
            {
                if ((object)sourceType1 == null)
                    sourceType1 = types1.Count <= 0 ? ConversionResolution.MostEncompassing(types3) : ConversionResolution.MostEncompassed(types1);
                if ((object)targetType1 == null)
                    targetType1 = types2.Count <= 0 ? ConversionResolution.MostEncompassed(types4) : ConversionResolution.MostEncompassing(types2);
                if ((object)sourceType1 == null || (object)targetType1 == null)
                {
                    resolutionIsAmbiguous = true;
                    methodList2 = new List<Symbols.Method>();
                    goto label_44;
                }
                else
                    ConversionResolution.FindBestMatch(targetType1, sourceType1, searchList, methodList1, ref genericMembersExistInList);
            }
            if (methodList1.Count > 1)
                resolutionIsAmbiguous = true;
            methodList2 = methodList1;
        label_44:
            return methodList2;
        }

        internal static ConversionResolution.ConversionClass ClassifyUserDefinedConversion(
          Type targetType,
          Type sourceType,
          ref Symbols.Method operatorMethod)
        {
            ConversionResolution.ConversionClass conversionClass;
            lock ((object)ConversionResolution.OperatorCaches.ConversionCache)
            {
                if (ConversionResolution.OperatorCaches.UnconvertibleTypeCache.Lookup(targetType) && ConversionResolution.OperatorCaches.UnconvertibleTypeCache.Lookup(sourceType))
                {
                    conversionClass = ConversionResolution.ConversionClass.None;
                    goto label_20;
                }
                else
                {
                    ConversionResolution.ConversionClass classification = ConversionClass.Ambiguous;
                    if (ConversionResolution.OperatorCaches.ConversionCache.Lookup(targetType, sourceType, ref classification, ref operatorMethod))
                    {
                        conversionClass = classification;
                        goto label_20;
                    }
                }
            }
            bool foundTargetTypeOperators = false;
            bool foundSourceTypeOperators = false;
            ConversionResolution.ConversionClass classification1 = ConversionResolution.DoClassifyUserDefinedConversion(targetType, sourceType, ref operatorMethod, ref foundTargetTypeOperators, ref foundSourceTypeOperators);
            lock ((object)ConversionResolution.OperatorCaches.ConversionCache)
            {
                if (!foundTargetTypeOperators)
                    ConversionResolution.OperatorCaches.UnconvertibleTypeCache.Insert(targetType);
                if (!foundSourceTypeOperators)
                    ConversionResolution.OperatorCaches.UnconvertibleTypeCache.Insert(sourceType);
                if (!foundTargetTypeOperators)
                {
                    if (!foundSourceTypeOperators)
                        goto label_19;
                }
                ConversionResolution.OperatorCaches.ConversionCache.Insert(targetType, sourceType, classification1, operatorMethod);
            }
        label_19:
            conversionClass = classification1;
        label_20:
            return conversionClass;
        }

        private static ConversionResolution.ConversionClass DoClassifyUserDefinedConversion(
          Type targetType,
          Type sourceType,
          ref Symbols.Method operatorMethod,
          ref bool foundTargetTypeOperators,
          ref bool foundSourceTypeOperators)
        {
            operatorMethod = (Symbols.Method)null;
            List<Symbols.Method> operatorSet = ConversionResolution.CollectConversionOperators(targetType, sourceType, ref foundTargetTypeOperators, ref foundSourceTypeOperators);
            ConversionResolution.ConversionClass conversionClass;
            if (operatorSet.Count == 0)
            {
                conversionClass = ConversionResolution.ConversionClass.None;
            }
            else
            {
                bool resolutionIsAmbiguous = false;
                List<Symbols.Method> methodList1 = ConversionResolution.ResolveConversion(targetType, sourceType, operatorSet, true, ref resolutionIsAmbiguous);
                if (methodList1.Count == 1)
                {
                    operatorMethod = methodList1[0];
                    operatorMethod.ArgumentsValidated = true;
                    conversionClass = ConversionResolution.ConversionClass.Widening;
                }
                else
                {
                    if (methodList1.Count == 0 && !resolutionIsAmbiguous)
                    {
                        List<Symbols.Method> methodList2 = ConversionResolution.ResolveConversion(targetType, sourceType, operatorSet, false, ref resolutionIsAmbiguous);
                        if (methodList2.Count == 1)
                        {
                            operatorMethod = methodList2[0];
                            operatorMethod.ArgumentsValidated = true;
                            conversionClass = ConversionResolution.ConversionClass.Narrowing;
                            goto label_10;
                        }
                        else if (methodList2.Count == 0)
                        {
                            conversionClass = ConversionResolution.ConversionClass.None;
                            goto label_10;
                        }
                    }
                    conversionClass = ConversionResolution.ConversionClass.Ambiguous;
                }
            }
        label_10:
            return conversionClass;
        }

        internal enum ConversionClass : sbyte
        {
            Bad,
            Identity,
            Widening,
            Narrowing,
            None,
            Ambiguous,
        }

        internal class OperatorCaches
        {
            internal static readonly ConversionResolution.OperatorCaches.FixedList ConversionCache = new ConversionResolution.OperatorCaches.FixedList();
            internal static readonly ConversionResolution.OperatorCaches.FixedExistanceList UnconvertibleTypeCache = new ConversionResolution.OperatorCaches.FixedExistanceList();

            internal sealed class FixedList
            {
                private readonly ConversionResolution.OperatorCaches.FixedList.Entry[] _list;
                private readonly int _size;
                private int _first;
                private int _last;
                private int _count;

                internal FixedList()
                  : this(50)
                {
                }

                internal FixedList(int size)
                {
                    this._size = size;
                    this._list = new ConversionResolution.OperatorCaches.FixedList.Entry[checked(this._size - 1 + 1)];
                    int num = checked(this._size - 2);
                    int index1 = 0;
                    while (index1 <= num)
                    {
                        this._list[index1].Next = checked(index1 + 1);
                        checked { ++index1; }
                    }
                    int index2 = checked(this._size - 1);
                    while (index2 >= 1)
                    {
                        this._list[index2].Previous = checked(index2 - 1);
                        checked { index2 += -1; }
                    }
                    this._list[0].Previous = checked(this._size - 1);
                    this._last = checked(this._size - 1);
                }

                private void MoveToFront(int item)
                {
                    if (item == this._first)
                        return;
                    int next = this._list[item].Next;
                    int previous = this._list[item].Previous;
                    this._list[previous].Next = next;
                    this._list[next].Previous = previous;
                    this._list[this._first].Previous = item;
                    this._list[this._last].Next = item;
                    this._list[item].Next = this._first;
                    this._list[item].Previous = this._last;
                    this._first = item;
                }

                internal void Insert(
                  Type targetType,
                  Type sourceType,
                  ConversionResolution.ConversionClass classification,
                  Symbols.Method operatorMethod)
                {
                    if (this._count < this._size)
                    {
                        this._count++;
                    }
                    int last = this._last;
                    this._first = last;
                    this._last = this._list[this._last].Previous;
                    this._list[last].TargetType = targetType;
                    this._list[last].SourceType = sourceType;
                    this._list[last].Classification = classification;
                    this._list[last].OperatorMethod = operatorMethod;
                }

                internal bool Lookup(
                  Type targetType,
                  Type sourceType,
                  ref ConversionResolution.ConversionClass classification,
                  ref Symbols.Method operatorMethod)
                {
                    int index = this._first;
                    int num = 0;
                    bool flag;
                    while (num < this._count)
                    {
                        if ((object)targetType == (object)this._list[index].TargetType && (object)sourceType == (object)this._list[index].SourceType)
                        {
                            classification = this._list[index].Classification;
                            operatorMethod = this._list[index].OperatorMethod;
                            this.MoveToFront(index);
                            flag = true;
                            goto label_6;
                        }
                        else
                        {
                            index = this._list[index].Next;
                            checked { ++num; }
                        }
                    }
                    classification = ConversionResolution.ConversionClass.Bad;
                    operatorMethod = (Symbols.Method)null;
                    flag = false;
                label_6:
                    return flag;
                }

                private struct Entry
                {
                    internal Type TargetType;
                    internal Type SourceType;
                    internal ConversionResolution.ConversionClass Classification;
                    internal Symbols.Method OperatorMethod;
                    internal int Next;
                    internal int Previous;
                }
            }

            internal sealed class FixedExistanceList
            {
                private readonly ConversionResolution.OperatorCaches.FixedExistanceList.Entry[] _list;
                private readonly int _size;
                private int _first;
                private int _last;
                private int _count;

                internal FixedExistanceList()
                  : this(50)
                {
                }

                internal FixedExistanceList(int size)
                {
                    this._size = size;
                    this._list = new ConversionResolution.OperatorCaches.FixedExistanceList.Entry[checked(this._size - 1 + 1)];
                    int num = checked(this._size - 2);
                    int index1 = 0;
                    while (index1 <= num)
                    {
                        this._list[index1].Next = checked(index1 + 1);
                        checked { ++index1; }
                    }
                    int index2 = checked(this._size - 1);
                    while (index2 >= 1)
                    {
                        this._list[index2].Previous = checked(index2 - 1);
                        checked { index2 += -1; }
                    }
                    this._list[0].Previous = checked(this._size - 1);
                    this._last = checked(this._size - 1);
                }

                private void MoveToFront(int item)
                {
                    if (item == this._first)
                        return;
                    int next = this._list[item].Next;
                    int previous = this._list[item].Previous;
                    this._list[previous].Next = next;
                    this._list[next].Previous = previous;
                    this._list[this._first].Previous = item;
                    this._list[this._last].Next = item;
                    this._list[item].Next = this._first;
                    this._list[item].Previous = this._last;
                    this._first = item;
                }

                internal void Insert(Type type)
                {
                    if (this._count < this._size)
                    {
                        _count++;
                    }
                    int last = this._last;
                    this._first = last;
                    this._last = this._list[this._last].Previous;
                    this._list[last].Type = type;
                }

                internal bool Lookup(Type type)
                {
                    int index = this._first;
                    int num = 0;
                    bool flag;
                    while (num < this._count)
                    {
                        if ((object)type == (object)this._list[index].Type)
                        {
                            this.MoveToFront(index);
                            flag = true;
                            goto label_6;
                        }
                        else
                        {
                            index = this._list[index].Next;
                            checked { ++num; }
                        }
                    }
                    flag = false;
                label_6:
                    return flag;
                }

                private struct Entry
                {
                    internal Type Type;
                    internal int Next;
                    internal int Previous;
                }
            }
        }
    }
}
