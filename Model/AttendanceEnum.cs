using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack_Pro.Model
{
	public enum ShiftClass
	{
		/// <summary>
		/// 班段1，上班
		/// </summary>
		ONE_CLOCK_ON,
		/// <summary>
		/// 班段1，下班
		/// </summary>
		ONE_CLOCK_OFF,
		/// <summary>
		/// 班段2，上班
		/// </summary>
		TWO_CLOCK_ON,
		/// <summary>
		/// 班段2，下班
		/// </summary>
		TWO_CLOCK_OFF,
		/// <summary>
		/// 班段3，上班
		/// </summary>
		THREE_CLOCK_ON,
		/// <summary>
		/// 班段3，下班
		/// </summary>
		THREE_CLOCK_OFF,
		/// <summary>
		/// 无效
		/// </summary>
		VOIDANCE
	}

	public enum ClockState
	{
		/// <summary>
		/// 正常
		/// </summary>
		NORMAL,
		/// <summary>
		/// 异常
		/// </summary>
		EXCEPTION
	}

	public enum ClockMethod
	{
		/// <summary>
		/// 指纹机
		/// </summary>
		LOG_FPVERIFY = 1,
		/// <summary>
		/// 
		/// </summary>
		LOG_PASSVERIFY,
		/// <summary>
		/// 
		/// </summary>
		LOG_CARDVERIFY,
		/// <summary>
		/// 
		/// </summary>
		LOG_SIGNIN,
		/// <summary>
		/// 人脸机
		/// </summary>
		LOG_FACEVERIFY
	}
}
