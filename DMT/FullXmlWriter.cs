using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DMT
{
    public class FullElementXmlWriterDecorator : XmlWriterDecorator
    {
        public FullElementXmlWriterDecorator(XmlWriter baseWriter) : base(baseWriter) { }

        public override void WriteEndElement()
        {
            base.WriteFullEndElement();
        }
    }

    public class XmlWriterDecorator : XmlWriter
    {
        readonly XmlWriter baseWriter;

        public XmlWriterDecorator(XmlWriter baseWriter)
        {
            if (baseWriter == null)
                throw new ArgumentNullException();
            this.baseWriter = baseWriter;
        }

        protected virtual bool IsSuspended { get { return false; } }

        public override void Close()
        {
            baseWriter.Close();
        }

        public override void Flush()
        {
            baseWriter.Flush();
        }

        public override string LookupPrefix(string ns)
        {
            return baseWriter.LookupPrefix(ns);
        }

        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteBase64(buffer, index, count);
        }

        public override void WriteCData(string text)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteCData(text);
        }

        public override void WriteCharEntity(char ch)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteCharEntity(ch);
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteChars(buffer, index, count);
        }

        public override void WriteComment(string text)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteComment(text);
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteDocType(name, pubid, sysid, subset);
        }

        public override void WriteEndAttribute()
        {
            if (IsSuspended)
                return;
            baseWriter.WriteEndAttribute();
        }

        public override void WriteEndDocument()
        {
            if (IsSuspended)
                return;
            baseWriter.WriteEndDocument();
        }

        public override void WriteEndElement()
        {
            if (IsSuspended)
                return;
            baseWriter.WriteEndElement();
        }

        public override void WriteEntityRef(string name)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteEntityRef(name);
        }

        public override void WriteFullEndElement()
        {
            if (IsSuspended)
                return;
            baseWriter.WriteFullEndElement();
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteProcessingInstruction(name, text);
        }

        public override void WriteRaw(string data)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteRaw(data);
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteRaw(buffer, index, count);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteStartDocument(bool standalone)
        {
            baseWriter.WriteStartDocument(standalone);
        }

        public override void WriteStartDocument()
        {
            baseWriter.WriteStartDocument();
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteStartElement(prefix, localName, ns);
        }

        public override WriteState WriteState
        {
            get { return baseWriter.WriteState; }
        }

        public override void WriteString(string text)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteString(text);
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteSurrogateCharEntity(lowChar, highChar);
        }

        public override void WriteWhitespace(string ws)
        {
            if (IsSuspended)
                return;
            baseWriter.WriteWhitespace(ws);
        }
    }
}
