using System;
using System.Text;

namespace BattleForgeEffectEditor.Models.Utility
{
    public struct BfString : IComparable<BfString>
    {
        public byte[] Text { get; private set; }

        public BfString(string s = "")
        {
            Text = s != null ? Encoding.UTF8.GetBytes(s) : new byte[0];
        }

        public static implicit operator BfString(string s) => new BfString(s);

        public static implicit operator string(BfString s) => s.ToString();

        public override int GetHashCode()
        {
            if (Text == null)
                Text = new byte[0];
            return Text.ToString().GetHashCode();
        }

        public override string ToString() => Text == null ? "" : Encoding.UTF8.GetString(Text);

        public int CompareTo(BfString other)
        {
            return string.Compare(ToString(), other.ToString(), StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BfString))
                return false;

            BfString otherString = (BfString)obj;
            if (Text.Length != otherString.Text.Length)
                return false;

            for (int i = 0; i < Text.Length; i++)
            {
                int thisChar = char.ToLower((char)Text[i]);
                int otherChar = char.ToLower((char)otherString.Text[i]);
                if (thisChar != otherChar)
                    return false;
            }

            return true;
        }
    }
}
