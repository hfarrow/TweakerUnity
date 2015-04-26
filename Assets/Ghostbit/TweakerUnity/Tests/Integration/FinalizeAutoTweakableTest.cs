using UnityEngine;
using System.Collections;
using System;
using Ghostbit.Tweaker.AssemblyScanner;

namespace Ghostbit.Tweaker.Core.Tests
{
	public class FinalizeAutoTweakableTest : MonoBehaviour
	{
		public class TestClass : IDisposable
		{
			[Tweakable("TestClass.AutoInt"),
			Range(0, 10)]
			public Tweakable<int> AutoInt = new Tweakable<int>();

			private bool disposeTweakable;

			public TestClass(bool disposeTweakable)
			{
				this.disposeTweakable = disposeTweakable;
				AutoTweakable.Bind(this);
			}

			public void Dispose()
			{
				if (disposeTweakable && AutoInt != null)
				{
					AutoInt.Dispose();
				}
				else
				{
					AutoInt = null;
				}
			}
		}

		private Tweaker tweaker;

		// Use this for initialization
		IEnumerator Start()
		{
			IScanner scanner = new Scanner();
			tweaker = new Tweaker();
			tweaker.Init(null, scanner);

			AutoTweakable.Manager = tweaker.Tweakables;

#pragma warning disable 0219 // is assigned but its value is never used
			TestClass testClass = new TestClass(false);
#pragma warning restore 0219

			ITweakable tweakable = null;
			tweakable = tweaker.Tweakables.GetTweakable(new SearchOptions("TestClass.AutoInt#"));
			IntegrationTest.Assert(tweakable != null);
			testClass = null;

			uint counter = 0;
			while(tweaker.Tweakables.GetTweakable(new SearchOptions("TestClass.AutoInt#")) != null)
			{
				GC.Collect();
				counter++;
				if(counter > 1000)
				{
					IntegrationTest.Fail("Failed to finalize AutoTweakable after " + counter + " frames.");
					yield break;
				}
				yield return null;
			}
			Debug.Log("Finalized AutoTweakable after " + counter + " frames.");
			IntegrationTest.Pass();
		}
	}
}