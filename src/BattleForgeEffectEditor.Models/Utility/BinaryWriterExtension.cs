using System;
using System.IO;

namespace BattleForgeEffectEditor.Models.Utility
{
    public static class BinaryWriterExtension
    {
        public static void WriteBfString(this BinaryWriter writer, BfString bfString)
        {
            if (bfString.Text == null || bfString.Text.Length == 0)
            {
                writer.Write(0);
            } else
            {
                writer.Write(bfString.Text.Length);
                writer.Write(bfString.Text);
            }
        }

        public static void Write(this BinaryWriter writer, Vector3 vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }

        public static void WriteEnumValue(this BinaryWriter writer, object enumValue)
        {
            writer.WriteStructureValue(enumValue, enumValue.GetType().GetEnumUnderlyingType());
        }

        public static void WriteStructureValue(this BinaryWriter writer, object value, Type type)
        {
            if (type == typeof(string))
                writer.WriteBfString((string)value);
            else if (type == typeof(sbyte))
                writer.Write((sbyte)value);
            else if (type == typeof(byte))
                writer.Write((byte)value);
            else if (type == typeof(bool))
                writer.Write((bool)value);
            else if (type == typeof(short))
                writer.Write((short)value);
            else if (type == typeof(ushort))
                writer.Write((ushort)value);
            else if (type == typeof(int))
                writer.Write((int)value);
            else if (type == typeof(uint))
                writer.Write((uint)value);
            else if (type == typeof(long))
                writer.Write((long)value);
            else if (type == typeof(ulong))
                writer.Write((ulong)value);
            else if (type == typeof(float))
                writer.Write((float)value);
            else
                throw new ArgumentException(type.FullName);
        }
    }
}
