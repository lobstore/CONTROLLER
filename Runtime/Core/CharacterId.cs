using System;
using UnityEngine;

namespace LOGIYGames.CharacterController
{
    [Serializable]
    public struct CharacterId : IEquatable<CharacterId>
    {
        [SerializeField] private string value;

        public string Value { get { return value ?? string.Empty; } }
        public bool IsValid { get { return !string.IsNullOrWhiteSpace(value); } }

        public CharacterId(string value)
        {
            // Invalid serialized data is represented safely and can be reported
            // by validation; it must not throw from an Inspector getter.
            this.value = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        public bool Equals(CharacterId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CharacterId && Equals((CharacterId)obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(CharacterId left, CharacterId right) { return left.Equals(right); }
        public static bool operator !=(CharacterId left, CharacterId right) { return !left.Equals(right); }
    }
}