using System;

namespace eQuantic.Core.Date
{

	// ------------------------------------------------------------------------
	public class SystemClock : IClock
	{

		// ----------------------------------------------------------------------
		internal SystemClock()
		{
		} // SystemClock

		// ----------------------------------------------------------------------
		public DateTime Now
		{
			get { return DateTime.Now; }
		} // Now

	} // class SystemClock

} // namespace Itenso.TimePeriod
// -- EOF -------------------------------------------------------------------
