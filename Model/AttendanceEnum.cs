using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
	public enum ShiftClass
	{
		ONE_CLOCK_ON,
		ONE_CLOCK_OFF,
		TWO_CLOCK_ON,
		TWO_CLOCK_OFF,
		THREE_CLOCK_ON,
		THREE_CLOCK_OFF,
		VOIDANCE
	}

	public enum ClockState
	{
		NORMAL,
		EXCEPTION
	}

	public enum ClockMethod
	{
		LOG_FPVERIFY = 1,
		LOG_PASSVERIFY,
		LOG_CARDVERIFY,
		LOG_SIGNIN,
		LOG_FACEVERIFY
	}
}
