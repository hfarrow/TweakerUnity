using UnityEngine;
using System.Collections;
using Ghostbit.Tweaker.UI;
using Ghostbit.Tweaker.Core;

namespace Ghostbit.Tweaker.Console
{
	public class TweakerConsole : MonoBehaviour
	{
		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();
		private Tweaker tweaker;
		private TreeView tree;

		void Awake()
		{

		}

		// Use this for initialization
		void Start()
		{

		}

		public void Init(Tweaker tweaker)
		{
			logger.Info("Init: " + tweaker);
			this.tweaker = tweaker;

			tree = new TreeView(tweaker);
			tree.BuildTree();
		}
	}
}