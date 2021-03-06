﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Synuit.Toolkit.Infra.Data.Dapper.Mapper
{
   public class FallBackTypeMapper : SqlMapper.ITypeMap
   {
      private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;

      public FallBackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
      {
         _mappers = mappers;
      }

      public ConstructorInfo FindExplicitConstructor()
      {
         return _mappers.Select(m => m.FindExplicitConstructor())
             .FirstOrDefault(result => result != null);
      }

      public ConstructorInfo FindConstructor(string[] names, Type[] types)
      {
         foreach (var mapper in _mappers)
         {
            try
            {
               var result = mapper.FindConstructor(names, types);

               if (result != null)
               {
                  return result;
               }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (NotImplementedException nix)
#pragma warning restore CS0168 // Variable is declared but never used
            {
               // the CustomPropertyTypeMap only supports a no-args
               // constructor and throws a not implemented exception.
               // to work around that, catch and ignore.
            }
         }
         return null;
      }

      public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
      {
         foreach (var mapper in _mappers)
         {
            try
            {
               var result = mapper.GetConstructorParameter(constructor, columnName);

               if (result != null)
               {
                  return result;
               }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (NotImplementedException nix)
#pragma warning restore CS0168 // Variable is declared but never used
            {
               // the CustomPropertyTypeMap only supports a no-args
               // constructor and throws a not implemented exception.
               // to work around that, catch and ignore.
            }
         }
         return null;
      }

      public SqlMapper.IMemberMap GetMember(string columnName)
      {
         foreach (var mapper in _mappers)
         {
            try
            {
               var result = mapper.GetMember(columnName);

               if (result != null)
               {
                  return result;
               }
            }
            catch (NotImplementedException nix)
            {
               // the CustomPropertyTypeMap only supports a no-args
               // constructor and throws a not implemented exception.
               // to work around that, catch and ignore.
            }
         }
         return null;
      }
   }
}