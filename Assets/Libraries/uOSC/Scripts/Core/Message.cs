using System;
using System.IO;
using UnityEngine;

namespace uOSC
{
    public struct Message
    {
        public string Address;
        public Timestamp Timestamp;
        public object[] Values;

        public static Message None
        {
            get { return new Message(""); }
        }

        public Message(string address, params object[] packet)
        {
            Address = address;
            Timestamp = new Timestamp();
            Values = packet;
        }

        public void Write(MemoryStream stream)
        {
            WriteAddress(stream);
            WriteTypes(stream);
            WriteValues(stream);
        }

        private void WriteAddress(MemoryStream stream)
        {
            Writer.Write(stream, Address);
        }

        private void WriteTypes(MemoryStream stream)
        {
            var types = ",";
            foreach (var value in Values)
            {
                if (value is int) types += Identifier.Int;
                else if (value is float) types += Identifier.Float;
                else if (value is string) types += Identifier.String;
                else if (value is byte[]) types += Identifier.Blob;
            }

            Writer.Write(stream, types);
        }

        private void WriteValues(MemoryStream stream)
        {
            foreach (var value in Values)
            {
                if (value is int) Writer.Write(stream, (int) value);
                else if (value is float) Writer.Write(stream, (float) value);
                else if (value is string) Writer.Write(stream, (string) value);
                else if (value is byte[]) Writer.Write(stream, (byte[]) value);
            }
        }

        public string GetString(int index)
        {
            if (Values[index] is string)
            {
                var data = (string) Values[index];
                return string.IsNullOrEmpty(data) ? "" : data;
            }

            Debug.Log("Wrong type");
            return "" ;
        }
        
        public int GetInt(int index)
        {
            if (Values[index] is int)
            {
                var data = (int) Values[index];
                if (Double.IsNaN(data)) return 0;
                return data;
            }

            if (Values[index] is float)
            {
                var data = (int) ((float) Values[index]);
                if (Double.IsNaN(data)) return 0;
                return data;
            }
            
            Debug.Log("Wrong type");
            return 0;
        }

        public float GetFloat(int index)
        {
            if (Values[index] is int)
            {
                float data = (int) Values[index];
                if (Double.IsNaN(data)) return 0f;
                return data;
            }

            if (Values[index] is float)
            {
                var data = (float) Values[index];
                if (Double.IsNaN(data)) return 0f;
                return data;
            }

            Debug.Log("Wrong type");
            return 0f;
        }
    }
}