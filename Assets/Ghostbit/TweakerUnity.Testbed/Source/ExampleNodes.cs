using UnityEngine;
using System.Collections;
using Ghostbit.Tweaker.Core;
using System;

public class ExampleNodes
{
	public enum TestEnum
	{
		VALUE_A = 0,
		VALUE_B = 1,
		VALUE_C = 2,
		VALUE_D = 3
	}

	#region Root
	[Tweakable("Example.MyString", Description="A test string. Try changing the value!")]
	public static string root_string = "root string value";

	[Tweakable("Example.MyBoolean")]
	public static bool root_bool = false;

	[Tweakable("Example.MyInteger"),
	 NamedToggleValue("zero", 0, 0),
	 NamedToggleValue("eleven", 11, 1),
	 NamedToggleValue("hundred", 100, 2),
	 NamedToggleValue("leet", 1337, 3),
	 NamedToggleValue("lucky", 7, 4),
	 NamedToggleValue("pie", 8, 5),
	 NamedToggleValue("apple", 9, 6),
	 NamedToggleValue("donut", 10, 7),
	 NamedToggleValue("eagle", 11, 8)]
	public static int root_int = 0;

	[Tweakable("Example.MyFloat")]
	[StepSize(1.5f)]
	[Ghostbit.Tweaker.Core.RangeAttribute(1.0f, 200f)]
	public static float root_float = 1.0f;

	[Invokable("Example.MyCommand", Description = "This is a test command with args and void return type.")]
	public static void root_command(
		[ArgDescription("some random integer")] int myInt,
		[ArgDescription("some random string")] string myString,
		[ArgDescription("some random bool")] bool myBool)
	{
		LogManager.GetCurrentClassLogger().Trace("root_command invoked: {0}, {1}, {2}", myInt, myString, myBool);
	}

	[Invokable("Example.MyEvent", Description = "This is a test event with args. There are no listeners.")]
	public static event Action<int, string> root_event;

	[Tweakable("Example.MyEnum")]
	public static TestEnum root_enum = TestEnum.VALUE_A;

	#region Group Dog
	[Tweakable("Example.Dog.Name")]
	public static string dog_name = "Spot";

	[Tweakable("Example.Dog.Breed")]
	public static string dog_breed = "Shepard";

	[Tweakable("Example.Dog.Size")]
	public static int dog_size = 5;

	#region Group Dog Toys
	[Tweakable("Example.Dog.Toys.Favorite")]
	public static string dog_toys_favorite = "Bone";

	[Tweakable("Example.Dog.Toys.Largest")]
	public static string dog_toys_largest = "Teddy Bear";

	[Tweakable("Example.Dog.Toys.Smallest")]
	public static string dog_toys_smallest = "Rubber Ring";

	[Tweakable("Example.Dog.Toys.Cheapest")]
	public static string dog_toys_cheapest = "Rope";

	[Tweakable("Example.Dog.Toys.Placeholder1")]
	public static string dog_toys_placeholder1 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder2")]
	public static string dog_toys_placeholder2 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder3")]
	public static string dog_toys_placeholder3 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder4")]
	public static string dog_toys_placeholder4 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder5")]
	public static string dog_toys_placeholder5 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder6")]
	public static string dog_toys_placeholder6 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder7")]
	public static string dog_toys_placeholder7 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder8")]
	public static string dog_toys_placeholder8 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder9")]
	public static string dog_toys_placeholder9 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder10")]
	public static string dog_toys_placeholder10 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder11")]
	public static string dog_toys_placeholder11 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder12")]
	public static string dog_toys_placeholder12 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder13")]
	public static string dog_toys_placeholder13 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder14")]
	public static string dog_toys_placeholder14 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder15")]
	public static string dog_toys_placeholder15 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder16")]
	public static string dog_toys_placeholder16 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder17")]
	public static string dog_toys_placeholder17 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder18")]
	public static string dog_toys_placeholder18 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder19")]
	public static string dog_toys_placeholder19 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder20")]
	public static string dog_toys_placeholder20 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder21")]
	public static string dog_toys_placeholder21 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder22")]
	public static string dog_toys_placeholder22 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder23")]
	public static string dog_toys_placeholder23 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder24")]
	public static string dog_toys_placeholder24 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder25")]
	public static string dog_toys_placeholder25 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder26")]
	public static string dog_toys_placeholder26 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder27")]
	public static string dog_toys_placeholder27 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder28")]
	public static string dog_toys_placeholder28 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder29")]
	public static string dog_toys_placeholder29 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder30")]
	public static string dog_toys_placeholder30 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder31")]
	public static string dog_toys_placeholder31 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder32")]
	public static string dog_toys_placeholder32 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder33")]
	public static string dog_toys_placeholder33 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder34")]
	public static string dog_toys_placeholder34 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder35")]
	public static string dog_toys_placeholder35 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder36")]
	public static string dog_toys_placeholder36 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder37")]
	public static string dog_toys_placeholder37 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder38")]
	public static string dog_toys_placeholder38 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder39")]
	public static string dog_toys_placeholder39 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder40")]
	public static string dog_toys_placeholder40 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder41")]
	public static string dog_toys_placeholder41 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder42")]
	public static string dog_toys_placeholder42 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder43")]
	public static string dog_toys_placeholder43 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder44")]
	public static string dog_toys_placeholder44 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder45")]
	public static string dog_toys_placeholder45 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder46")]
	public static string dog_toys_placeholder46 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder47")]
	public static string dog_toys_placeholder47 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder48")]
	public static string dog_toys_placeholder48 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder49")]
	public static string dog_toys_placeholder49 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder50")]
	public static string dog_toys_placeholder50 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder51")]
	public static string dog_toys_placeholder51 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder52")]
	public static string dog_toys_placeholder52 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder53")]
	public static string dog_toys_placeholder53 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder54")]
	public static string dog_toys_placeholder54 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder55")]
	public static string dog_toys_placeholder55 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder56")]
	public static string dog_toys_placeholder56 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder57")]
	public static string dog_toys_placeholder57 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder58")]
	public static string dog_toys_placeholder58 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder59")]
	public static string dog_toys_placeholder59 = "placeholder";

	[Tweakable("Example.Dog.Toys.Placeholder60")]
	public static string dog_toys_placeholder60 = "placeholder";

	[Invokable("Example.Dog.Toys.PlayWithToys")]
	public static void dog_toys_playWithToys(string toysToPlayWith)
	{

	}
	#endregion // Group Dog Toys
	#endregion // Group Dog
	#endregion // Root

	static ExampleNodes()
	{
		root_event += on_root_event;
	}

	static void on_root_event(int arg1, string arg2)
	{
		LogManager.GetCurrentClassLogger().Trace("root_event invoked: {0}, {1}", arg1, arg2);
	}
}
