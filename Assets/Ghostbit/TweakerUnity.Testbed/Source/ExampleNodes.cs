using UnityEngine;
using System.Collections;
using Ghostbit.Tweaker.Core;

public class ExampleNodes
{
	#region Root
	[Tweakable("MyString")]
	public static string root_string = "root_string_value";

	[Tweakable("MyBoolean")]
	public static bool root_bool = false;

	[Tweakable("MyInteger")]
	public static int root_int = 1;

	[Tweakable("MyFloat")]
	public static float root_float = 1.0f;

	[Invokable("RoomsCheats.Something.MyCommand")]
	public static void root_command()
	{
		LogManager.GetCurrentClassLogger().Trace("root_command invoked");
	}

	#region Group Dog
	[Tweakable("Dog.Name")]
	public static string dog_name = "Spot";

	[Tweakable("Dog.Breed")]
	public static string dog_breed = "Shepard";

	[Tweakable("Dog.Size")]
	public static int dog_size = 5;

	#region Group Dog Toys
	[Tweakable("Dog.Toys.Favorite")]
	public static string dog_toys_favorite = "Bone";

	[Tweakable("Dog.Toys.Largest")]
	public static string dog_toys_largest = "Teddy Bear";

	[Tweakable("Dog.Toys.Smallest")]
	public static string dog_toys_smallest = "Rubber Ring";

	[Tweakable("Dog.Toys.Cheapest")]
	public static string dog_toys_cheapest = "Rope";

	[Tweakable("Dog.Toys.Placeholder1")]
	public static string dog_toys_placeholder1 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder2")]
	public static string dog_toys_placeholder2 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder3")]
	public static string dog_toys_placeholder3 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder4")]
	public static string dog_toys_placeholder4 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder5")]
	public static string dog_toys_placeholder5 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder6")]
	public static string dog_toys_placeholder6 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder7")]
	public static string dog_toys_placeholder7 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder8")]
	public static string dog_toys_placeholder8 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder9")]
	public static string dog_toys_placeholder9 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder10")]
	public static string dog_toys_placeholder10 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder11")]
	public static string dog_toys_placeholder11 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder12")]
	public static string dog_toys_placeholder12 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder13")]
	public static string dog_toys_placeholder13 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder14")]
	public static string dog_toys_placeholder14 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder15")]
	public static string dog_toys_placeholder15 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder16")]
	public static string dog_toys_placeholder16 = "placeholder";

	[Tweakable("Dog.Toys.Placeholder17")]
	public static string dog_toys_placeholder17 = "placeholder";

	[Invokable("Dog.Toys.PlayWithToys")]
	public static void dog_toys_playWithToys(string toysToPlayWith)
	{

	}
	#endregion // Group Dog Toys
	#endregion // Group Dog

	#region Group Cat
	[Tweakable("Cat.Name")]
	public static string cat_name = "Fluffy";

	[Tweakable("Cat.Breed")]
	public static string cat_breed = "Alley";

	[Tweakable("Cat.Size")]
	public static int cat_size = 2;
	#endregion // Group Cat

	#endregion // Root
}
