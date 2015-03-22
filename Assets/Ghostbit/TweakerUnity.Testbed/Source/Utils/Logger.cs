using UnityEngine;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;

namespace Ghostbit.Tweaker.Console.Testbed
{
	public class LogManager : ITweakerLogManager
	{
		private Dictionary<string, ITweakerLogger> loggers = new Dictionary<string, ITweakerLogger>();

		public ITweakerLogger GetLogger(string name)
		{
			ITweakerLogger logger = null;
			if(!loggers.TryGetValue(name, out logger))
			{
				logger = new Logger(name);
				loggers.Add(name, logger);
			}

			return logger;
		}
	}

	public class Logger : ITweakerLogger
	{
		private string name;

		public Logger(string name)
		{
			this.name = name;
		}

		public void Trace(string format, params object[] args)
		{
			UnityEngine.Debug.Log(string.Format("{0} | {1} | {2}", "TRACE", name, string.Format(format, args)));
		}

		public void Debug(string format, params object[] args)
		{
			UnityEngine.Debug.Log(string.Format("{0} | {1} | {2}", "DEBUG", name, string.Format(format, args)));
		}

		public void Info(string format, params object[] args)
		{
			UnityEngine.Debug.Log(string.Format("{0} | {1} | {2}", "INFO", name, string.Format(format, args)));
		}

		public void Warn(string format, params object[] args)
		{
			UnityEngine.Debug.LogWarning(string.Format("{0} | {1} | {2}", "WARN", name, string.Format(format, args)));
		}

		public void Error(string format, params object[] args)
		{
			UnityEngine.Debug.LogError(string.Format("{0} | {1} | {2}", "ERROR", name, string.Format(format, args)));
		}

		public void Fatal(string format, params object[] args)
		{
			UnityEngine.Debug.LogError(string.Format("{0} | {1} | {2}", "FATAL", name, string.Format(format, args)));
		}
	}
}