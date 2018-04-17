﻿//
//  Synuit.Toolkit - Application Architecture Tools - Patterns, Types and Components 
//  Copyright © 2012-2018 The Dude.  All Rights Reserved.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Synuit.Toolkit.DataStructures
{
   [XmlRoot("dictionary")]
   public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
   {

      private HashSet<TKey> _hashSet;// = new HashSet<TKey>();
      private bool _uniqueKey = false;


      public SerializableDictionary() : base() { }

      public SerializableDictionary(bool uniqueKey) : this()
      {
         init(uniqueKey);
      }
      private void init(bool uniqueKey)
      {
         _uniqueKey = uniqueKey;
         _hashSet = (_uniqueKey) ? new HashSet<TKey>() : null;
      }

      public bool UniqueKey { get { return _uniqueKey; } set { init(value); } }

      public new void Add(TKey key, TValue value)
      {
         bool add = false;
         if (_uniqueKey) { add = _hashSet.Add(key); } else { add = true; }

         if (add) { base.Add(key, value); }
      }



      #region IXmlSerializable Members

      public System.Xml.Schema.XmlSchema GetSchema() { return null; }

      public void ReadXml(System.Xml.XmlReader reader)
      {
         XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));

         XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

         bool wasEmpty = reader.IsEmptyElement;

         reader.Read();

         if (wasEmpty) return;

         while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
         {
            reader.ReadStartElement("item");

            reader.ReadStartElement("key");

            TKey key = (TKey)keySerializer.Deserialize(reader);

            reader.ReadEndElement();

            reader.ReadStartElement("value");

            TValue value = (TValue)valueSerializer.Deserialize(reader);

            reader.ReadEndElement();

            this.Add(key, value);

            reader.ReadEndElement();
            reader.MoveToContent();
         }

         reader.ReadEndElement();
      }

      public void WriteXml(System.Xml.XmlWriter writer)
      {

         XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
         XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

         foreach (TKey key in this.Keys)
         {

            writer.WriteStartElement("item");
            writer.WriteStartElement("key");

            keySerializer.Serialize(writer, key);

            writer.WriteEndElement();

            writer.WriteStartElement("value");

            TValue value = this[key];

            valueSerializer.Serialize(writer, value);

            writer.WriteEndElement();
            writer.WriteEndElement();
         }
      }
      #endregion
   }
}
