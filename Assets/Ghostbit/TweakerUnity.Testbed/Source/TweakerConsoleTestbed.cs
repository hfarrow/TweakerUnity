using UnityEngine;
using System.Collections;
using Ghostbit.Tweaker.Core;

using CoreLogManager = Ghostbit.Tweaker.Core.LogManager;
using Ghostbit.Tweaker.AssemblyScanner;
using System.Reflection;
using System;

namespace Ghostbit.Tweaker.UI.Testbed
{
	public class TweakerConsoleTestbed : MonoBehaviour
	{
		public TweakerConsoleController ConsolePrefab;
		private TweakerConsoleController console;
		private ITweakerLogger logger;
		private Tweaker tweaker;

		void Awake()
		{
			CoreLogManager.Set(new LogManager());
			logger = CoreLogManager.GetCurrentClassLogger();
			logger.Info("Logger initialized");

			tweaker = new Tweaker();
			Scanner scanner = new Scanner();
			ScanOptions scanOptions = new ScanOptions();
			scanOptions.Assemblies.ScannableRefs = new Assembly[] { typeof(TweakerConsoleTestbed).Assembly };

			TweakerOptions tweakerOptions = new TweakerOptions();
			tweakerOptions.Flags =
				TweakerOptionFlags.DoNotAutoScan |
				TweakerOptionFlags.ScanForTweakables |
				TweakerOptionFlags.ScanForInvokables |
				TweakerOptionFlags.ScanForWatchables;
			tweaker.Init(tweakerOptions, scanner);
			tweaker.Scanner.Scan(scanOptions);

			console = Instantiate(ConsolePrefab) as TweakerConsoleController;
			logger.Info("console instatiated: " + console);
			console.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>(), false);
			logger.Info("console parented to testbed canvas");

			console.Init(tweaker);
		}

		void Start()
		{
			console.Refresh();
		}
	}
}