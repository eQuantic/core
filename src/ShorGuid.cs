using System;

namespace eQuantic.Core;

/// <summary>
/// Represents a globally unique identifier (GUID) with a 
/// shorter string value. Sguid
/// </summary>
public struct ShortGuid : IComparable, IComparable<ShortGuid>, IEquatable<ShortGuid>
{
	#region Static

	/// <summary>
	/// A read-only instance of the ShortGuid class whose value 
	/// is guaranteed to be all zeroes. 
	/// </summary>
	public static readonly ShortGuid Empty = new ShortGuid(Guid.Empty);

	#endregion

	#region Fields

	Guid _guid;
	string _value;

	#endregion

	#region Contructors

	/// <summary>
	/// Creates a ShortGuid from a base64 encoded string
	/// </summary>
	/// <param name="value">The encoded guid as a 
	/// base64 string</param>
	public ShortGuid(string value)
	{
		_value = value;
		_guid = Decode(value);
	}

	/// <summary>
	/// Creates a ShortGuid from a Guid
	/// </summary>
	/// <param name="guid">The Guid to encode</param>
	public ShortGuid(Guid guid)
	{
		_value = Encode(guid);
		_guid = guid;
	}

	#endregion

	#region Methods

	/// <summary>
	/// Returns the underlying Guid value.
	/// </summary>
	/// <returns>The Guid representation of this ShortGuid.</returns>
	public Guid ToGuid()
	{
		return _guid;
	}

	/// <summary>
	/// Sets the underlying Guid value and updates the string representation.
	/// </summary>
	/// <param name="value">The Guid value to set.</param>
	public void SetGuid(Guid value)
	{
		if (value != _guid)
		{
			_guid = value;
			_value = Encode(value);
		}
	}

	/// <summary>
	/// Sets the string value and updates the underlying Guid.
	/// </summary>
	/// <param name="value">The base64 encoded string value to set.</param>
	public void SetValue(string value)
	{
		if (value != _value)
		{
			_value = value;
			_guid = Decode(value);
		}
	}

	#endregion

	#region ToString

	/// <summary>
	/// Returns the base64 encoded guid as a string
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return _value;
	}

	#endregion

	#region Equals

	/// <summary>
	/// Returns a value indicating whether this instance and a 
	/// specified Object represent the same type and value.
	/// </summary>
	/// <param name="obj">The object to compare</param>
	/// <returns></returns>
	public override bool Equals(object obj)
	{
		if (obj is ShortGuid)
			return _guid.Equals(((ShortGuid)obj)._guid);
		if (obj is Guid)
			return _guid.Equals((Guid)obj);
		if (obj is string)
			return _guid.Equals(((ShortGuid)obj)._guid);
		return false;
	}

	#endregion

	#region GetHashCode

	/// <summary>
	/// Returns the HashCode for underlying Guid.
	/// </summary>
	/// <returns></returns>
	public override int GetHashCode()
	{
		return _guid.GetHashCode();
	}

	#endregion

	#region NewGuid

	/// <summary>
	/// Initialises a new instance of the ShortGuid class
	/// </summary>
	/// <returns></returns>
	public static ShortGuid NewGuid()
	{
		return new ShortGuid(Guid.NewGuid());
	}

	#endregion

	#region Encode

	/// <summary>
	/// Creates a new instance of a Guid using the string value, 
	/// then returns the base64 encoded version of the Guid.
	/// </summary>
	/// <param name="value">An actual Guid string (i.e. not a ShortGuid)</param>
	/// <returns></returns>
	public static string Encode(string value)
	{
		Guid guid = new Guid(value);
		return Encode(guid);
	}

	/// <summary>
	/// Encodes the given Guid as a base64 string that is 22 
	/// characters long.
	/// </summary>
	/// <param name="guid">The Guid to encode</param>
	/// <returns></returns>
	public static string Encode(Guid guid)
	{
		string encoded = Convert.ToBase64String(guid.ToByteArray());
		encoded = encoded
			.Replace("/", "_")
			.Replace("+", "-");
		return encoded.Substring(0, 22);
	}

	#endregion

	#region Decode

	/// <summary>
	/// Decodes the given base64 string
	/// </summary>
	/// <param name="value">The base64 encoded string of a Guid</param>
	/// <returns>A new Guid</returns>
	public static Guid Decode(string value)
	{
		value = value
			.Replace("_", "/")
			.Replace("-", "+");
		byte[] buffer = Convert.FromBase64String(value + "==");
		return new Guid(buffer);
	}

	#endregion

	#region Operators

	/// <summary>
	/// Determines if both ShortGuids have the same underlying 
	/// Guid value.
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static bool operator ==(ShortGuid x, ShortGuid y)
	{
		if ((object)x == null) return (object)y == null;
		return x._guid == y._guid;
	}

	/// <summary>
	/// Determines if both ShortGuids do not have the 
	/// same underlying Guid value.
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static bool operator !=(ShortGuid x, ShortGuid y)
	{
		return !(x == y);
	}

	/// <summary>
	/// Implicitly converts the ShortGuid to it's string equivilent
	/// </summary>
	/// <param name="shortGuid"></param>
	/// <returns></returns>
	public static implicit operator string(ShortGuid shortGuid)
	{
		return shortGuid._value;
	}

	/// <summary>
	/// Implicitly converts the ShortGuid to it's Guid equivilent
	/// </summary>
	/// <param name="shortGuid"></param>
	/// <returns></returns>
	public static implicit operator Guid(ShortGuid shortGuid)
	{
		return shortGuid._guid;
	}

	/// <summary>
	/// Implicitly converts the string to a ShortGuid
	/// </summary>
	/// <param name="shortGuid"></param>
	/// <returns></returns>
	public static implicit operator ShortGuid(string shortGuid)
	{
		return new ShortGuid(shortGuid);
	}

	/// <summary>
	/// Implicitly converts the Guid to a ShortGuid 
	/// </summary>
	/// <param name="guid"></param>
	/// <returns></returns>
	public static implicit operator ShortGuid(Guid guid)
	{
		return new ShortGuid(guid);
	}

	/// <summary>
	/// Compares this instance to a specified object and returns an indication of their relative values.
	/// </summary>
	/// <param name="obj">An object to compare, or null.</param>
	/// <returns>A signed number indicating the relative values of this instance and obj.</returns>
	public int CompareTo(object obj)
	{
		return _guid.CompareTo(((ShortGuid)obj).ToGuid());
	}

	/// <summary>
	/// Compares this instance to a specified ShortGuid object and returns an indication of their relative values.
	/// </summary>
	/// <param name="other">A ShortGuid object to compare to this instance.</param>
	/// <returns>A signed number indicating the relative values of this instance and other.</returns>
	public int CompareTo(ShortGuid other)
	{
		return _guid.CompareTo(other.ToGuid());
	}

	/// <summary>
	/// Returns a value indicating whether this instance and a specified ShortGuid object represent the same value.
	/// </summary>
	/// <param name="other">A ShortGuid object to compare to this instance.</param>
	/// <returns>true if other is equal to this instance; otherwise, false.</returns>
	public bool Equals(ShortGuid other)
	{
		return this == other;
	}
	#endregion
}