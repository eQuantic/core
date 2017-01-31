using System.Collections;

namespace eQuantic.Core.Date
{

	/// <summary>
	/// Some hash utility methods for use in the implementation of value types
	/// and collections.
	/// </summary>
	public static class HashTool
	{

		// ----------------------------------------------------------------------
		public static int AddHashCode( int hash, object obj )
		{
			int combinedHash = obj != null ? obj.GetHashCode() : nullValue;
			// if ( hash != 0 ) // perform this check to prevent FxCop warning 'op could overflow'
			// {
			combinedHash += hash * factor;
			// }
			return combinedHash;
		} // AddHashCode

		// ----------------------------------------------------------------------
		public static int AddHashCode( int hash, int objHash )
		{
			int combinedHash = objHash;
			// if ( hash != 0 ) // perform this check to prevent FxCop warning 'op could overflow'
			// {
			combinedHash += hash * factor;
			// }
			return combinedHash;
		} // AddHashCode

		// ----------------------------------------------------------------------
		public static int ComputeHashCode( object obj )
		{
			return obj != null ? obj.GetHashCode() : nullValue;
		} // ComputeHashCode

		// ----------------------------------------------------------------------
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

		// ----------------------------------------------------------------------
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

} // namespace Itenso.TimePeriod
// -- EOF -------------------------------------------------------------------
