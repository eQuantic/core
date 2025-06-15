using System.Collections;

namespace eQuantic.Core.Date;

/// <summary>
/// Some hash utility methods for use in the implementation of value types
/// and collections.
/// </summary>
public static class HashTool
{

	/// <summary>
	/// Adds an object's hash code to an existing hash value.
	/// </summary>
	/// <param name="hash">The existing hash value.</param>
	/// <param name="obj">The object whose hash code to add.</param>
	/// <returns>The combined hash code.</returns>
	public static int AddHashCode( int hash, object obj )
	{
		int combinedHash = obj != null ? obj.GetHashCode() : nullValue;
		// if ( hash != 0 ) // perform this check to prevent FxCop warning 'op could overflow'
		// {
		combinedHash += hash * factor;
		// }
		return combinedHash;
	} // AddHashCode

	/// <summary>
	/// Adds a hash code to an existing hash value.
	/// </summary>
	/// <param name="hash">The existing hash value.</param>
	/// <param name="objHash">The hash code to add.</param>
	/// <returns>The combined hash code.</returns>
	public static int AddHashCode( int hash, int objHash )
	{
		int combinedHash = objHash;
		// if ( hash != 0 ) // perform this check to prevent FxCop warning 'op could overflow'
		// {
		combinedHash += hash * factor;
		// }
		return combinedHash;
	} // AddHashCode

	/// <summary>
	/// Computes the hash code for a single object.
	/// </summary>
	/// <param name="obj">The object to compute the hash code for.</param>
	/// <returns>The hash code of the object, or a null value if the object is null.</returns>
	public static int ComputeHashCode( object obj )
	{
		return obj != null ? obj.GetHashCode() : nullValue;
	} // ComputeHashCode

	/// <summary>
	/// Computes a combined hash code for multiple objects.
	/// </summary>
	/// <param name="objs">The objects to compute the hash code for.</param>
	/// <returns>The combined hash code of all objects.</returns>
	public static int ComputeHashCode( params object[] objs )
	{
		int hash = initValue;
		if ( objs != null )
		{
			foreach ( object obj in objs )
			{
				hash = hash * factor + ( obj != null ? obj.GetHashCode() : nullValue );
			}
		}
		return hash;
	} // ComputeHashCode

	/// <summary>
	/// Computes a combined hash code for all items in an enumerable collection.
	/// </summary>
	/// <param name="enumerable">The enumerable collection to compute the hash code for.</param>
	/// <returns>The combined hash code of all items in the collection.</returns>
	public static int ComputeHashCode( IEnumerable enumerable )
	{
		int hash = initValue;
		foreach ( object item in enumerable )
		{
			hash = hash * factor + ( item != null ? item.GetHashCode() : nullValue );
		}
		return hash;
	} // ComputeHashCode

	// ----------------------------------------------------------------------
	// members
	private const int nullValue = 0;
	private const int initValue = 1;
	private const int factor = 31;

} // class HashTool

// namespace Itenso.TimePeriod
// -- EOF -------------------------------------------------------------------
