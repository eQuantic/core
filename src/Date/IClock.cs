using System;

namespace eQuantic.Core.Date;

// ------------------------------------------------------------------------
// see http://stackoverflow.com/questions/43711/whats-a-good-way-to-overwrite-datetime-now-during-testing
public interface IClock
{

	// ----------------------------------------------------------------------
	DateTime Now { get; }

} // interface IClock

// namespace Itenso.TimePeriod
// -- EOF -------------------------------------------------------------------
