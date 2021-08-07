using System;
using System.IO;
using System.Text;

namespace BattleForgeEffectEditor.Models.Utility
{
    public static class BinaryReaderExtension
    {
        public static string ReadBfString(this BinaryReader reader)
        {
            int count = reader.ReadInt32();
            if (count == 0)
                return string.Empty;
            return Encoding.ASCII.GetString(reader.ReadBytes(count));
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            return new Vector3
            {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle(),
                Z = reader.ReadSingle(),
            };
        }

        public static T ReadEnumValue<T>(this BinaryReader reader)
        {
            Type type = typeof(T);
            return (T)Enum.ToObject(type, reader.ReadStructureValue(type.GetEnumUnderlyingType()));
        }

        public static object ReadStructureValue(this BinaryReader reader, Type type)
        {
            if (type == typeof(string))
                return (object)reader.ReadBfString();
            if (type == typeof(sbyte))
                return (object)reader.ReadSByte();
            if (type == typeof(byte))
                return (object)reader.ReadByte();
            if (type == typeof(bool))
                return (object)reader.ReadBoolean();
            if (type == typeof(ushort))
                return (object)reader.ReadUInt16();
            if (type == typeof(short))
                return (object)reader.ReadInt16();
            if (type == typeof(uint))
                return (object)reader.ReadUInt32();
            if (type == typeof(int))
                return (object)reader.ReadInt32();
            if (type == typeof(long))
                return (object)reader.ReadInt64();
            if (type == typeof(ulong))
                return (object)reader.ReadUInt64();
            if (type == typeof(float))
                return (object)reader.ReadSingle();
            throw new ArgumentException(type.FullName);
        }

        public static T Peek<T>(this BinaryReader reader) where T : struct
        {
            if (typeof(T).IsCustomValueType())
                throw new ArgumentException(typeof(T) + " not supported for Peek");
            long position = reader.BaseStream.Position;
            T value = (T)reader.ReadStructureValue(typeof(T));
            reader.BaseStream.Seek(position, SeekOrigin.Begin);
            return value;
        }
    }
}
